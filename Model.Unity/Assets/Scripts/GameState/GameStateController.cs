using UnityEngine;

namespace Manager
{
    public class GameStateController : MonoBehaviour
    {
        public static GameStateController Instance;

        [Header("Animation")]
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
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        #region GameState
        /// <summary>
        /// 切換主選單狀態
        /// </summary>
        internal void SwitchMainMenuState()
        {
            _animator.SetTrigger("MainMenu");
        }

        /// <summary>
        /// 切換選擇關卡狀態
        /// </summary> 
        internal void SwitchGameStageState()
        {
            _animator.SetTrigger("GameStage");
        }

        /// <summary>
        /// 切換遊戲場景狀態
        /// </summary>
        internal void SwitchGameSceneState()
        {
            _animator.SetTrigger("GameScene");
        }

        /// <summary>
        /// 切換遊戲暫停狀態
        /// </summary>
        internal void SwitchGameStopState()
        {
            _animator.SetTrigger("GameStop");
        }

        /// <summary>
        /// 切換遊戲失敗狀態
        /// </summary>
        internal void SwitchGameOverState()
        {
            _animator.SetTrigger("GameOver");
        }

        /// <summary>
        /// 切換遊戲過關狀態
        /// </summary>
        internal void SwitchGameClearState()
        {
            _animator.SetTrigger("GameClear");
        }
        #endregion
    }
}