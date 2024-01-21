using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField]
    private Material _transitionIn;

    /// <summary>
    /// time秒かけてトランジションを行う
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public async UniTask Animate(float time)
    {
        GetComponent<Image>().material = _transitionIn;
        float current = 0;
        while (current < time)
        {
            _transitionIn.SetFloat("_Alpha", current / time);
            await UniTask.Yield(PlayerLoopTiming.Update);
            current += Time.deltaTime;
        }
        _transitionIn.SetFloat("_Alpha", 1);
    }

    public async UniTask ReverseAnimate(float time)
    {
        GetComponent<Image>().material = _transitionIn;
        float current = 0;
        while (current < time)
        {
            _transitionIn.SetFloat("_Alpha", 1 - current / time);
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            current += Time.deltaTime;
        }
        _transitionIn.SetFloat("_Alpha", 0);
    }

    private void OnDestroy()
    {
        _transitionIn.SetFloat("_Alpha", 0);
    }
}
