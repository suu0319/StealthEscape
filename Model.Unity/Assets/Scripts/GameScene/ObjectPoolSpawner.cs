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
                RandomCreate(soldierName, gameSceneData.StageIndex);
                RandomCreate(spikeTrapName, gameSceneData.StageIndex);
                RandomCreate(shootTrapName, gameSceneData.StageIndex);
            }
            else
            {
                CreateEnemy(soldierName, gameStageData.SoldierDataList);
                CreateTrap(spikeTrapName, gameStageData.SpikeTrapDataList);
                CreateTrap(shootTrapName, gameStageData.ShootTrapDataList);
            }
        }

        /// <summary>
        /// 生成敵人
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">敵人名稱</param>
        /// <param name="enemyData">敵人資料List</param>
        private void CreateEnemy<T>(string name, List<T> enemyDataList) where T : EnemyData
        {
            for (int i = 0; i < enemyDataList.Count; i++)
            {
                Vector3[] patrolPosition = enemyDataList[i].PatrolPointsData.Position;

                GameObject soldierObj = ObjectPool.Instance.SpawnFromPool(soldierName, patrolPosition[0], Quaternion.identity);

                switch (name)
                {
                    case soldierName:
                        SoldierAIController soldierAIController = soldierObj.GetComponent<SoldierAIController>();
                        soldierObj.transform.parent = _soldierParentObj.transform;
                        soldierAIController.Agent.speed = enemyDataList[i].Speed;

                        for (int y = 0; y < patrolPosition.Length; y++)
                        {
                            soldierAIController.PatrolPoint.Add(patrolPosition[y]);
                        }
                        break;

                    default:
                        Debug.LogWarning("Enemy doesn't Exist");
                        break;
                }
            }
        }

        /// <summary>
        /// 生成陷阱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">陷阱名稱</param>
        /// <param name="trapDataList">陷阱資料List</param>
        private void CreateTrap<T>(string name, List<T> trapDataList) where T: TrapData
        {
            for (int i = 0; i < trapDataList.Count; i++)
            {
                Vector3 position = trapDataList[i].PositionData.Position;
                Quaternion rotation = trapDataList[i].PositionData.Rotation;

                GameObject trapObj = ObjectPool.Instance.SpawnFromPool(name, position, rotation);

                switch (name)
                {
                    case spikeTrapName:
                        SpikeTrap spikeTrap = trapObj.GetComponentInChildren<SpikeTrap>();
                        trapObj.transform.parent = _spikeTrapParentObj.transform;
                        spikeTrap.Interval = trapDataList[i].Interval;
                        break;

                    case shootTrapName:
                        ShootTrap shootTrap = trapObj.GetComponentInChildren<ShootTrap>();
                        trapObj.transform.parent = _shootTrapParentObj.transform;
                        shootTrap.Interval = trapDataList[i].Interval;
                        shootTrap.Speed = (trapDataList[i] as ShootTrapData).Speed;
                        break;

                    default:
                        Debug.LogWarning("Trap doesn't Exist");
                        break;
                }
            }
        }

        /// <summary>
        /// 隨機生成
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="stageNum">關卡編號</param>
        private void RandomCreate(string name, int stageNum)
        {
            int trapAmount = 0;
            GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageNum];

            switch (name) 
            {
                case soldierName:
                    trapAmount = GameManager.Instance.GameSceneData.SoldierAmount;
                    RandomCreateSoldierLoop(name, trapAmount, stageNum, gameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList);
                    break;

                case spikeTrapName:
                    trapAmount = GameManager.Instance.GameSceneData.SpikeTrapAmount;
                    RandomCreateTrapLoop(name, trapAmount, stageNum, gameStageData.SpikeTrapPositionConfig.PositionDataList);
                    break;

                case shootTrapName:
                    trapAmount = GameManager.Instance.GameSceneData.ShootTrapAmount;
                    RandomCreateTrapLoop(name, trapAmount, stageNum, gameStageData.ShootTrapPositionConfig.PositionDataList);
                    break;

                default:
                    Debug.LogWarning("Soldier or Trap doesn't Exist");
                    break;
            }
        }

        /// <summary>
        /// 隨機生成士兵Loop
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="amount">物件數量</param>
        /// <param name="stageNum">關卡編號</param>
        /// <param name="positionData">座標資料</param>
        private void RandomCreateSoldierLoop(string name, int amount, int stageNum, List<PatrolPointsData> positionData)
        {
            int num = 0;
            List<int> randomExistList = new List<int>();

            GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageNum];
            int soldierAmount = GameManager.Instance.GameSceneData.SoldierAmount;

            while (num < amount)
            {
                int random = Random.Range(0, 2);
                int randomIndex = Random.Range(0, soldierAmount);

                Vector3[] patrolPosition = positionData[randomIndex].Position;

                if ((random == 1) && (!randomExistList.Contains(randomIndex)))
                {
                    switch (name)
                    {
                        case soldierName:
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
                            Debug.LogWarning("Enemy doesn't Exist");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 隨機生成陷阱Loop
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="amount">物件數量</param>
        /// <param name="stageNum">關卡編號</param>
        /// <param name="positionData">座標資料</param>
        private void RandomCreateTrapLoop(string name, int amount, int stageNum, List<PositionData> positionData)
        {
            int num = 0;
            List<int> randomExistList = new List<int>();
            GameStageData gameStageData = GameManager.Instance.GameStageConfig.StageDataList[stageNum];

            while (num < amount)
            {
                int random = Random.Range(0, 2);
                int randomIndex = Random.Range(0, amount);

                Vector3 position = positionData[randomIndex].Position;
                Quaternion rotation = positionData[randomIndex].Rotation;

                if ((random == 1) && (!randomExistList.Contains(randomIndex)))
                {
                    switch (name)
                    {
                        case spikeTrapName:
                            GameObject spikeTrapObj = ObjectPool.Instance.SpawnFromPool(spikeTrapName, position, rotation);
                            SpikeTrap spikeTrap = spikeTrapObj.GetComponentInChildren<SpikeTrap>();
                            spikeTrapObj.transform.parent = _spikeTrapParentObj.transform;
                            spikeTrap.Interval = gameStageData.SpikeTrapIntervalAuto;

                            randomExistList.Add(randomIndex);
                            num++;
                            break;

                        case shootTrapName:
                            GameObject shootTrapObj = ObjectPool.Instance.SpawnFromPool(shootTrapName, position, rotation);
                            ShootTrap shootTrap = shootTrapObj.GetComponentInChildren<ShootTrap>();
                            shootTrapObj.transform.parent = _shootTrapParentObj.transform;
                            shootTrap.Interval = gameStageData.ShootTrapIntervalAuto;
                            shootTrap.Speed = 100f;

                            randomExistList.Add(randomIndex);
                            num++;
                            break;

                        default:
                            Debug.LogWarning("Trap doesn't Exist");
                            break;
                    }
                }
            }
        }
    }
}