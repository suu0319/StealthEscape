using UnityEngine;
using UnityEngine.SceneManagement;
using GameStage;

namespace Manager
{
    [System.Serializable]
    public class GameSceneData
    {
        [SerializeField]
        internal int StageIndex;
        [SerializeField]
        internal bool IsCheat;
        [SerializeField]
        internal float PlayerSpeed;
        [SerializeField]
        internal string Name;
        [SerializeField]
        internal string Level;
        [SerializeField]
        internal AudioClip BGM;

        [SerializeField]
        internal bool IsManual;
        [SerializeField]
        internal int SoldierAmount;
        [SerializeField]
        internal float SoldierSpeed;

        [SerializeField]
        internal int SpikeTrapAmount;
        [SerializeField]
        internal float SpikeTrapInterval;
        [SerializeField]
        internal int ShootTrapAmount;
        [SerializeField]
        internal float ShootTrapInterval;
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Config")]
        [SerializeField]
        internal GameStageConfig GameStageConfig;

        [SerializeField]
        internal GameSceneData GameSceneData;

        internal int PassLevelNum = 0;

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
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        /// 取得關卡編號
        /// </summary>
        internal void GetStageIndex()
        {
            Scene gameScene = SceneManager.GetActiveScene();

            for (int i = 0; i < GameStageConfig.StageDataList.Count; i++)
            {
                if (gameScene.name == GameStageConfig.StageDataList[i].SceneName)
                {
                    GameSceneData.StageIndex = i;
                }
            }
        }

        /// <summary>
        /// 初始化遊戲場景資料
        /// </summary>
        internal void InitGameSceneData()
        {
            GameStageData gameStageData = GameStageConfig.StageDataList[GameSceneData.StageIndex];

            GameSceneData.IsCheat = GameStageConfig.IsCheat;
            GameSceneData.PlayerSpeed = GameStageConfig.PlayerSpeed;

            GameSceneData.Name = gameStageData.SceneName;
            GameSceneData.BGM = gameStageData.BGM;
            GameSceneData.IsManual = gameStageData.IsManual;

            if (!gameStageData.IsManual)
            {
                GameSceneData.Level = gameStageData.Level.ToString();
                GameSceneData.SoldierAmount = gameStageData.SoldierAmountAuto;
                GameSceneData.SoldierSpeed = gameStageData.SoldierSpeedAuto;
                GameSceneData.SpikeTrapAmount = gameStageData.SpikeTrapAmountAuto;
                GameSceneData.SpikeTrapInterval = gameStageData.SpikeTrapIntervalAuto;
                GameSceneData.ShootTrapAmount = gameStageData.ShootTrapAmountAuto;
                GameSceneData.ShootTrapInterval = gameStageData.ShootTrapIntervalAuto;
            }
            else
            {
                GameSceneData.SoldierAmount = gameStageData.SoldierDataList.Count;
                GameSceneData.SpikeTrapAmount = gameStageData.SpikeTrapDataList.Count;
                GameSceneData.ShootTrapAmount = gameStageData.ShootTrapDataList.Count;
            }
        }
    }
}