using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class UI_CraftWindow : MonoBehaviour, ISaveManager
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    /// <summary>
    /// チュートリアルが終わっているかどうか
    /// 終わっていない場合はCreate時にイベントが流れる
    /// </summary>
    private bool isTutorial;

    private Subject<Unit> isCraftUITutorialSubject = new Subject<Unit>();
    public IObservable<Unit> IsCraftUITutorialAsObservable
        => isCraftUITutorialSubject.AsObservable();

    public void LoadData(GameData _data)
    {
        isTutorial = _data.isFirstTutorial;
    }

    public void SaveData(ref GameData _data)
    { 
    }

    public void SetupCraftWindow(ItemData_Equipment _data)
    {

        craftButton.onClick.RemoveAllListeners();

        for(int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for(int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialImage.Length)
                Debug.Log("You have more materials");

            materialImage[i].sprite = _data.craftingMaterials[i].data.itemIcon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;

        }


        itemIcon.sprite = _data.itemIcon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                bool canCraft = Inventory.instance.CanCraft(_data, _data.craftingMaterials);

                if (canCraft)
                {
                    AudioManager.instance.PlaySFX(44, null);
                    if (!isTutorial)
                        isCraftUITutorialSubject.OnNext(Unit.Default);
                }
                else
                    AudioManager.instance.PlaySFX(43, null);
            }
            ).AddTo(this);

        //craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }
}
