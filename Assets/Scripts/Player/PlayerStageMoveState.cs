using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    private const float FADE_DURATION = 2.0f;

    /// <summary>
    /// 出てくる方のゲート
    /// </summary>
    private GameObject[] exitGate;

    public PlayerStageMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, GameObject[] _exitGate) : base(_player, _stateMachine, _animBoolName)
    {
        exitGate = _exitGate;

        foreach (var gate in exitGate)
        {
            Transform gateChildren = gate.GetComponentInChildren<Transform>();

            foreach (Transform gateChild in gateChildren)
            {
                gateChild.DOScale(0, 0);
            }
            
        }
    }

    public override async void Enter()
    {
        base.Enter();
        defaultGravity = player.rb.gravityScale;

        stateTimer = flyTime;
        rb.gravityScale = 0;
        renderer = player.playerRenderer;

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
            )
            .Append(renderer.DOFade(0f, FADE_DURATION))
            .Append(player.transform.DOLocalMove(
                        exitGate[player.gateNumber].transform.position,
                        0f
                    )
            )
            .Append(renderer.DOFade(1f, FADE_DURATION));

        await sequence.AsyncWaitForCompletion();

        stateMachine.ChangeState(player.airState);
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
