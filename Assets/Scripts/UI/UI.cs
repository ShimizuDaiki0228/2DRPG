using Cysharp.Threading.Tasks;
using RPG.Definition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using DG.Tweening;
using System.Reflection;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    [Header("Transition screen")]
    [SerializeField] private Image transitionScreenImage;
    [SerializeField] private TransitionScreen transitionScreen;

    [Header("Talk screen")]
    [SerializeField] private GameObject talkScreenBackgroundGameObject;
    [SerializeField] private CanvasGroup talkScreenBackground;
    [SerializeField] private GameObject talkScreenPlayerGameObject;
    [SerializeField] private RectTransform talkScreenPlayer;

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
    [SerializeField] private GameObject operationUI;
    [SerializeField] private GameObject descriptionUI;
    [SerializeField] private GameObject tutorialInstructionArrowImage;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    /// <summary>
    /// TutorialInstructionArrowImageの位置
    /// </summary>
    [SerializeField] private Transform arrowCraftUIPos;
    [SerializeField] private Transform arrowCharacterUIPos;
    [SerializeField] private Transform arrowSkillTreeUIPos;

    private Sequence arrowImageSequence = DOTween.Sequence();

    /// <summary>
    /// チュートリアルで最初に解禁させるスキル
    /// これ以外のスキルを解放されるとイベントがうまくいかなくなるため
    /// 現状他の金額を上げて、開放できないようにすることで解決
    /// </summary>
    [SerializeField]
    private UI_SkillTreeSlot tutorialSkillSlot;

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

    private const int DESCRIPTION_HIDDEN_WAIT_TIME = 100;

    //テキストが表示された時にEnterキーで閉じれるようにする
    private IDisposable subscription;

    //メニューが使えるかどうか
    //SpecialDescriptionManagerスクリプトで使えるかどうかを購読する
    private bool isMenuUsing;

    Dictionary<int, string> specialDescriptionDict = new Dictionary<int, string>();
    Dictionary<int, string> specialDescriptionKeyInstructionDict = new Dictionary<int, string>();
    Dictionary<int, KeyCode> specialDescriptionKeyCodeDict = new Dictionary<int, KeyCode>();

    KeyCode descriptionKeyCode = default;

    private bool isOKeyUsing = false;
    private bool isPKeyUsing = false;
    private bool isKKeyUsing = false;
    private bool isLKeyUsing = false;

    /// <summary>
    /// 現在チュートリアルの説明中かどうか
    /// </summary>
    public ReactiveProperty<bool> IsTutorialPlayingProp = new ReactiveProperty<bool>(false);

    /// <summary>
    /// 壊せる壁、実際には上に移動する
    /// チュートリアル後に動かす
    /// </summary>
    [SerializeField]
    private GameObject canBrokenWall;

    /// <summary>
    /// canBrokenWallが破壊されたかどうか
    /// 前回のセーブデータで破壊されている場合は表示しないように
    /// </summary>
    private bool isBrokenWallDestroy;

    /// <summary>
    /// canBrokenWallが移動するときに生成するエフェクト
    /// </summary>
    [SerializeField]
    private GameObject dustFXPrefab;

    /// <summary>
    /// canBrokenWallが最後まで上にいった時に生成するエフェクト
    /// </summary>
    [SerializeField]
    private GameObject dustFinishFXPrefab;

    /// <summary>
    /// カメラを揺らすときの値
    /// </summary>
    private ReactiveProperty<float> amplitudeGainProp = new ReactiveProperty<float>(0);
    private ReactiveProperty<float> frequencyGainProp  = new ReactiveProperty<float>(0);

    /// <summary>
    /// noiseのそれぞれの値の最大値
    /// </summary>
    private const float MAX_AMPLITUDEGAIN = 1;
    private const float MAX_FREQUENCYGAIN = 5;

    /// <summary>
    /// カメラの揺れが最大になるまでにかかる時間
    /// </summary>
    private const float MAX_FREQUENCY_DURATION = 0.5f;

    /// <summary>
    /// カメラの揺れが0になるまでにかかる時間
    /// </summary>
    private const float ZERO_FREQUENCY_DURATION = 2.5f;

    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);

        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    void Start()
    {
        pressKeyInstructionText.gameObject.SetActive(false);

        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);

        SetEvent();
    }

    /// <summary>
    /// イベント設定
    /// </summary>
    private void SetEvent()
    {
        //説明を表示させるためのトリガーとなるオブジェクトに触れるとUIが表示される処理を購読
        DescriptionManager.instance.OnDescriptionDisplayObjectAsObservable
            .Subscribe(index =>
            {
                DisplayTalkScreen(index).Forget();
            }).AddTo(this);

        if (!GameManager.instance.IsFirstTutorial)
        {
            GameManager.instance.OnFisrtTutorialAsObservable
                .Take(1)
                .Subscribe(index =>
                {
                    SwitchToSpecialDescriptionDisplay(index).Forget();

                }
                ).AddTo(this);

            craftWindow.IsCraftUITutorialAsObservable
                .Take(1)
                .Subscribe(_ =>
                    SwitchToSpecialDescriptionDisplay(2).Forget()
                ).AddTo(this);

            Inventory.instance.IsCharacterUITutorialAsObservable
                .Take(1)
                .Subscribe(_ =>
                    SwitchToSpecialDescriptionDisplay(3).Forget()
                ).AddTo(this);

            tutorialSkillSlot.IsSkillUITutorialAsObservable
                .Take(1)
                .Subscribe(_ =>
                    SwitchToSpecialDescriptionDisplay(4).Forget()
                ).AddTo(this);
        }

        SpecialDescriptionManager.instance.OnMenuUsingAsObservable
            .Take(1)
            .Subscribe(_ =>
                isMenuUsing = true
            ).AddTo(this);

        amplitudeGainProp
            .Subscribe(amplitudeGain => noise.m_AmplitudeGain = amplitudeGain).AddTo(this);

        frequencyGainProp
            .Subscribe(frequencyGain => noise.m_FrequencyGain = frequencyGain).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenuUsing)
        {
            if (Input.GetKeyDown(KeyCode.O) && isOKeyUsing)
                SwitchWithKeyTo(characterUI);

            if (Input.GetKeyDown(KeyCode.P) && isPKeyUsing)
                SwitchWithKeyTo(craftUI);

            if (Input.GetKeyDown(KeyCode.K) && isKKeyUsing)
                SwitchWithKeyTo(skillTreeUI);

            if (Input.GetKeyDown(KeyCode.L) && isLKeyUsing)
                SwitchWithKeyTo(optionsUI);
        }

        if (Input.GetKeyDown(KeyCode.M))
            SwitchWithKeyTo(operationUI);

    }

    /// <summary>
    /// トーク画面を表示する
    /// </summary>
    private async UniTask DisplayTalkScreen(int index)
    {
        isMenuUsing = false;
        AudioManager.instance.StopSFX(8);
        await TalkUIDisplay();

        SwitchWithKeyTo(descriptionUI);
        DescriptionManager.instance.descriptionUIAnimator.SetTrigger("Display");
        string description = DescriptionDefinition.GetDescriptionText(index);
        descriptionText.text = "";

        await DisplayText(description);
        PressKeyInstruction();

        GameManager.instance.PauseGame(true);
    }

    /// <summary>
    /// プレイヤーが表示されて話すようにする
    /// </summary>
    /// <returns></returns>
    private async Task TalkUIDisplay()
    {
        talkScreenBackground.gameObject.SetActive(true);
        talkScreenPlayer.gameObject.SetActive(true);

        //初期の透明状態にする
        talkScreenBackground.DOFade(0, 0);

        Sequence fadeSequence = DOTween.Sequence();

        fadeSequence
            .Append(talkScreenBackground.DOFade(
                        0.5f,
                        0.2f
                    )
                    .SetUpdate(true)
            )
            .Append(talkScreenPlayer.DOLocalMoveX(
                        talkScreenPlayer.rect.position.x + 250,
                        0.5f
                    )
                    .SetUpdate(true)
            );

        await fadeSequence.AsyncWaitForCompletion();
    }

    private void PressKeyInstruction()
    {
        pressKeyInstructionText.gameObject.SetActive(true);
        subscription = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return))
            .Take(1)
            .Subscribe(_ =>
            {
                HiddenTalkScreen().Forget();
                pressKeyInstructionText.gameObject.SetActive(false);
                SwitchToDescriptionHidden().Forget();
                subscription.Dispose();
            }).AddTo(this);
    }

    /// <summary>
    /// トーク画面を隠す0
    /// </summary>
    private async UniTask HiddenTalkScreen(bool isTutorial = false)
    {
        GameManager.instance.PauseGame(false);

        talkScreenBackground.DOFade(0f, 0.2f);
        await talkScreenPlayer.DOLocalMoveX(talkScreenPlayer.rect.position.x - 250,
                                      0.5f).ToUniTask();

        talkScreenBackground.gameObject.SetActive(false);
        talkScreenPlayer.gameObject.SetActive(false);

        if(isTutorial)
        {
            canBrokenWallMove().Forget();
        }
    }

    /// <summary>
    /// canBrokenWallを動かす
    /// チュートリアル後
    /// </summary>
    /// <returns></returns>
    private async UniTask canBrokenWallMove()
    {
        isBrokenWallDestroy = true;

        DOTween.To(() => amplitudeGainProp.Value, x 
            => amplitudeGainProp.Value = x,
            MAX_AMPLITUDEGAIN, MAX_FREQUENCY_DURATION);
        DOTween.To(() => frequencyGainProp.Value, x 
            => frequencyGainProp.Value = x,
            MAX_FREQUENCYGAIN, MAX_FREQUENCY_DURATION);

        GameObject dustFX = Instantiate(dustFXPrefab,
                                                    dustFXPrefab.transform.position,
                                                    Quaternion.identity);

        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(canBrokenWall.transform.DOMoveY(7, 5)
                    .SetEase(Ease.OutCirc)
            )
            .AppendInterval(2)
            .Join(DOTween.To(() => amplitudeGainProp.Value, x
                  => amplitudeGainProp.Value = x,
                  0, ZERO_FREQUENCY_DURATION)
            )
            .Join(DOTween.To(() => frequencyGainProp.Value, x
                  => frequencyGainProp.Value = x,
                  0, ZERO_FREQUENCY_DURATION)
            );

        await sequence.AsyncWaitForCompletion();

        Destroy(canBrokenWall);
        Destroy(dustFX);

        GameObject dustFinishFX = Instantiate(dustFinishFXPrefab,
                                              dustFinishFXPrefab.transform.position,
                                              Quaternion.identity);

        ParticleSystem dustFinishFXParticleSystem
        = dustFinishFX.GetComponent<ParticleSystem>();

        if (dustFinishFXParticleSystem != null)
            Destroy(dustFinishFX, dustFinishFXParticleSystem.main.duration);
    }

    //説明のUIを表示
    //何かオブジェクトではなく特別な場合に呼ばれる
    private async UniTask SwitchToSpecialDescriptionDisplay(int index)
    {
        if(index == 1)
            IsTutorialPlayingProp.Value = true;

        arrowImageSequence.Kill();

        await TalkUIDisplay();
        SwitchWithKeyTo(descriptionUI);
        DescriptionManager.instance.descriptionUIAnimator.SetTrigger("Display");

        specialDescriptionDict = DescriptionDefinition.GetSpecialDescriptionText(index);
        specialDescriptionKeyInstructionDict = DescriptionDefinition.GetSpecialDescriptionKeyInstruction(index);
        specialDescriptionKeyCodeDict = DescriptionDefinition.GetSpecialDescriptionKeyCode(index);

        SpecialPressKeyInstruction(index,
                                   0,
                                   specialDescriptionDict,
                                   specialDescriptionKeyInstructionDict,
                                   specialDescriptionKeyCodeDict).Forget();
    }

    private async UniTask SpecialPressKeyInstruction(int index,
                                                     int textNum,
                                                     Dictionary<int, string> textDict,
                                                     Dictionary<int, string> keyInstructionDict,
                                                     Dictionary<int, KeyCode> keyCodeDict)
    {
        int _textNum = textNum;
        string text = textDict[textNum];
        string keyInstruction = keyInstructionDict[textNum];
        KeyCode keyCode = keyCodeDict[textNum];

        descriptionText.text = "";
        await DisplayText(text);
        KeyType keyType = SpecialDescriptionManager.instance.GetKeyType(index, textNum);

        descriptionKeyCode = keyCode;
        pressKeyInstructionText.text = $"{keyInstruction}キーをクリックしてください";

        if (keyInstruction == "O") isOKeyUsing = true;
        else if(keyInstruction == "P") isPKeyUsing = true;
        else if(keyInstruction == "K") isKKeyUsing = true;
        else if(keyInstruction == "L") isLKeyUsing = true;

        pressKeyInstructionText.gameObject.SetActive(true);
        subscription = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(descriptionKeyCode))
            .Subscribe(_ =>
            {
                if (_textNum < textDict.Count - 1)
                {
                    _textNum++;
                    SpecialPressKeyInstruction(index, 
                                               _textNum,
                                               textDict,
                                               keyInstructionDict,
                                               keyCodeDict).Forget();
                }
                else
                {
                    ///チュートリアル最後の項目
                    ///これによってingameUIに戻れるようになる
                    if(index == 4)
                        IsTutorialPlayingProp.Value = false;

                    if (index == 5)
                        HiddenTalkScreen(true).Forget();
                    else
                        HiddenTalkScreen().Forget();

                    SwitchToDescriptionHidden().Forget();
                }
                pressKeyInstructionText.gameObject.SetActive(false);
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
        DescriptionManager.instance.descriptionUIAnimator.SetTrigger("Hidden");
        await UniTask.DelayFrame(DESCRIPTION_HIDDEN_WAIT_TIME);
        SwitchWithKeyTo(descriptionUI);
    }

    /// <summary>
    /// UIの切り替えを行う
    /// </summary>
    /// <param name="uiTutorial">チュートリアルかつ、UIの説明をしているか</param>
    public void SwitchTo(GameObject _menu, bool uiTutorial = false)
    {
        AudioManager.instance.StopSFX(8);

        if(_menu == descriptionUI)
        {
            inGameUI.SetActive(false);
            tutorialInstructionArrowImage.SetActive(false);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                //ゲームオブジェクトがアクティブ状態のときにfadeScreenの状態にするため
                bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

                if (fadeScreen == false)
                {
                    //現状だとdescriptionUIが消せないため個別で消すようにしたほうがいいかも
                    if (transform.GetChild(i).gameObject != descriptionUI)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }

        }
        
        transitionScreen.gameObject.SetActive(true);

        talkScreenBackground.gameObject.SetActive(true);
        talkScreenPlayer.gameObject.SetActive(true);

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(5, null);
            _menu.SetActive(true);
        }

        if(uiTutorial)
            tutorialInstructionArrowImage.gameObject.SetActive(true);

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
        if (_menu != null && _menu.activeSelf)
        {
            if (!GameManager.instance.IsFirstTutorial
                && !IsTutorialPlayingProp.Value
                && _menu != operationUI
                && _menu != descriptionUI)
            {
                SwitchToSpecialDescriptionDisplay(5).Forget();
                GameManager.instance.SetIsFirstTutorial();
            }
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        if(IsTutorialPlayingProp.Value)
        {
            if (_menu == craftUI)
                tutorialInstructionArrowImage.transform.position = arrowCraftUIPos.position;
            else if (_menu == skillTreeUI)
                tutorialInstructionArrowImage.transform.position = arrowSkillTreeUIPos.position;
            else if (_menu == characterUI)
                tutorialInstructionArrowImage.transform.position = arrowCharacterUIPos.position;
            else
            {
                SwitchTo(_menu);
                return;
            }

            arrowImageSequence = AnimationUtility.InstructionArrowSequence(tutorialInstructionArrowImage,
                                                      tutorialInstructionArrowImage.transform);

            SwitchTo(_menu, true);
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject UIChild = transform.GetChild(i).gameObject;

            if (UIChild.activeSelf && 
                (UIChild.GetComponent<UI_FadeScreen>() == null 
                 && UIChild.GetComponent<TransitionScreen>() == null)
                 && UIChild != talkScreenBackgroundGameObject
                 && UIChild != talkScreenPlayerGameObject)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        FadeOut();
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

        isOKeyUsing = _data.IsOKeyUsing;
        isPKeyUsing = _data.IsPKeyUsing;
        isKKeyUsing = _data.IsKKeyUsing;
        isLKeyUsing = _data.IsLKeyUsing;

        isBrokenWallDestroy = _data.IsBrokenWallDestroyed;
        if (isBrokenWallDestroy)
            Destroy(canBrokenWall);
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }

        _data.IsOKeyUsing = isOKeyUsing;
        _data.IsPKeyUsing = isPKeyUsing;
        _data.IsKKeyUsing = isKKeyUsing;
        _data.IsLKeyUsing = isLKeyUsing;

        _data.IsBrokenWallDestroyed = isBrokenWallDestroy;
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void FadeOut()
    {
        fadeScreen.FadeOut();
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void FadeIn()
    {
        fadeScreen.FadeIn();
    }

    /// <summary>
    /// グラデーションでフェードアウトさせる際
    /// </summary>
    public async UniTask GradationFadeOut(Sprite sprite)
    {
        transitionScreenImage.sprite = sprite;
        await transitionScreen.Animate(2);
    }

    /// <summary>
    /// グラデーションでフェードアウトさせる際
    /// </summary>
    public async UniTask GradationFadeIn(Sprite sprite)
    {
        transitionScreenImage.sprite = sprite;
        await transitionScreen.ReverseAnimate(2);
    }
}
