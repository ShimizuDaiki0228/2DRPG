using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    private const float DELAY_START = 1.5f;

    private void Start()
    {
        if(SaveManager.instance.HasSaveData() == false)
            continueButton.gameObject.SetActive(false);

        continueButton.OnClickAsObservable()
            .Subscribe(_ =>
                ContinueGame(continueButton)
            ).AddTo(this);

        exitButton.OnClickAsObservable()
            .Subscribe(_ =>
               ExitGame()
            ).AddTo(this);

        newGameButton.OnClickAsObservable()
            .Subscribe(_ =>
                NewGame(newGameButton)
            ).AddTo(this);
    }

    public void ContinueGame(Button button)
    {
        LoadSceneWithFadeEffect(button);
    }

    public void NewGame(Button button)
    {
        SaveManager.instance.DeleteSaveData();
        LoadSceneWithFadeEffect(button);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    async void LoadSceneWithFadeEffect(Button button)
    {
        AudioManager.instance.PlaySFX(40, null);
        Sequence buttonClickSequence = ButtonClickAnimationSequence(button);
        await buttonClickSequence.AsyncWaitForCompletion();

        fadeScreen.FadeOut();

        await UniTask.WaitForSeconds(DELAY_START);

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// ボタンを押した時のアニメーションシーケンス
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    private Sequence ButtonClickAnimationSequence(Button button)
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(button.transform.DOScale(
                    0.8f,
                    0.3f
                )
                .SetEase(Ease.OutCirc)
            )
            .Append(button.transform.DOScale(
                    1,
                    0.3f
                )
                .SetEase(Ease.OutCirc)
            );

        return sequence;
    }
}
