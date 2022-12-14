using System;
using UnityEngine;
using UnityEngine.UI;
using GameStage;
using Manager;

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
            GameManager.Instance.InitGameData();
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
                    _gameStageData.SoldierAmountAuto = (int)Math.Round((float)_gameStageData.SoldierDataList.Count * 0.8f);
                    _gameStageData.SoldierSpeedAuto = (6f * 0.6f);
                    _gameStageData.SpikeTrapAmountAuto = (int)Math.Round((float)_gameStageData.SpikeTrapDataList.Count * 0.8f);
                    _gameStageData.SpikeTrapIntervalAuto = (2f * 2.5f);
                    _gameStageData.ShootTrapAmountAuto = (int)Math.Round((float)_gameStageData.ShootTrapDataList.Count * 0.8f);
                    _gameStageData.ShootTrapIntervalAuto = (3f * 2f);
                    break;

                case 1:
                    _gameStageData.SoldierAmountAuto = (int)Math.Round((float)_gameStageData.SoldierDataList.Count * 0.8f);
                    _gameStageData.SoldierSpeedAuto = (6f * 0.8f);
                    _gameStageData.SpikeTrapAmountAuto = (int)Math.Round((float)_gameStageData.SpikeTrapDataList.Count * 0.8f);
                    _gameStageData.SpikeTrapIntervalAuto = (2f * 1.5f);
                    _gameStageData.ShootTrapAmountAuto = (int)Math.Round((float)_gameStageData.ShootTrapDataList.Count * 0.8f);
                    _gameStageData.ShootTrapIntervalAuto = (3f * 1.2f);
                    break;

                case 2:
                    _gameStageData.SoldierAmountAuto = _gameStageData.SoldierDataList.Count;
                    _gameStageData.SoldierSpeedAuto = 6f;
                    _gameStageData.SpikeTrapAmountAuto = _gameStageData.SpikeTrapDataList.Count;
                    _gameStageData.SpikeTrapIntervalAuto = 2f;
                    _gameStageData.ShootTrapAmountAuto = _gameStageData.ShootTrapDataList.Count;
                    _gameStageData.ShootTrapIntervalAuto = 3f;
                    break;
            }
        }
    }
}