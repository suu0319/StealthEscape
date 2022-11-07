using UnityEngine;

namespace Manager
{
    public class GameStateController : MonoBehaviour
    {
        private static GameStateController _instance;
        public static GameStateController Instance 
        {
            get 
            {
                return _instance;
            }
        }

        [SerializeField]
        private Animator _animator;

        private void Awake()
        {
            InitSingleton();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void InitSingleton()
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

        #region GameState
        /// <summary>
        /// 切換主選單狀態
        /// </summary>
        public void SwitchMainMenuState()
        {
            _animator.SetTrigger("MainMenu");
        }

        /// <summary>
        /// 切換選擇關卡狀態
        /// </summary>
        public void SwitchGameStageState()
        {
            _animator.SetTrigger("GameStage");
        }

        /// <summary>
        /// 切換遊戲場景狀態
        /// </summary>
        public void SwitchGameSceneState()
        {
            _animator.SetTrigger("GameScene");
        }

        /// <summary>
        /// 切換遊戲暫停狀態
        /// </summary>
        public void SwitchGameStopState() 
        {
            _animator.SetTrigger("GameStop");
        }

        /// <summary>
        /// 切換遊戲失敗狀態
        /// </summary>
        public void SwitchGameOverState()
        {
            _animator.SetTrigger("GameOver");
        }

        /// <summary>
        /// 切換遊戲過關狀態
        /// </summary>
        public void SwitchGameClearState()
        {
            _animator.SetTrigger("GameClear");
        }
        #endregion
    }
}