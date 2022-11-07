using UnityEngine;
using UnityEngine.UI;
using GameLoading;
using Manager;

namespace OptionMenu 
{
    public class OptionMenuController : MonoBehaviour
    {
        private static OptionMenuController _instance;
        public static OptionMenuController Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void Init()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        [SerializeField]
        private GameObject _optionMenu;

        [SerializeField]
        internal Button OpenBtn, ContinueBtn, BackMainMenuBtn, QuitGameBtn;

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
            Time.timeScale = 0f;
            _optionMenu.SetActive(true);
            GameStateController.Instance.SwitchGameStopState();
        }

        /// <summary>
        /// 繼續遊戲
        /// </summary>
        internal void ContinueGame()
        {
            Time.timeScale = 1f;
            _optionMenu.SetActive(false);
            GameStateController.Instance.SwitchGameSceneState();
        }

        /// <summary>
        /// 返回主選單
        /// </summary>
        internal void BackMainMenu()
        {
            Time.timeScale = 1f;
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