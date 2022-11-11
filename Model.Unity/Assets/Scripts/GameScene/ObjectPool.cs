using System.Collections.Generic;
using UnityEngine;
using GameStage;
using Manager;

namespace ObjectPool
{
    [System.Serializable]
    public class Pool
    {
        public string Name;
        public GameObject Prefab;
        public int Amount;
    }

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;

        [SerializeField]
        private List<Pool> poolList;
        private Dictionary<string, Queue<GameObject>> poolDict;

        private const string soldierName = "Soldier";
        private const string spikeTrapName = "SpikeTrap";
        private const string shootTrapName = "ShootTrap";

        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            InitObjectPool();
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

        /// <summary>
        /// 初始化物件池
        /// </summary>
        private void InitObjectPool()
        {
            int stageIndex = GameManager.Instance.GameSceneData.StageIndex;
            GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageIndex];

            poolDict = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in poolList)
            {
                DetectObjectType(pool, gameStageData);

                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Amount; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDict.Add(pool.Name, objectPool);
            }
        }

        /// <summary>
        /// 判斷物件種類
        /// </summary>
        /// <param name="pool">物件池</param>
        /// <param name="gameStageData">關卡資料</param>
        private void DetectObjectType(Pool pool, GameStageData gameStageData) 
        {
            switch (pool.Name)
            {
                case soldierName:
                    if (!gameStageData.IsManual)
                    {
                        pool.Amount = gameStageData.SoldierAmountAuto;
                    }
                    else
                    {
                        pool.Amount = gameStageData.SoldierDataList.Count;
                    }
                    break;

                case spikeTrapName:
                    if (!gameStageData.IsManual)
                    {
                        pool.Amount = gameStageData.SpikeTrapAmountAuto;
                    }
                    else
                    {
                        pool.Amount = gameStageData.SpikeTrapDataList.Count;
                    }
                    break;

                case shootTrapName:
                    if (!gameStageData.IsManual)
                    {
                        pool.Amount = gameStageData.ShootTrapAmountAuto;
                    }
                    else
                    {
                        pool.Amount = gameStageData.ShootTrapDataList.Count;
                    }
                    break;

                default:
                    Debug.LogWarning("Pool with name doest't exist");
                    break;
            }
        }

        /// <summary>
        /// 生成物件
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="position">座標</param>
        /// <param name="rotation">旋轉</param>
        /// <returns></returns>
        internal GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
        {
            if (!poolDict.ContainsKey(name))
            {
                Debug.LogWarning("Pool with name doest't exist");
                return null;
            }

            GameObject objectToSpawn = poolDict[name].Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            objectToSpawn.SetActive(true);
            poolDict[name].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        /// <summary>
        /// 回收物件
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="gameObject">物件</param>
        internal void RecycleToPool(string name, GameObject gameObject)
        {
            poolDict[name].Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}