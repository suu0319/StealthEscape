using Manager;

namespace Factory
{
    public class ShootTrapFactory : BaseFactory
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
                for (int i = 0; i < GameStageData.ShootTrapAmountAuto; i++)
                {
                    SpawnFromPool();
                }
            }
            else
            {
                for (int i = 0; i < GameStageData.ShootTrapDataList.Count; i++)
                {
                    SpawnFromPool();
                }
            }
        }
    }
}