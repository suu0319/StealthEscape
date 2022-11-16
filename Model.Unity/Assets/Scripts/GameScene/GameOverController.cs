using UnityEngine;
using UnityEngine.UI;
using GameLoading;
using Mediator;
using Manager;

namespace GameOver
{
    public class GameOverController : MonoBehaviour
    {
        public static GameOverController Instance;

        [Header("GameObject")]
        [SerializeField]
        internal GameObject GameOverPanel;

        [Header("Script")]
        [SerializeField]
        private GameSceneMediator _mediator;

        [Header("Operate Button")]
        [SerializeField]
        internal Button ReplayBtn;
        [SerializeField]
        internal Button BackMainMenuBtn;
        [SerializeField]
        internal Button QuitGameBtn;

        [Header("CanvasGroup")]
        [SerializeField]
        private CanvasGroup _canvasGroupBackground;
        [SerializeField]
        private CanvasGroup _canvasGroupMenu;

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
                Destroy(this.gameObject);
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
            ReplayBtn.onClick.AddListener(ReplayGame);
            BackMainMenuBtn.onClick.AddListener(BackMainMenu);
            QuitGameBtn.onClick.AddListener(QuitGame);
        }

        /// <summary>
        /// 重玩遊戲
        /// </summary>
        internal void ReplayGame()
        {
            GameLoadingPanel.Instance.gameObject.SetActive(true);
            GameLoadingAsync.Instance.LoadGame(_mediator.SceneName);
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