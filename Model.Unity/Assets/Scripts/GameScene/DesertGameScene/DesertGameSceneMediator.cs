using UnityEngine;
using Puzzle;
using Manager;

namespace Mediator 
{
    public class DesertGameSceneMediator : GameSceneMediator
    {
        private static DesertGameSceneMediator _instance;
        public static DesertGameSceneMediator Instance
        {
            get 
            {
                return _instance;
            }
        }

        [SerializeField]
        internal PintuPuzzleTrigger PintuPuzzleTrigger;

        private void Awake()
        {
            InitSingleton();
            InitGameSceneData();
        }

        private void Start()
        {
            ScreenFadeOut();
            ShowGameSceneData();
            InitOptionMenu();
            InitGameOverMenu();
            GetSceneIndex();
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

        /// <summary>
        /// 顯示遊戲場景資料
        /// </summary>
        protected override void ShowGameSceneData() 
        {
            Debug.Log("作弊模式: " + GameManager.Instance.GameSceneData.IsCheat);
            Debug.Log("玩家速度: " + GameManager.Instance.GameSceneData.PlayerSpeed);
            Debug.Log("關卡名稱: " + GameManager.Instance.GameSceneData.Name);
            Debug.Log("關卡難度: " + GameManager.Instance.GameSceneData.Level);
            Debug.Log("關卡音樂: " + GameManager.Instance.GameSceneData.BGM);
            Debug.Log("士兵敵人數量: " + GameManager.Instance.GameSceneData.SoldierEnemyAmount);
            Debug.Log("士兵敵人速度: " + GameManager.Instance.GameSceneData.SoldierEnemySpeed);
            Debug.Log("地刺陷阱數量: " + GameManager.Instance.GameSceneData.SpikeTrapAmount);
            Debug.Log("地刺陷阱速度: " + GameManager.Instance.GameSceneData.SpikeTrapInterval);
            Debug.Log("箭矢陷阱數量: " + GameManager.Instance.GameSceneData.ShootTrapAmount);
            Debug.Log("箭矢陷阱速度: " + GameManager.Instance.GameSceneData.ShootTrapInterval);
        }
    }
}