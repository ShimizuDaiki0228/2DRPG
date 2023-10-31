using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シングルトンパターン
public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        //同じようにPlayerManagerを持っているゲームオブジェクトが存在したときにinstanceを生成しないようにする
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    public bool HaveEnoughMoney(int _price)
    {
        if(_price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency = currency - _price;
        return true;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
