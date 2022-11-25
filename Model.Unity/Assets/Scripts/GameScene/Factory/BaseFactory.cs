using System;
using UnityEngine;
using GameStage;
using Pool;
using Manager;

namespace Factory
{
    public abstract class BaseFactory : MonoBehaviour
    {
        [SerializeField]
        protected string ObjectPoolName;

        protected GameStageData GameStageData;
        protected GameSceneData GameSceneData;

        protected int StageIndex;

        /// <summary>
        /// 初始化生成
        /// </summary>
        internal abstract void InitSpawn();

        /// <summary>
        /// 生成(從物件池取出)
        /// </summary>
        internal void SpawnFromPool()
        {
            ObjectPool.Instance.SpawnFromPool(ObjectPoolName);
        }

        #region 物件池測試
        /// <summary>
        /// 生成測試
        /// </summary>
        [ContextMenu("SpawnTest")]
        private void SpawnTest()
        {
            try
            {
                ObjectPool.Instance.SpawnFromPool(ObjectPoolName);
            }
            catch (Exception e)
            {
                Debug.LogError("物件池已空: " + e.Message);
            }
        }
        #endregion
    }
}