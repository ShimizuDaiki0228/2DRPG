using Cysharp.Threading.Tasks;
using RPG.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Unity.VisualScripting;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject descriptionUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI itemCreateText;
    [SerializeField] private TextMeshProUGUI addSkillText;
    [SerializeField] private TextMeshProUGUI dashAttackText;
    [SerializeField] private TextMeshProUGUI blackHoleAttackText;
    [SerializeField] private TextMeshProUGUI CrystalAttackText;
    [SerializeField] private TextMeshProUGUI PortionText;
    [SerializeField] private TextMeshProUGUI SwordSkillText;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI pressKeyInstructionText;


    private bool onDescriptionDisplay = false;
    private const int DESCRIPTION_HIDDEN_WAIT_TIME = 100;

    //テキストが表示された時にEnterキーで閉じれるようにする
    private IDisposable subscription;

    //メニューが使えるかどうか
    //SpecialDescriptionManagerスクリプトで使えるかどうかを購読する
    private bool isMenuUsing;

    List<string> specialDescriptionList = new List<string>();

    KeyCode descriptionKeyCode = default;

    private bool isTutorialDescription = false;

    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);
    }


    void Start()
    {
        pressKeyInstructionText.gameObject.SetActive(false);

        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);

        //説明を表示させるためのトリガーとなるオブジェクトに触れるとUIが表示される処理を購読
        DescriptionManager.instance.OnDescriptionDisplayObjectAsObservable
            .Subscribe(index =>
            {
                SwitchToDescriptionDisplay(index).Forget();
                onDescriptionDisplay = true;

            }).AddTo(this);

        if(!GameManager.instance.IsFirstTutorial)
        {
            GameManager.instance.OnFisrtTutorialAsObservable
                .Take(1)
                .Subscribe(index =>
                {
                    SwitchToSpecialDescriptionDisplay(index).Forget();
                    
                }
                ).AddTo(this);
        }

        SpecialDescriptionManager.instance.OnMenuUsingAsObservable
            .Take(1)
            .Subscribe(_ =>
                isMenuUsing = true
            ).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenuUsing)
        {
            if (Input.GetKeyDown(KeyCode.O))
                SwitchWithKeyTo(characterUI);

            if (Input.GetKeyDown(KeyCode.P))
                SwitchWithKeyTo(craftUI);

            if (Input.GetKeyDown(KeyCode.K))
                SwitchWithKeyTo(skillTreeUI);

            if (Input.GetKeyDown(KeyCode.L))
                SwitchWithKeyTo(optionsUI);
        }
    }

    //説明のUIを表示
    //何かオブジェクトではなく特別な場合に呼ばれる
    private async UniTask SwitchToSpecialDescriptionDisplay(int index)
    {
        isTutorialDescription = true;
        SwitchWithKeyTo(descriptionUI);
        DescriptionManager.instance.descriptionUIAnimator.SetTrigger("Display");

        specialDescriptionList = DescriptionDefinition.GetSpecialDescriptionText(index);
        
        SpecialPressKeyInstruction(index , 0, specialDescriptionList).Forget();
    }

    private async UniTask SpecialPressKeyInstruction(int index ,int textNum , List<string> specialDescriptionList)
    {
        int _textNum = textNum;
        string description = specialDescriptionList[_textNum];
        descriptionText.text = "";
        await DisplayText(description);
        KeyType keyType = SpecialDescriptionManager.instance.GetKeyType(index, textNum);

        
        if (keyType == KeyType.P)
        {
            descriptionKeyCode = KeyCode.P;
            pressKeyInstructionText.text = "Pキーをクリックしてください";
        }
        else if (keyType == KeyType.O)
        {
            descriptionKeyCode = KeyCode.O;
            pressKeyInstructionText.text = "Oキーをクリックしてください";
        }
        else if (keyType == KeyType.K)
        {
            descriptionKeyCode = KeyCode.K;
            pressKeyInstructionText.text = "Kキーをクリックしてください";
        }
        else if (keyType == KeyType.L)
        {
            descriptionKeyCode = KeyCode.L;
            pressKeyInstructionText.text = "Lキーをクリックしてください";
        }
        else if (keyType == KeyType.Return)
        {
            descriptionKeyCode = KeyCode.Return;
            pressKeyInstructionText.text = "Enterキーをクリックしてください";
        }

        pressKeyInstructionText.gameObject.SetActive(true);
        subscription = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(descriptionKeyCode))
            .Subscribe(_ =>
            {
                if (_textNum < specialDescriptionList.Count - 1)
                {
                    _textNum++;
                    SpecialPressKeyInstruction(index, _textNum, specialDescriptionList).Forget();
                }
                else
                {
                    if (!GameManager.instance.IsFirstTutorial)
                        GameManager.instance.SetIsFirstTutorial();

                    SwitchToDescriptionHidden().Forget();
                }
                pressKeyInstructionText.gameObject.SetActive(false);
                subscription.Dispose();
            }).AddTo(this);
    }

    //説明のUIを表示
    //テキストを表示させるオブジェクトに当たったときに呼ばれる
    private async UniTask SwitchToDescriptionDisplay(int index)
    {
        SwitchWithKeyTo(descriptionUI);
        DescriptionManager.instance.descriptionUIAnimator.SetTrigger("Display");
        string description = DescriptionDefinition.GetDescriptionText(index);
        descriptionText.text = "";

        await DisplayText(description);
        PressKeyInstruction();
    }

    private void PressKeyInstruction()
    {
        pressKeyInstructionText.gameObject.SetActive(true);
        subscription = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return))
            .Subscribe(_ =>
            {
                pressKeyInstructionText.gameObject.SetActive(false);
                SwitchToDescriptionHidden().Forget();
                subscription.Dispose();
            }).AddTo(this);
    }

    //UIのDescriptionTextを表示しながら、音を出す
    private async UniTask DisplayText(string description)
    {
        int frameCounter = 0;
        int framesBetweenSounds = 50; // 50フレームごとに音を再生するとする。
        //DOTextの完了を待機する
        await descriptionText.DOText(description, description.Length * 0.1f).SetUpdate(true)
            .SetUpdate(true)
            .OnUpdate(() =>
            {
                frameCounter++;
                if (frameCounter % framesBetweenSounds == 0 && descriptionText.text.Length < description.Length)
                {
                    AudioManager.instance.PlaySFX(36, null);
                }
            })
            .GetAwaiter();
    }



    private async UniTask SwitchToDescriptionHidden()
    {
        onDescriptionDisplay = false;
        DescriptionManager.instance.descriptionUIAnimator.SetTrigger("Hidden");
        await UniTask.DelayFrame(DESCRIPTION_HIDDEN_WAIT_TIME);
        SwitchWithKeyTo(descriptionUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        

        for(int i = 0; i < transform.childCount; i++)
        {
            //ゲームオブジェクトがアクティブ状態のときにfadeScreenの状態にするため
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if (fadeScreen == false)
            {
                //現状だとdescriptionUIが消せないため個別で消すようにしたほうがいいかも
                if(transform.GetChild(i).gameObject != descriptionUI)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        if(_menu != null)
        {
            AudioManager.instance.PlaySFX(5, null);
            _menu.SetActive(true);
        }

        if(GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());  
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);

        endText.SetActive(true);

        yield return new WaitForSeconds(2f);

        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach(UI_VolumeSlider item in volumeSettings)
            {
                if(item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
