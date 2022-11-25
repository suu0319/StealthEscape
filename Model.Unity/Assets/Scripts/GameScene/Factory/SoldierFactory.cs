using Manager;

namespace Factory
{
    public class SoldierFactory : BaseFactory
    {
        /// <summary>
        /// 初始化生成
        /// </summary>
        internal override void InitSpawn()
        {
            StageIndex = GameManager.Instance.GameSceneData.StageIndex;
            GameStageData = GameManager.Instance.GameStageConfig.StageDataList[StageIndex];
            GameSceneData = GameManager.Instance.GameSceneData;

            if (!GameSceneData.IsManual)
            {
                for (int i = 0; i < GameStageData.SoldierAmountAuto; i++)
                {
                    SpawnFromPool();
                }
            }
            else
            {
                for (int i = 0; i < GameStageData.SoldierDataList.Count; i++)
                {
                    SpawnFromPool();
                }
            }
        }
    }
}