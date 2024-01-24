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
    /// TutorialInstructionArrowImage�̈ʒu
    /// </summary>
    [SerializeField] private Transform arrowCraftUIPos;
    [SerializeField] private Transform arrowCharacterUIPos;
    [SerializeField] private Transform arrowSkillTreeUIPos;

    private Sequence arrowImageSequence = DOTween.Sequence();

    /// <summary>
    /// �`���[�g���A���ōŏ��ɉ��ւ�����X�L��
    /// ����ȊO�̃X�L������������ƃC�x���g�����܂������Ȃ��Ȃ邽��
    /// ���󑼂̋��z���グ�āA�J���ł��Ȃ��悤�ɂ��邱�Ƃŉ���
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

    //�e�L�X�g���\�����ꂽ����Enter�L�[�ŕ����悤�ɂ���
    private IDisposable subscription;

    //���j���[���g���邩�ǂ���
    //SpecialDescriptionManager�X�N���v�g�Ŏg���邩�ǂ������w�ǂ���
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
    /// ���݃`���[�g���A���̐��������ǂ���
    /// </summary>
    public ReactiveProperty<bool> IsTutorialPlayingProp = new ReactiveProperty<bool>(false);

    /// <summary>
    /// �󂹂�ǁA���ۂɂ͏�Ɉړ�����
    /// �`���[�g���A����ɓ�����
    /// </summary>
    [SerializeField]
    private GameObject canBrokenWall;

    /// <summary>
    /// canBrokenWall���j�󂳂ꂽ���ǂ���
    /// �O��̃Z�[�u�f�[�^�Ŕj�󂳂�Ă���ꍇ�͕\�����Ȃ��悤��
    /// </summary>
    private bool isBrokenWallDestroy;

    /// <summary>
    /// canBrokenWall���ړ�����Ƃ��ɐ�������G�t�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject dustFXPrefab;

    /// <summary>
    /// canBrokenWall���Ō�܂ŏ�ɂ��������ɐ�������G�t�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject dustFinishFXPrefab;

    /// <summary>
    /// �J������h�炷�Ƃ��̒l
    /// </summary>
    private ReactiveProperty<float> amplitudeGainProp = new ReactiveProperty<float>(0);
    private ReactiveProperty<float> frequencyGainProp  = new ReactiveProperty<float>(0);

    /// <summary>
    /// noise�̂��ꂼ��̒l�̍ő�l
    /// </summary>
    private const float MAX_AMPLITUDEGAIN = 1;
    private const float MAX_FREQUENCYGAIN = 5;

    /// <summary>
    /// �J�����̗h�ꂪ�ő�ɂȂ�܂łɂ����鎞��
    /// </summary>
    private const float MAX_FREQUENCY_DURATION = 0.5f;

    /// <summary>
    /// �J�����̗h�ꂪ0�ɂȂ�܂łɂ����鎞��
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
    /// �C�x���g�ݒ�
    /// </summary>
    private void SetEvent()
    {
        //������\�������邽�߂̃g���K�[�ƂȂ�I�u�W�F�N�g�ɐG����UI���\������鏈�����w��
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
    /// �g�[�N��ʂ�\������
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
    /// �v���C���[���\������Ęb���悤�ɂ���
    /// </summary>
    /// <returns></returns>
    private async Task TalkUIDisplay()
    {
        talkScreenBackground.gameObject.SetActive(true);
        talkScreenPlayer.gameObject.SetActive(true);

        //�����̓�����Ԃɂ���
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
    /// �g�[�N��ʂ��B��0
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
    /// canBrokenWall�𓮂���
    /// �`���[�g���A����
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

    //������UI��\��
    //�����I�u�W�F�N�g�ł͂Ȃ����ʂȏꍇ�ɌĂ΂��
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
        pressKeyInstructionText.text = $"{keyInstruction}�L�[���N���b�N���Ă�������";

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
                    ///�`���[�g���A���Ō�̍���
                    ///����ɂ����ingameUI�ɖ߂��悤�ɂȂ�
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

    //UI��DescriptionText��\�����Ȃ���A�����o��
    private async UniTask DisplayText(string description)
    {
        int frameCounter = 0;
        int framesBetweenSounds = 50; // 50�t���[�����Ƃɉ����Đ�����Ƃ���B
        //DOText�̊�����ҋ@����
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
    /// UI�̐؂�ւ����s��
    /// </summary>
    /// <param name="uiTutorial">�`���[�g���A�����AUI�̐��������Ă��邩</param>
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
                //�Q�[���I�u�W�F�N�g���A�N�e�B�u��Ԃ̂Ƃ���fadeScreen�̏�Ԃɂ��邽��
                bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

                if (fadeScreen == false)
                {
                    //���󂾂�descriptionUI�������Ȃ����ߌʂŏ����悤�ɂ����ق�����������
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
    /// �t�F�[�h�A�E�g
    /// </summary>
    public void FadeOut()
    {
        fadeScreen.FadeOut();
    }

    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    public void FadeIn()
    {
        fadeScreen.FadeIn();
    }

    /// <summary>
    /// �O���f�[�V�����Ńt�F�[�h�A�E�g�������
    /// </summary>
    public async UniTask GradationFadeOut(Sprite sprite)
    {
        transitionScreenImage.sprite = sprite;
        await transitionScreen.Animate(2);
    }

    /// <summary>
    /// �O���f�[�V�����Ńt�F�[�h�A�E�g�������
    /// </summary>
    public async UniTask GradationFadeIn(Sprite sprite)
    {
        transitionScreenImage.sprite = sprite;
        await transitionScreen.ReverseAnimate(2);
    }
}
