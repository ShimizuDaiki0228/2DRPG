using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
    public static DescriptionManager instance;

    [SerializeField]
    private RectTransform descriptionUI;

    public Animator descriptionUIAnimator { get; private set; }

    public Subject<int> descriptionDisplayObjectSubject = new Subject<int>();
    public IObservable<int> OnDescriptionDisplayObjectAsObservable => descriptionDisplayObjectSubject.AsObservable();

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        descriptionUIAnimator = descriptionUI.GetComponent<Animator>();
    }
}