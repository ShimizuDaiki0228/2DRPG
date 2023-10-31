using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DOTweenExtensions
{
    public static UniTask GetAwaiter(this Tween tween)
    {
        var completionSource = new UniTaskCompletionSource();
        tween.OnComplete(() => completionSource.TrySetResult());
        return completionSource.Task;
    }
}
