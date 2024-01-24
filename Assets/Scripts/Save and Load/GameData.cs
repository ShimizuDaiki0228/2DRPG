using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public SerializableDictionary<string, bool> descriptionPoints;
    public string descriptionPointId;

    //最後に死んだプレイヤーの位置
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    //最初のアイテムをドロップしたかしていないか
    public bool isFirstTutorial;

    //メニューを開けるかどうか
    public bool isMenuUsing;

    //各種UIを開けるかどうか
    public bool IsOKeyUsing;
    public bool IsPKeyUsing;
    public bool IsKKeyUsing;
    public bool IsLKeyUsing;

    /// <summary>
    /// canBrokenWallが破壊されたかどうか
    /// </summary>
    public bool IsBrokenWallDestroyed;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        IsBrokenWallDestroyed = false;

        IsOKeyUsing = false;
        IsPKeyUsing = false;
        IsKKeyUsing = false;
        IsLKeyUsing = false;

        this.isMenuUsing = false;
        this.isFirstTutorial = false;

        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;

        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();

        descriptionPointId = string.Empty;
        descriptionPoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
