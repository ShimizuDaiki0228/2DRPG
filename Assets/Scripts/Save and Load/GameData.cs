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

    //�Ō�Ɏ��񂾃v���C���[�̈ʒu
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    //�ŏ��̃A�C�e�����h���b�v���������Ă��Ȃ���
    public bool isFirstTutorial;

    //���j���[���J���邩�ǂ���
    public bool isMenuUsing;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
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
