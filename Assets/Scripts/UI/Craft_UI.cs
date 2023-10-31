using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Craft_UI : MonoBehaviour
{
    public static Craft_UI instance;

    public bool isTutorial;

    public Subject<bool> craftUIActiveSubject = new Subject<bool>();
    public IObservable<bool> craftUIActiveAsObservable => craftUIActiveSubject.AsObservable();

    private void Awake()
    {
        if(instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    void OnEnable()
    {
        craftUIActiveSubject.OnNext(true);
    }

    void OnDisable()
    {
        craftUIActiveSubject.OnNext(false);
    }
}
