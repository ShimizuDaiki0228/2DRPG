using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerStageMoveState : PlayerState
{
    private float flyTime = 1f;

    private float defaultGravity;

    /// <summary>
    /// プレイヤーの画像
    /// </summary>
    private SpriteRenderer renderer;

    /// <summary>
    /// プレイヤーが消えるまでにかかる時間
    /// </summary>
    private const float DELAY_FADE_DURATION = 2.0f;

    /// <summary>
    /// プレイヤーが消えるまでにかかる時間
    /// </summary>
    private const float FADE_DURATION = 1.0f;

    /// <summary>
    /// 画面がフェードアウトするのを待つ時間
    /// 実際にはこの値を変えることでフェードアウトの時間が変わるわけではない
    /// </summary>
    private const float UI_FADEOUT_DURATION = 2.0f;

    /// <summary>
    /// 画面がフェードインするのを待つ時間
    /// 実際にはこの値を変えることでフェードインの時間が変わるわけではない
    /// </summary>
    private const float UI_FADEIN_DURATION = 0.3f;

    /// <summary>
    /// 出てくる方のゲート
    /// </summary>
    private GameObject[] _exitGate;

    private Subject<string> onFadeSubject = new Subject<string>();
    public IObservable<string> OnFadeObservable => onFadeSubject.AsObservable();

    public PlayerStageMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, GameObject[] _exitGate) : base(_player, _stateMachine, _animBoolName)
    {
        this._exitGate = _exitGate;

        foreach (var gate in this._exitGate)
        {
            ChildDOScale(gate, 0 , 0);

        }
    }

    /// <summary>
    /// 子要素の大きさを変更する
    /// ゲートは子要素として存在するため
    /// </summary>
    /// <param name="gate"></param>
    private async UniTask ChildDOScale(GameObject gate, float scale , float duration)
    {
        Transform[] gateChildren = gate.GetComponentsInChildren<Transform>();

        List<UniTask> tasks = new List<UniTask>();
        foreach (Transform gateChild in gateChildren)
        {
            // 各アニメーションを開始し、タスクをリストに追加
            // ここではawaitしないため、すぐに次のアニメーションに進む
            tasks.Add(
                gateChild.DOScale(scale, duration)
                .SetEase(Ease.OutCirc)
                .ToUniTask());
        }

        // 全てのアニメーションが完了するまで待機
        await UniTask.WhenAll(tasks);
    }

    public override async void Enter()
    {
        base.Enter();
        defaultGravity = player.rb.gravityScale;

        stateTimer = flyTime;
        rb.gravityScale = 0;
        renderer = player.playerRenderer;

        Sequence beforeMoveSequence = FloatingAnimationSequence();

        await beforeMoveSequence.AsyncWaitForCompletion();

        renderer.DOFade(0f, FADE_DURATION);
        onFadeSubject.OnNext("fadeOut");
        await UniTask.WaitForSeconds(UI_FADEOUT_DURATION);


        onFadeSubject.OnNext("fadeIn");
        await UniTask.WaitForSeconds(UI_FADEIN_DURATION);

        GameObject exitGate = _exitGate[player.gateNumber];


        player.transform.DOLocalMove(
                        exitGate.transform.position + new Vector3(0, -1, 0),
                        0f
        );

        await UniTask.WaitForSeconds(1);

        await ArrivalAnimationSequence(exitGate);

        
        stateMachine.ChangeState(player.airState);
    }

    /// <summary>
    /// プレイヤーがゲートに触れて浮くアニメーションシーケンス
    /// </summary>
    /// <returns></returns>
    private Sequence FloatingAnimationSequence()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(player.transform.DOLocalMoveY(
                        player.transform.localPosition.y + 2.5f,
                        flyTime
                )
                .SetEase(Ease.OutCirc)
            )
            .Append(player.transform.DOLocalMoveY(
                        player.transform.localPosition.y + 2.3f,
                        DELAY_FADE_DURATION
                    )
            );

        return sequence;
    }

    /// <summary>
    /// ゲートに触れて到着した後のアニメーションシーケンス
    /// </summary>
    /// <returns></returns>
    private async UniTask ArrivalAnimationSequence(GameObject exitGate)
    {
        await ChildDOScale(exitGate, 1, 2);

        await renderer.DOFade(1f, FADE_DURATION).ToUniTask();

        await ChildDOScale(exitGate, 0, 2);
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        

        
    }
}
