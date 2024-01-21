using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField]
    private Material _transitionIn;

    void Start()
    {
        StartCoroutine(BeginTransition());
    }

    IEnumerator BeginTransition()
    {
        yield return Animate(_transitionIn, 2);
    }
    
    IEnumerator BeginReverseTransition()
    {
        yield return ReverseAnimate(_transitionIn, 2);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            StartCoroutine(BeginTransition());
        
        if (Input.GetKeyUp(KeyCode.B))
            StartCoroutine(BeginReverseTransition());
    }

    /// <summary>
    /// time秒かけてトランジションを行う
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Animate(Material material, float time)
    {
        GetComponent<Image>().material = material;
        float current = 0;
        while (current < time)
        {
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 1);
    }

    IEnumerator ReverseAnimate(Material material, float time)
    {
        GetComponent<Image>().material = material;
        float current = 0;
        while (current < time)
        {
            material.SetFloat("_Alpha", 1 - current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 0);
    }
}
