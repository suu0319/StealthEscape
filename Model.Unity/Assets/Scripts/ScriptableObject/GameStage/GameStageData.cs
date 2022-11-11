using UnityEngine;
using System.Collections.Generic;
using Enemy;
using Trap;
using Position;

namespace GameStage 
{
    [System.Serializable]
    public class GameStageData
    {
        #region 關卡基本設定
        public Level Level;
        public Sprite Image;
        public string SceneName;
        public AudioClip BGM;
        public bool IsManual = false;
        #endregion

        #region 難度選擇參數(自動)
        public int SoldierAmountAuto = 0;
        public float SoldierSpeedAuto = 0;
        public int SpikeTrapAmountAuto = 0;
        public float SpikeTrapIntervalAuto = 0;
        public int ShootTrapAmountAuto = 0;
        public float ShootTrapIntervalAuto = 0;
        #endregion

        #region 關卡進階設定
        public List<SoldierData> SoldierDataList;
        public List<SpikeTrapData> SpikeTrapDataList;
        public List<ShootTrapData> ShootTrapDataList;

        public PatrolPointsConfig SoldierPatrolPointsConfig;
        public PositionConfig SpikeTrapPositionConfig;
        public PositionConfig ShootTrapPositionConfig;
        #endregion
    }

    public enum Level 
    {
        Easy,
        Normal,
        Hard
    }
}