using UnityEngine;
using UnityEngine.UI;
using GameLoading;
using Manager;

namespace GameOver 
{
    public class GameOverController : MonoBehaviour
    {
        private static GameOverController _instance;
        public static GameOverController Instance
        {
            get
            {
                return _instance;
            }
        }

        [SerializeField]
        internal GameObject GameOverPanel;

        [SerializeField]
        private GameSceneMediator _mediator;

        [SerializeField]
        internal Button ReplayBtn, BackMainMenuBtn, QuitGameBtn;

        [SerializeField]
        private CanvasGroup _canvasGroupBackground, _canvasGroupMenu;

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