using UnityEngine;
using Puzzle;
using Manager;

namespace Mediator 
{
    public class DesertGameSceneMediator : GameSceneMediator
    {
        public static DesertGameSceneMediator Instance;

        [Header("Script")]
        [SerializeField]
        internal PintuPuzzleTrigger PintuPuzzleTrigger;

        private void Awake()
        {
            InitSingleton();
            InitGameSceneData();
        }

        private void Start()
        {
            ShowGameSceneData();
            InitOptionMenu();
            InitGameOverMenu();
            GetSceneIndex();
            ScreenFadeOut();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        protected override void InitSingleton()
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
        /// 顯示遊戲場景資料
        /// </summary>
        protected override void ShowGameSceneData() 
        {
            GameSceneData gameSceneData = GameManager.Instance.GameSceneData;

            Debug.Log("作弊模式: " + gameSceneData.IsCheat);
            Debug.Log("玩家速度: " + gameSceneData.PlayerSpeed);
            Debug.Log("關卡名稱: " + gameSceneData.Name);
            Debug.Log("關卡音樂: " + gameSceneData.BGM);

            if (!gameSceneData.IsManual)
            {
                Debug.Log("關卡難度: " + gameSceneData.Level);
            }

            Debug.Log("士兵敵人數量: " + gameSceneData.SoldierAmount);

            if (!gameSceneData.IsManual) 
            {
                Debug.Log("士兵敵人速度: " + gameSceneData.SoldierSpeed);
            }

            Debug.Log("地刺陷阱數量: " + gameSceneData.SpikeTrapAmount);

            if (!gameSceneData.IsManual)
            {
                Debug.Log("地刺陷阱間隔: " + gameSceneData.SpikeTrapInterval);
            }

            Debug.Log("箭矢陷阱數量: " + gameSceneData.ShootTrapAmount);

            if (!gameSceneData.IsManual)
            {
                Debug.Log("箭矢陷阱間隔: " + gameSceneData.ShootTrapInterval);
            }
        }
    }
}