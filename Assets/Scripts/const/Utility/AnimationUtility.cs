using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility
{
    /// <summary>
    /// UI�̃`���[�g���A�����ɕ\���������̃A�j���[�V�����V�[�P���X
    /// </summary>
    /// <returns></returns>
    public static Sequence InstructionArrowSequence(GameObject arrow,
                                                    Transform firstSetTransform)
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(arrow.transform.DOMoveX(
                firstSetTransform.position.x + 20,
                1)
                .SetLoops(-1, LoopType.Restart)
            );

        return sequence;
    }
}
