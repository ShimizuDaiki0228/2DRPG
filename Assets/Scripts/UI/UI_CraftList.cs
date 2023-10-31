using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;

    private GameObject tutorialSelectItem;

    //チュートリアル時に武器の木の棒に対してアクションを行うため
    [SerializeField] private bool isWeapon;

    void Awake()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();

        if (isWeapon)
            Craft_UI.instance.craftUIActiveAsObservable
                .Subscribe(isActive =>
                {
                    Debug.Log(isActive);
                    Debug.Log(GameManager.instance.IsFirstTutorial);
                    if (isActive && !GameManager.instance.IsFirstTutorial)
                    {
                        Debug.Log(tutorialSelectItem);
                        TutorialSelectItem();
                    }
                }
                ).AddTo(this);

    }

    public void SetupCraftList()
    {
        for(int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for(int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
            if (newSlot.GetComponent<UI_CraftSlot>().item.data.ToString() == "Wooden Sword (ItemData_Equipment)")
            {
                tutorialSelectItem = newSlot;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }

    private void TutorialSelectItem()
    {
        foreach(Transform child in tutorialSelectItem.transform)
        {
            Debug.Log(child.gameObject.name);
            if (child.GetComponent<Image>() != null)            
            {
                Image image = child.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

                DOTween.Sequence()
                    .AppendInterval(1f)
                    .Append(image.DOFade(0f, 1f))
                    .Append(image.DOFade(1f, 1f))
                    .SetLoops(-1);

                //TimeScaleのせいで止まっている可能性がある

                Debug.Log("OK");
            }
        }
    }
}
