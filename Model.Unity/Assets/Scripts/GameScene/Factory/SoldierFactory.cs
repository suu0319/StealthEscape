using System;
using UnityEngine;
using Enemy;
using Pool;
using Manager;

namespace Factory
{
    public class SoldierFactory : BaseFactory
    {
        private const string soldierName = "Soldier";

        /// <summary>
        /// 初始化生成
        /// </summary>
        internal override void InitSpawn()
        {
            StageIndex = GameManager.Instance.GameSceneData.StageIndex;
            GameStageData = GameManager.Instance.GameStageConfig.StageDataList[StageIndex];
            GameSceneData = GameManager.Instance.GameSceneData;
            ObjAmount = GameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList.Count;

            if (!GameStageData.IsManual)
            {
                for (int i = 0; i < GameStageData.SoldierAmountAuto; i++) 
                {
                    RandomIndex = UnityEngine.Random.Range(0, ObjAmount);

                    while (RandomExistList.Contains(RandomIndex)) 
                    {
                        RandomIndex = UnityEngine.Random.Range(0, ObjAmount);
                    }

                    RandomExistList.Add(RandomIndex);

                    SpawnFromPool(GameStageData.SoldierDataList[RandomIndex]);
                }
            }
            else
            {
                for (int i = 0; i < GameStageData.SoldierDataList.Count; i++)
                {
                    SpawnFromPool(GameStageData.SoldierDataList[i]);
                }
            }
        }

        /// <summary>
        /// 生成(從物件池取出)
        /// </summary>
        /// <param name="data">SoldierData</param>
        internal void SpawnFromPool(SoldierData data)
        {
            Vector3[] patrolPosition;

            GameObject soldierObj;

            SoldierAIController soldierAIController;

            try
            {
                patrolPosition = data.PatrolPointsData.Position;

                soldierObj = ObjectPool.Instance.SpawnFromPool(soldierName, patrolPosition[0], Quaternion.identity);

                soldierAIController = soldierObj.GetComponent<SoldierAIController>();

                if (!GameStageData.IsManual)
                {
                    soldierAIController.Agent.speed = GameStageData.SoldierSpeedAuto;
                }
                else
                {
                    soldierAIController.Agent.speed = data.Speed;
                }

                for (int y = 0; y < patrolPosition.Length; y++)
                {
                    soldierAIController.PatrolPointsList.Add(patrolPosition[y]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);

                patrolPosition = GameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList[RandomIndex].Position;

                soldierObj = ObjectPool.Instance.SpawnFromPool(soldierName, patrolPosition[0], Quaternion.identity);

                soldierAIController = soldierObj.GetComponent<SoldierAIController>();

                RandomIndex = UnityEngine.Random.Range(0, ObjAmount);

                while (RandomExistList.Contains(RandomIndex))
                {
                    RandomIndex = UnityEngine.Random.Range(0, ObjAmount);
                }

                RandomExistList.Add(RandomIndex);

                for (int y = 0; y < patrolPosition.Length; y++)
                {
                    soldierAIController.PatrolPointsList.Add(patrolPosition[y]);
                }
            }
        }

        #region 物件池測試
        /// <summary>
        /// 生成測試
        /// </summary>
        [ContextMenu("SpawnTest")]
        protected override void SpawnTest()
        {
            RandomIndex = UnityEngine.Random.Range(0, ObjAmount);
            SpawnFromPool(GameStageData.SoldierDataList[RandomIndex]);
        }
        #endregion
    }
}