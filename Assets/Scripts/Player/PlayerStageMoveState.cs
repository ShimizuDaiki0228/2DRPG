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
    /// �v���C���[�̉摜
    /// </summary>
    private SpriteRenderer renderer;

    /// <summary>
    /// �v���C���[��������܂łɂ����鎞��
    /// </summary>
    private const float DELAY_FADE_DURATION = 2.0f;

    /// <summary>
    /// �v���C���[��������܂łɂ����鎞��
    /// </summary>
    private const float FADE_DURATION = 1.0f;

    /// <summary>
    /// ��ʂ��t�F�[�h�A�E�g����̂�҂���
    /// ���ۂɂ͂��̒l��ς��邱�ƂŃt�F�[�h�A�E�g�̎��Ԃ��ς��킯�ł͂Ȃ�
    /// </summary>
    private const float UI_FADEOUT_DURATION = 2.0f;

    /// <summary>
    /// ��ʂ��t�F�[�h�C������̂�҂���
    /// ���ۂɂ͂��̒l��ς��邱�ƂŃt�F�[�h�C���̎��Ԃ��ς��킯�ł͂Ȃ�
    /// </summary>
    private const float UI_FADEIN_DURATION = 0.3f;

    /// <summary>
    /// �o�Ă�����̃Q�[�g
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
    /// �q�v�f�̑傫����ύX����
    /// �Q�[�g�͎q�v�f�Ƃ��đ��݂��邽��
    /// </summary>
    /// <param name="gate"></param>
    private async UniTask ChildDOScale(GameObject gate, float scale , float duration)
    {
        Transform[] gateChildren = gate.GetComponentsInChildren<Transform>();

        List<UniTask> tasks = new List<UniTask>();
        foreach (Transform gateChild in gateChildren)
        {
            // �e�A�j���[�V�������J�n���A�^�X�N�����X�g�ɒǉ�
            // �����ł�await���Ȃ����߁A�����Ɏ��̃A�j���[�V�����ɐi��
            tasks.Add(
                gateChild.DOScale(scale, duration)
                .SetEase(Ease.OutCirc)
                .ToUniTask());
        }

        // �S�ẴA�j���[�V��������������܂őҋ@
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
    /// �v���C���[���Q�[�g�ɐG��ĕ����A�j���[�V�����V�[�P���X
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
    /// �Q�[�g�ɐG��ē���������̃A�j���[�V�����V�[�P���X
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
