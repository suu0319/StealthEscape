using UnityEngine;
using GameStage;
using GameLoading;

namespace Mediator 
{
    public class StartSceneMediator : BaseMediator
    {
        private static StartSceneMediator _instance;
        public static StartSceneMediator Instance 
        {
            get 
            {
                return _instance;
            }
        }

        [SerializeField]
        internal GameStageConfig GameStageConfig;
        [SerializeField]
        internal GameStageController GameStageController;
        [SerializeField]
        internal GameStagePanel GameStagePanel;

        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            GameLoadingPanel.Instance.ScreenFadeOut();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        protected override void InitSingleton()
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
    }
}