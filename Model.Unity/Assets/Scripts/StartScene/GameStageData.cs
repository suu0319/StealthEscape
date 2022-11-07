using UnityEngine;

namespace GameStage 
{
    [System.Serializable]
    public class GameStageData
    {
        public Level Level;
        public Sprite Image;
        public string Title;
        public AudioClip BGM;

        public int SoldierEnemyAmount = 16;
        public float SoldierEnemySpeed = 6f;
        public int SpikeTrapAmount = 11;
        public float SpikeTrapInterval = 2f;
        public int ShootTrapAmount = 16;
        public float ShootTrapInterval = 3f;
    }

    public enum Level 
    {
        Easy,
        Normal,
        Hard
    }
}