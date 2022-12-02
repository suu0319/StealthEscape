using System;
using UnityEngine;
using Trap;
using Pool;
using Manager;

namespace Factory
{
    public class ShootTrapFactory : BaseFactory
    {
        private const string shootTrapName = "ShootTrap";

        /// <summary>
        /// 初始化生成
        /// </summary>
        internal override void InitSpawn()
        {
            StageIndex = GameManager.Instance.GameSceneData.StageIndex;
            GameStageData = GameManager.Instance.GameStageConfig.StageDataList[StageIndex];
            GameSceneData = GameManager.Instance.GameSceneData;
            ObjAmount = GameStageData.ShootTrapPositionConfig.PositionDataList.Count;

            if (!GameStageData.IsManual)
            {
                for (int i = 0; i < GameStageData.ShootTrapAmountAuto; i++)
                {
                    RandomIndex = UnityEngine.Random.Range(0, ObjAmount);

                    while (RandomExistList.Contains(RandomIndex))
                    {
                        RandomIndex = UnityEngine.Random.Range(0, ObjAmount);
                    }

                    RandomExistList.Add(RandomIndex);

                    SpawnFromPool(GameStageData.ShootTrapDataList[RandomIndex]);
                }
            }
            else
            {
                for (int i = 0; i < GameStageData.ShootTrapDataList.Count; i++)
                {
                    SpawnFromPool(GameStageData.ShootTrapDataList[i]);
                }
            }
        }

        /// <summary>
        /// 生成(從物件池取出)
        /// </summary>
        /// <param name="data">ShootTrapData</param>
        internal void SpawnFromPool(ShootTrapData data)
        {
            Vector3 position = data.PositionData.Position;
            Quaternion rotation = data.PositionData.Rotation;

            GameObject shootTrapObj = ObjectPool.Instance.SpawnFromPool(shootTrapName, position, rotation);

            ShootTrap shootTrap = shootTrapObj.GetComponentInChildren<ShootTrap>();
            shootTrap.Interval = data.Interval;
            shootTrap.Speed = data.Speed;
        }

        #region 物件池測試
        /// <summary>
        /// 生成測試
        /// </summary>
        [ContextMenu("SpawnTest")]
        protected override void SpawnTest()
        {
            try
            {
                RandomIndex = UnityEngine.Random.Range(0, ObjAmount);

                SpawnFromPool(GameStageData.ShootTrapDataList[RandomIndex]);
            }
            catch (Exception e)
            {
                Debug.LogError("物件池已空: " + e.Message);
            }
        }
        #endregion
    }
}