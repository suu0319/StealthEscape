using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameStage;

namespace Manager
{
    [System.Serializable]
    public class GameSceneData
    {
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
        internal int SoldierEnemyAmount;
        [SerializeField]
        internal float SoldierEnemySpeed;
        [SerializeField]
        internal int SpikeTrapAmount;
        [SerializeField]
        internal float SpikeTrapInterval;
        [SerializeField]
        internal int ShootTrapAmount;
        [SerializeField]
        internal float ShootTrapInterval;

        internal GameObject[] SoldierGroup;
        internal List<GameObject> SoldierExistList;

        internal GameObject[] SpikeTrapGroup;
        internal List<GameObject> SpikeTrapExistList;

        internal GameObject[] ShootTrapGroup;
        internal List<GameObject> ShootTrapExistList;
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        internal GameStageConfig GameStageConfig;

        [SerializeField]
        internal GameSceneData GameSceneData;

        internal int PassLevelNum = 0;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

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
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        /// 初始化遊戲場景資料
        /// </summary>
        internal void InitGameSceneData()
        {
            Scene gameScene = SceneManager.GetActiveScene();

            for (int i = 0; i < GameStageConfig.StageDataList.Count; i++) 
            {
                if (gameScene.name == GameStageConfig.StageDataList[i].Image.name) 
                {
                    GameSceneData.IsCheat = GameStageConfig.IsCheat;
                    GameSceneData.PlayerSpeed = GameStageConfig.PlayerSpeed;
                    GameSceneData.Name = GameStageConfig.StageDataList[i].Image.name;
                    GameSceneData.Level = GameStageConfig.StageDataList[i].Level.ToString();
                    GameSceneData.BGM = GameStageConfig.StageDataList[i].BGM;

                    GameSceneData.SoldierEnemyAmount = GameStageConfig.StageDataList[i].SoldierEnemyAmount;
                    GameSceneData.SoldierEnemySpeed = GameStageConfig.StageDataList[i].SoldierEnemySpeed;
                    GameSceneData.SpikeTrapAmount = GameStageConfig.StageDataList[i].SpikeTrapAmount;
                    GameSceneData.SpikeTrapInterval = GameStageConfig.StageDataList[i].SpikeTrapInterval;
                    GameSceneData.ShootTrapAmount = GameStageConfig.StageDataList[i].ShootTrapAmount;
                    GameSceneData.ShootTrapInterval = GameStageConfig.StageDataList[i].ShootTrapInterval;

                    InitSoldierAmount();
                    InitSpikeSpikeTrapAmount();
                    InitShootSpikeTrapAmount();

                    break;
                }
            }   
        }

        /// <summary>
        /// 初始化敵人數量
        /// </summary>
        private void InitSoldierAmount()
        {
            GameSceneData.SoldierGroup = GameObject.FindGameObjectsWithTag("SoldierEnemy");
            GameSceneData.SoldierExistList = new List<GameObject>();

            InitGameObjectAmount(GameSceneData.SoldierGroup, GameSceneData.SoldierExistList, GameSceneData.SoldierEnemyAmount);
        }

        /// <summary>
        /// 初始化地刺陷阱數量
        /// </summary>
        private void InitSpikeSpikeTrapAmount() 
        {
            GameSceneData.SpikeTrapGroup = GameObject.FindGameObjectsWithTag("SpikeTrap");
            GameSceneData.SpikeTrapExistList = new List<GameObject>();

            InitGameObjectAmount(GameSceneData.SpikeTrapGroup, GameSceneData.SpikeTrapExistList, GameSceneData.SpikeTrapAmount);
        }

        /// <summary>
        /// 初始化箭矢陷阱數量
        /// </summary>
        private void InitShootSpikeTrapAmount()
        {
            GameSceneData.ShootTrapGroup = GameObject.FindGameObjectsWithTag("ShootTrap");
            GameSceneData.ShootTrapExistList = new List<GameObject>();

            InitGameObjectAmount(GameSceneData.ShootTrapGroup, GameSceneData.ShootTrapExistList, GameSceneData.ShootTrapAmount);
        }

        /// <summary>
        /// 初始化物件數量
        /// </summary>
        /// <param name="gameObjects">物件(場景預設所有的)</param>
        /// <param name="gameObjectsExist">物件(會SetActive(true))</param>
        /// <param name="maxNum">物件最大數量</param>
        private void InitGameObjectAmount(GameObject[] gameObjects, List<GameObject> gameObjectsExist, int maxNum) 
        {
            int num = 0;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }

            while (num < maxNum)
            {
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i].SetActive(false);

                    int random = Random.Range(0, 2);

                    if ((random == 1) && (!gameObjectsExist.Contains(gameObjects[i])))
                    {
                        gameObjectsExist.Add(gameObjects[i]);
                        num++;

                        if (num >= maxNum)
                        {
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            for (int i = 0; i < gameObjectsExist.Count; i++)
            {
                gameObjectsExist[i].SetActive(true);
            }
        }
    }
}