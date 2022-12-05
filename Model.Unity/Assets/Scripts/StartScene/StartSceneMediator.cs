using UnityEngine;
using UnityEngine.UI;
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

        [Header("UI")]
        [SerializeField]
        private Dropdown _stage;
        [SerializeField]
        private Dropdown _level;

        private GameStageData _gameStageData;
        private int stageIndex = 0;

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

        /// <summary>
        /// 改變關卡選項
        /// </summary>
        public void ChangeStage() 
        {
            switch (_stage.value) 
            {
                case 0:
                    stageIndex = 0;
                    break;

                case 1:
                    stageIndex = 1;
                    break;

                case 2:
                    stageIndex = 2;
                    break;
            }
        }

        /// <summary>
        /// 改變難度選項
        /// </summary>
        public void ChangeLevel() 
        {
            _gameStageData = GameStageConfig.StageDataList[stageIndex];

            switch (_level.value)
            {
                case 0:
                    _gameStageData.SoldierAmountAuto = 10;
                    _gameStageData.SoldierSpeedAuto = 3f;
                    _gameStageData.SpikeTrapAmountAuto = 45;
                    _gameStageData.SpikeTrapIntervalAuto = 5f;
                    _gameStageData.ShootTrapAmountAuto = 13;
                    _gameStageData.ShootTrapIntervalAuto = 4f;
                    break;

                case 1:
                    _gameStageData.SoldierAmountAuto = 13;
                    _gameStageData.SoldierSpeedAuto = 5f;
                    _gameStageData.SpikeTrapAmountAuto = 60;
                    _gameStageData.SpikeTrapIntervalAuto = 3f;
                    _gameStageData.ShootTrapAmountAuto = 13;
                    _gameStageData.ShootTrapIntervalAuto = 4f;
                    break;

                case 2:
                    _gameStageData.SoldierAmountAuto = 16;
                    _gameStageData.SoldierSpeedAuto = 6f;
                    _gameStageData.SpikeTrapAmountAuto = 75;
                    _gameStageData.SpikeTrapIntervalAuto = 2f;
                    _gameStageData.ShootTrapAmountAuto = 16;
                    _gameStageData.ShootTrapIntervalAuto = 3f;
                    break;
            }
        }
    }
}