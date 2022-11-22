using System;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Trap;
using Position;
using GameStage;
using Manager;

namespace ObjectPool
{
    public class ObjectPoolSpawner : MonoBehaviour
    {
        [Header("Soldier Settings")]
        [SerializeField]
        private GameObject _soldierParentObj;
        private const string soldierName = "Soldier";

        [Header("SpikeTrap Settings")]
        [SerializeField]
        private GameObject _spikeTrapParentObj;
        private const string spikeTrapName = "SpikeTrap";

        [Header("ShootTrap Settings")]
        [SerializeField]
        private GameObject _shootTrapParentObj;
        private const string shootTrapName = "ShootTrap";

        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            int stageIndex = GameManager.Instance.GameSceneData.StageIndex;
            GameSceneData gameSceneData = GameManager.Instance.GameSceneData;
            GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageIndex];

            if (!gameSceneData.IsManual)
            {
                RandomCreate<SoldierData>(gameSceneData.StageIndex);
                RandomCreate<SpikeTrapData>(gameSceneData.StageIndex);
                RandomCreate<ShootTrapData>(gameSceneData.StageIndex);
            }
            else
            {
                CreateEnemy<SoldierData>(gameStageData.SoldierDataList);
                CreateTrap<SpikeTrapData>(gameStageData.SpikeTrapDataList);
                CreateTrap<ShootTrapData>(gameStageData.ShootTrapDataList);
            }
        }

        /// <summary>
        /// 生成敵人
        /// </summary>
        /// <typeparam name="T">EnemyData</typeparam>
        /// <param name="enemyDataList">enemyDataList</param>
        private void CreateEnemy<T>(List<T> enemyDataList) where T : EnemyData
        {
            try
            {
                for (int i = 0; i < enemyDataList.Count; i++)
                {
                    Vector3[] patrolPosition = enemyDataList[i].PatrolPointsData.Position;

                    GameObject soldierObj = ObjectPool.Instance.SpawnFromPool(soldierName, patrolPosition[0], Quaternion.identity);

                    switch (typeof(T))
                    {
                        case Type t when t == typeof(SoldierData):
                            SoldierAIController soldierAIController = soldierObj.GetComponent<SoldierAIController>();
                            soldierObj.transform.parent = _soldierParentObj.transform;
                            soldierAIController.Agent.speed = enemyDataList[i].Speed;

                            for (int y = 0; y < patrolPosition.Length; y++)
                            {
                                soldierAIController.PatrolPoint.Add(patrolPosition[y]);
                            }
                            break;

                        default:
                            Debug.LogError("Enemy doesn't Exist");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("EnemyDataList資料缺失: " + e.Message);
            }
        }

        /// <summary>
        /// 生成陷阱
        /// </summary>
        /// <typeparam name="T">TrapData</typeparam>
        /// <param name="trapDataList">trapDataList</param>
        private void CreateTrap<T>(List<T> trapDataList) where T: TrapData
        {
            try
            {
                for (int i = 0; i < trapDataList.Count; i++)
                {
                    Vector3 position = trapDataList[i].PositionData.Position;
                    Quaternion rotation = trapDataList[i].PositionData.Rotation;

                    switch (typeof(T))
                    {
                        case Type t when t == typeof(SpikeTrapData):
                            GameObject spikeTrapObj = ObjectPool.Instance.SpawnFromPool(spikeTrapName, position, rotation);
                            SpikeTrap spikeTrap = spikeTrapObj.GetComponentInChildren<SpikeTrap>();
                            spikeTrapObj.transform.parent = _spikeTrapParentObj.transform;
                            spikeTrap.Interval = trapDataList[i].Interval;
                            break;

                        case Type t when t == typeof(ShootTrapData):
                            GameObject shootTrapObj = ObjectPool.Instance.SpawnFromPool(shootTrapName, position, rotation);
                            ShootTrap shootTrap = shootTrapObj.GetComponentInChildren<ShootTrap>();
                            shootTrapObj.transform.parent = _shootTrapParentObj.transform;
                            shootTrap.Interval = trapDataList[i].Interval;
                            shootTrap.Speed = (trapDataList[i] as ShootTrapData).Speed;
                            break;

                        default:
                            Debug.LogError("Trap doesn't Exist");
                            break;
                    }
                }
            }        
            catch (Exception e)
            {
                Debug.LogError("TrapDataList資料缺失: " + e.Message);
            }
        }

        /// <summary>
        /// 隨機生成
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="stageNum">關卡編號</param>
        private void RandomCreate<T>(int stageNum)
        {
            try
            {
                int objAmount = 0;
                GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageNum];

                switch (typeof(T))
                {
                    case Type t when t == typeof(SoldierData):
                        objAmount = GameManager.Instance.GameSceneData.SoldierAmount;
                        RandomCreateSoldierLoop<SoldierData>(objAmount, stageNum, gameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList);
                        break;

                    case Type t when t == typeof(SpikeTrapData):
                        objAmount = GameManager.Instance.GameSceneData.SpikeTrapAmount;
                        RandomCreateTrapLoop<SpikeTrapData>(objAmount, stageNum, gameStageData.SpikeTrapPositionConfig.PositionDataList);
                        break;

                    case Type t when t == typeof(ShootTrapData):
                        objAmount = GameManager.Instance.GameSceneData.ShootTrapAmount;
                        RandomCreateTrapLoop<ShootTrapData>(objAmount, stageNum, gameStageData.ShootTrapPositionConfig.PositionDataList);
                        break;

                    default:
                        Debug.LogError("Soldier or Trap doesn't Exist");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GameStageConfig.StageDataList資料缺失: " + e.Message);
            }
        }

        /// <summary>
        /// 隨機生成士兵Loop
        /// </summary>
        /// <param name="amount">士兵數量</param>
        /// <param name="stageNum">關卡編號</param>
        /// <param name="positionData">座標資料</param>
        private void RandomCreateSoldierLoop<T>(int amount, int stageNum, List<PatrolPointsData> positionData) where T : EnemyData
        {
            try
            {
                GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageNum];

                int num = 0;
                int soldierAmount = gameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList.Count;
                List<int> randomExistList = new List<int>();

                while (num < amount)
                {
                    int randomIndex = UnityEngine.Random.Range(0, soldierAmount);

                    Vector3[] patrolPosition = positionData[randomIndex].Position;

                    if (!randomExistList.Contains(randomIndex))
                    {
                        switch (typeof(T))
                        {
                            case Type t when t == typeof(SoldierData):
                                GameObject soldierObj = ObjectPool.Instance.SpawnFromPool(soldierName, patrolPosition[0], Quaternion.identity);
                                SoldierAIController soldierAIController = soldierObj.GetComponent<SoldierAIController>();
                                soldierObj.transform.parent = _soldierParentObj.transform;
                                soldierAIController.Agent.speed = gameStageData.SoldierSpeedAuto;

                                for (int i = 0; i < patrolPosition.Length; i++)
                                {
                                    soldierAIController.PatrolPoint.Add(patrolPosition[i]);
                                }

                                randomExistList.Add(randomIndex);
                                num++;
                                break;

                            default:
                                Debug.LogError("Enemy doesn't Exist");
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GameStageConfig.StageDataList資料缺失: " + e.Message);
            }
        }

        /// <summary>
        /// 隨機生成陷阱Loop
        /// </summary>
        /// <param name="amount">陷阱數量</param>
        /// <param name="stageNum">關卡編號</param>
        /// <param name="positionData">座標資料</param>
        private void RandomCreateTrapLoop<T>(int amount, int stageNum, List<PositionData> positionData) where T : TrapData
        {
            try
            {
                GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageNum];

                int num = 0;
                int trapAmount = 0;
                List<int> randomExistList = new List<int>();

                switch (typeof(T))
                {
                    case Type t when t == typeof(SpikeTrapData):
                        trapAmount = gameStageData.SpikeTrapPositionConfig.PositionDataList.Count;
                        break;

                    case Type t when t == typeof(ShootTrapData):
                        trapAmount = gameStageData.ShootTrapPositionConfig.PositionDataList.Count;
                        break;

                    default:
                        Debug.LogError("Trap doesn't Exist");
                        break;
                }

                while (num < amount)
                {
                    int randomIndex = UnityEngine.Random.Range(0, trapAmount);

                    Vector3 position = positionData[randomIndex].Position;
                    Quaternion rotation = positionData[randomIndex].Rotation;

                    if (!randomExistList.Contains(randomIndex))
                    {
                        switch (typeof(T))
                        {
                            case Type t when t == typeof(SpikeTrapData):
                                GameObject spikeTrapObj = ObjectPool.Instance.SpawnFromPool(spikeTrapName, position, rotation);
                                SpikeTrap spikeTrap = spikeTrapObj.GetComponentInChildren<SpikeTrap>();
                                spikeTrapObj.transform.parent = _spikeTrapParentObj.transform;
                                spikeTrap.Interval = gameStageData.SpikeTrapIntervalAuto;

                                randomExistList.Add(randomIndex);
                                num++;
                                break;

                            case Type t when t == typeof(ShootTrapData):
                                GameObject shootTrapObj = ObjectPool.Instance.SpawnFromPool(shootTrapName, position, rotation);
                                ShootTrap shootTrap = shootTrapObj.GetComponentInChildren<ShootTrap>();
                                shootTrapObj.transform.parent = _shootTrapParentObj.transform;
                                shootTrap.Interval = gameStageData.ShootTrapIntervalAuto;
                                shootTrap.Speed = 100f;

                                randomExistList.Add(randomIndex);
                                num++;
                                break;

                            default:
                                Debug.LogError("Trap doesn't Exist");
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("GameStageConfig.StageDataList資料缺失: " + e.Message);
            }
        }

        #region 物件池測試
        /// <summary>
        /// 生成士兵
        /// </summary>
        [ContextMenu("Spawn Soldier")]
        private void SpawnSoldier()
        {
            try
            {
                ObjectPool.Instance.SpawnFromPool("Soldier", new Vector3(0, 0, 0), Quaternion.identity);
            }
            catch (Exception e)
            {
                Debug.LogError("物件池已空: " + e.Message);
            }
        }
        #endregion
    }
}