using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUnscaledTime : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
}
