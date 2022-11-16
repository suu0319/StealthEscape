using UnityEngine;
using DG.Tweening;
using GameLoading;
using Manager;

public class SuccessfulEscape : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField]
    private GameObject _successfulEscapePanel;

    [Header("Animation")]
    [SerializeField]
    private Animator _boatAnimator;
    [SerializeField]
    private Animator _playerAnimator;

    [Header("CanvasGroup")]
    [SerializeField]
    private CanvasGroup _canvasGroupBackground;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        GameLoadingPanel.Instance.ScreenFadeOut();

        var time = 0;

        DOTween.To(() => time, x => time = x, 1, 2f).onComplete += PlayEscapeAnimation;
    }

    /// <summary>
    /// 播放逃脫動畫
    /// </summary>
    private void PlayEscapeAnimation()
    {
        var time = 0;

        _boatAnimator.enabled = true;
        _playerAnimator.enabled = true;

        DOTween.To(() => time, x => time = x, 1, 1.5f).onComplete += AppearEscapeScreen;
    }

    /// <summary>
    /// 出現逃脫成功畫面
    /// </summary>
    private void AppearEscapeScreen()
    {
        var time = 0;  

        _successfulEscapePanel.SetActive(true);
        _canvasGroupBackground.DOFade(1, 5f).SetEase(Ease.Linear).onComplete += (() => DOTween.To(() => time, x => time = x, 1, 5f).onComplete += BackMainMenu);
    }

    /// <summary>
    /// 返回主選單
    /// </summary>
    private void BackMainMenu()
    {
        GameLoadingPanel.Instance.gameObject.SetActive(true);
        GameLoadingAsync.Instance.LoadGame("StartScene");
        GameStateController.Instance.SwitchMainMenuState();
    }
}