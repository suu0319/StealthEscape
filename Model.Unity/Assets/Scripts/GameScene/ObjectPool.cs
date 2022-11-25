using System;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Trap;
using GameStage;
using Manager;

namespace Pool
{
    [System.Serializable]
    public class Pool
    {
        public string Name;
        public GameObject Prefab;
        public GameObject ParentObj;

        [HideInInspector]
        public int Amount;

        [HideInInspector]
        public List<int> randomExistList = new List<int>();
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

        private GameStageData _gameStageData;

        private int stageIndex;

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
            }
        }

        /// <summary>
        /// 初始化物件池
        /// </summary>
        internal void InitObjectPool()
        {
            stageIndex = GameManager.Instance.GameSceneData.StageIndex;
            _gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageIndex];

            poolDict = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in poolList)
            {
                DetectObjectType(pool);

                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Amount; i++)
                {
                    switch (pool.Name)
                    {
                        case soldierName:
                            GameObject soldierObj = InitSoldierSettings(i, pool.randomExistList, pool.Prefab);
                            soldierObj.transform.parent = pool.ParentObj.transform;
                            soldierObj.SetActive(false);
                            objectPool.Enqueue(soldierObj);
                            break;

                        case spikeTrapName:
                            GameObject spikeTrapObj = InitSpikeTrapSettings(i, pool.randomExistList, pool.Prefab);
                            spikeTrapObj.transform.parent = pool.ParentObj.transform;
                            spikeTrapObj.SetActive(false);
                            objectPool.Enqueue(spikeTrapObj);
                            break;

                        case shootTrapName:
                            GameObject shootTrapObj = InitShootTrapSettings(i, pool.randomExistList, pool.Prefab);
                            shootTrapObj.transform.parent = pool.ParentObj.transform;
                            shootTrapObj.SetActive(false);
                            objectPool.Enqueue(shootTrapObj);
                            break;

                        default:
                            Debug.LogError("Pool with name doest't exist");
                            break;
                    }
                }

                poolDict.Add(pool.Name, objectPool);
            }
        }

        /// <summary>
        /// 判斷物件種類
        /// </summary>
        /// <param name="pool">物件池</param>
        private void DetectObjectType(Pool pool)
        {
            switch (pool.Name)
            {
                case soldierName:
                    if (!_gameStageData.IsManual)
                    {
                        pool.Amount = _gameStageData.SoldierAmountAuto;
                    }
                    else
                    {
                        pool.Amount = _gameStageData.SoldierDataList.Count;
                    }
                    break;

                case spikeTrapName:
                    if (!_gameStageData.IsManual)
                    {
                        pool.Amount = _gameStageData.SpikeTrapAmountAuto;
                    }
                    else
                    {
                        pool.Amount = _gameStageData.SpikeTrapDataList.Count;
                    }
                    break;

                case shootTrapName:
                    if (!_gameStageData.IsManual)
                    {
                        pool.Amount = _gameStageData.ShootTrapAmountAuto;
                    }
                    else
                    {
                        pool.Amount = _gameStageData.ShootTrapDataList.Count;
                    }
                    break;

                default:
                    Debug.LogError("Pool with name doest't exist");
                    break;
            }
        }

        /// <summary>
        /// 初始化士兵
        /// </summary>
        /// <param name="index">編號</param>
        /// <param name="randomExistList">randomExistList</param>
        /// <param name="prefab">prefab</param>
        /// <returns></returns>
        private GameObject InitSoldierSettings(int index, List<int> randomExistList, GameObject prefab)
        {
            try
            {
                if (!_gameStageData.IsManual)
                {
                    int soldierAmount = _gameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList.Count;
                    int randomIndex = UnityEngine.Random.Range(0, soldierAmount);

                    while (randomExistList.Contains(randomIndex))
                    {
                        randomIndex = UnityEngine.Random.Range(0, soldierAmount);
                    }

                    Vector3[] patrolPosition = _gameStageData.SoldierDataList[randomIndex].PatrolPointsData.Position;
                    GameObject soldierObj = Instantiate(prefab, patrolPosition[0], Quaternion.identity);
                    SoldierAIController soldierAIController = soldierObj.GetComponent<SoldierAIController>();
                    soldierAIController.Agent.speed = _gameStageData.SoldierSpeedAuto;

                    for (int i = 0; i < patrolPosition.Length; i++)
                    {
                        soldierAIController.PatrolPoint.Add(patrolPosition[i]);
                    }

                    randomExistList.Add(randomIndex);

                    return soldierObj;
                }
                else
                {
                    Vector3[] patrolPosition = _gameStageData.SoldierDataList[index].PatrolPointsData.Position;

                    GameObject soldierObj = Instantiate(prefab, patrolPosition[0], Quaternion.identity);

                    SoldierAIController soldierAIController = soldierObj.GetComponent<SoldierAIController>();
                    soldierAIController.Agent.speed = _gameStageData.SoldierDataList[index].Speed;

                    for (int y = 0; y < patrolPosition.Length; y++)
                    {
                        soldierAIController.PatrolPoint.Add(patrolPosition[y]);
                    }

                    return soldierObj;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GameStageConfig.StageDataList資料缺失: " + e.Message);

                return null;
            }
        }

        /// <summary>
        /// 初始化地刺陷阱
        /// </summary>
        /// <param name="index">編號</param>
        /// <param name="randomExistList">randomExistList</param>
        /// <param name="prefab">prefab</param>
        /// <returns></returns>
        private GameObject InitSpikeTrapSettings(int index, List<int> randomExistList, GameObject prefab)
        {
            try
            {
                if (!_gameStageData.IsManual)
                {
                    int spikeTrapAmount = _gameStageData.SpikeTrapPositionConfig.PositionDataList.Count;
                    int randomIndex = UnityEngine.Random.Range(0, spikeTrapAmount);

                    while (randomExistList.Contains(randomIndex))
                    {
                        randomIndex = UnityEngine.Random.Range(0, spikeTrapAmount);
                    }

                    Vector3 position = _gameStageData.SpikeTrapDataList[randomIndex].PositionData.Position;
                    Quaternion rotation = _gameStageData.SpikeTrapDataList[randomIndex].PositionData.Rotation;

                    GameObject spikeTrapObj = Instantiate(prefab, position, rotation);
                    SpikeTrap spikeTrap = spikeTrapObj.GetComponentInChildren<SpikeTrap>();
                    spikeTrap.Interval = _gameStageData.SpikeTrapIntervalAuto;

                    randomExistList.Add(randomIndex);

                    return spikeTrapObj;
                }
                else
                {
                    Vector3 position = _gameStageData.SpikeTrapDataList[index].PositionData.Position;
                    Quaternion rotation = _gameStageData.SpikeTrapDataList[index].PositionData.Rotation;

                    GameObject spikeTrapObj = Instantiate(prefab, position, rotation);
                    SpikeTrap spikeTrap = spikeTrapObj.GetComponentInChildren<SpikeTrap>();
                    spikeTrap.Interval = _gameStageData.SpikeTrapDataList[index].Interval;

                    return spikeTrapObj;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GameStageConfig.StageDataList資料缺失: " + e.Message);

                return null;
            }
        }

        /// <summary>
        /// 初始化箭矢陷阱
        /// </summary>
        /// <param name="index">編號</param>
        /// <param name="randomExistList">randomExistList</param>
        /// <param name="prefab">prefab</param>
        /// <returns></returns>
        private GameObject InitShootTrapSettings(int index, List<int> randomExistList, GameObject prefab)
        {
            try
            {
                if (!_gameStageData.IsManual)
                {
                    int shootTrapAmount = _gameStageData.ShootTrapPositionConfig.PositionDataList.Count;
                    int randomIndex = UnityEngine.Random.Range(0, shootTrapAmount);

                    while (randomExistList.Contains(randomIndex))
                    {
                        randomIndex = UnityEngine.Random.Range(0, shootTrapAmount);
                    }

                    Vector3 position = _gameStageData.ShootTrapDataList[randomIndex].PositionData.Position;
                    Quaternion rotation = _gameStageData.ShootTrapDataList[randomIndex].PositionData.Rotation;

                    GameObject shootTrapObj = Instantiate(prefab, position, rotation);
                    ShootTrap shootTrap = shootTrapObj.GetComponentInChildren<ShootTrap>();
                    shootTrap.Interval = _gameStageData.ShootTrapIntervalAuto;
                    shootTrap.Speed = _gameStageData.ShootTrapSpeedAuto;

                    randomExistList.Add(randomIndex);

                    return shootTrapObj;
                }
                else
                {
                    Vector3 position = _gameStageData.ShootTrapDataList[index].PositionData.Position;
                    Quaternion rotation = _gameStageData.ShootTrapDataList[index].PositionData.Rotation;

                    GameObject shootTrapObj = Instantiate(prefab, position, rotation);
                    ShootTrap shootTrap = shootTrapObj.GetComponentInChildren<ShootTrap>();
                    shootTrap.Interval = _gameStageData.ShootTrapDataList[index].Interval;
                    shootTrap.Speed = _gameStageData.ShootTrapDataList[index].Speed;

                    return shootTrapObj;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GameStageConfig.StageDataList資料缺失: " + e.Message);

                return null;
            }
        }

        /// <summary>
        /// 生成物件
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <returns></returns>
        internal void SpawnFromPool(string name)
        {
            if (!poolDict.ContainsKey(name))
            {
                Debug.LogError("Pool with name doest't exist");
            }

            GameObject objectToSpawn = poolDict[name].Dequeue();

            objectToSpawn.SetActive(true);
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