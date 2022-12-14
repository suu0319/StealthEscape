using System;
using UnityEngine;
using Trap;
using Pool;
using Manager;

namespace Factory
{
    public class SpikeTrapFactory : BaseFactory
    {
        private const string spikeTrapName = "SpikeTrap";

        /// <summary>
        /// 初始化生成
        /// </summary>
        internal override void InitSpawn()
        {
            StageIndex = GameManager.Instance.GameSceneData.StageIndex;
            GameStageData = GameManager.Instance.GameStageConfig.StageDataList[StageIndex];
            GameSceneData = GameManager.Instance.GameSceneData;
            ObjAmount = GameStageData.SpikeTrapPositionConfig.PositionDataList.Count;

            if (!GameStageData.IsManual)
            {
                for (int i = 0; i < GameStageData.SpikeTrapAmountAuto; i++)
                {
                    RandomIndex = UnityEngine.Random.Range(0, ObjAmount);

                    while (RandomExistList.Contains(RandomIndex))
                    {
                        RandomIndex = UnityEngine.Random.Range(0, ObjAmount);
                    }

                    RandomExistList.Add(RandomIndex);

                    SpawnFromPool(GameStageData.SpikeTrapDataList[RandomIndex]);
                }
            }
            else
            {
                for (int i = 0; i < GameStageData.SpikeTrapDataList.Count; i++)
                {
                    SpawnFromPool(GameStageData.SpikeTrapDataList[i]);
                }
            }
        }

        /// <summary>
        /// 生成(從物件池取出)
        /// </summary>
        /// <param name="data">SpikeTrapData</param>
        internal void SpawnFromPool(SpikeTrapData data)
        {
            Vector3 position = data.PositionData.Position;
            Quaternion rotation = data.PositionData.Rotation;

            GameObject spikeTrapObj = ObjectPool.Instance.SpawnFromPool(spikeTrapName, position, rotation);

            SpikeTrap spikeTrap = spikeTrapObj.GetComponentInChildren<SpikeTrap>();

            if (!GameStageData.IsManual)
            {
                spikeTrap.Interval = GameStageData.SpikeTrapIntervalAuto;
            }
            else
            {
                spikeTrap.Interval = data.Interval;
            }
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

                SpawnFromPool(GameStageData.SpikeTrapDataList[RandomIndex]);
            }
            catch (Exception e)
            {
                Debug.LogError("物件池已空: " + e.Message);
            }
        }
        #endregion
    }
}