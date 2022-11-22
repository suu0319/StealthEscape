using UnityEngine;
using UnityEngine.UI;
using GameLoading;
using Manager;

namespace OptionMenu
{
    public class OptionMenuController : MonoBehaviour
    {
        public static OptionMenuController Instance;

        [Header("GameObject")]
        [SerializeField]
        private GameObject _optionMenu;

        [Header("Operate Button")]
        [SerializeField]
        internal Button OpenBtn;
        [SerializeField]
        internal Button ContinueBtn;
        [SerializeField]
        internal Button BackMainMenuBtn;
        [SerializeField]
        internal Button QuitGameBtn;

        private void Awake()
        {
            InitSingleton();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 註冊選項選單Button OnClick事件
        /// </summary>
        internal void AddOnClickListener()
        {
            OpenBtn.onClick.AddListener(OpenOptionMenu);
            ContinueBtn.onClick.AddListener(ContinueGame);
            BackMainMenuBtn.onClick.AddListener(BackMainMenu);
            QuitGameBtn.onClick.AddListener(QuitGame);
        }

        /// <summary>
        /// 開啟選項選單
        /// </summary>
        internal void OpenOptionMenu()
        {
            _optionMenu.SetActive(true);
            GameStateController.Instance.SwitchGameStopState();
        }

        /// <summary>
        /// 繼續遊戲
        /// </summary>
        internal void ContinueGame()
        {
            _optionMenu.SetActive(false);
            GameStateController.Instance.SwitchGameSceneState();
        }

        /// <summary>
        /// 返回主選單
        /// </summary>
        internal void BackMainMenu()
        {
            GameLoadingPanel.Instance.gameObject.SetActive(true);
            GameLoadingAsync.Instance.LoadGame("StartScene");
            GameStateController.Instance.SwitchMainMenuState();
        }

        /// <summary>
        /// 退出遊戲
        /// </summary>
        internal void QuitGame()
        {
            Application.Quit();
        }
    }
}