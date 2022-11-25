using UnityEngine;
using UnityEngine.UI;
using GameLoading;
using Manager;

public class MainMenuController : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _gameStage;

    [Header("Operate Button")]
    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _stageBtn;
    [SerializeField]
    private Button _quitBtn;
    [SerializeField]
    private Button _backBtn;

    private void Awake()
    {
        InitMainMenuBtn();
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void StartGame()
    {
        GameLoadingPanel.Instance.gameObject.SetActive(true);
        GameLoadingAsync.Instance.LoadGame("Desert");
        GameStateController.Instance.SwitchGameSceneState();
    }

    /// <summary>
    /// 返回主選單UI
    /// </summary>
    private void BackMainMenu()
    {
        _gameStage.SetActive(false);
        _mainMenu.SetActive(true);
        GameStateController.Instance.SwitchMainMenuState();
    }

    /// <summary>
    /// 開啟選擇關卡UI
    /// </summary>
    private void ChooseStage()
    {
        _mainMenu.SetActive(false);
        _gameStage.SetActive(true);
        GameStateController.Instance.SwitchGameStageState();
    }

    /// <summary>
    /// 退出遊戲
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 主選單按鈕註冊OnClick事件
    /// </summary>
    private void InitMainMenuBtn()
    {
        _startBtn.onClick.AddListener(StartGame);
        _stageBtn.onClick.AddListener(ChooseStage);
        _quitBtn.onClick.AddListener(QuitGame);
        _backBtn.onClick.AddListener(BackMainMenu);
    }
}