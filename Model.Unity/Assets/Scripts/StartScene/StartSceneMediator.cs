using UnityEngine;
using GameStage;

namespace Mediator
{
    public class StartSceneMediator : BaseMediator
    {
        public static StartSceneMediator Instance;

        [Header("Config")]
        [SerializeField]
        internal GameStageConfig GameStageConfig;

        [Header("Script")]
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
            ScreenFadeOut();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        protected override void InitSingleton()
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
    }
}