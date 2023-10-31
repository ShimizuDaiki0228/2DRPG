using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public enum KeyType
{
    //エンターキー
    Return,

    //Oキー
    O,

    //Pキー
    P,

    //Kキー
    K,

    //Lキー
    L
}


public class SpecialDescriptionManager : MonoBehaviour, ISaveManager
{
    public static SpecialDescriptionManager instance;

    private bool isMenuUsing; 

    public Subject<bool> isMenuUsingSubject = new Subject<bool>();
    public IObservable<bool> OnMenuUsingAsObservable => isMenuUsingSubject.AsObservable();

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadMenuUsing(_data);
    }

    //初めのチュートリアルが終わっているかどうかを確認
    //終わっていればメニューを使える
    private void LoadMenuUsing(GameData _data)
    {
        isMenuUsing = _data.isMenuUsing;

        if (isMenuUsing)
            isMenuUsingSubject.OnNext(isMenuUsing);
    }

    public void SaveData(ref GameData _data)
    {
        _data.isMenuUsing = isMenuUsing;
    }

    public KeyType GetKeyType(int index, int textNum)
    {
        if(index == 1)
        {
            if (textNum == 1)
            {
                isMenuUsing = true;
                isMenuUsingSubject.OnNext(true);
                return KeyType.P;
            }
            else if (textNum == 2)
                return KeyType.O;
            else if (textNum == 3)
                return KeyType.K;
            else if (textNum == 4)
                return KeyType.L;

        }

        return KeyType.Return;
    }
}
