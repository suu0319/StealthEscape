using System.Collections.Generic;
using UnityEngine;
using GameStage;
using Manager;

namespace Factory
{
    public abstract class BaseFactory : MonoBehaviour
    {
        protected GameStageData GameStageData;
        protected GameSceneData GameSceneData;

        protected int StageIndex = 0;
        protected int ObjAmount = 0;
        protected int RandomIndex = 0;

        protected List<int> RandomExistList = new List<int>();

        /// <summary>
        /// 初始化生成
        /// </summary>
        internal abstract void InitSpawn();

        #region 物件池測試
        /// <summary>
        /// 生成測試
        /// </summary>
        protected abstract void SpawnTest();
        #endregion
    }
}