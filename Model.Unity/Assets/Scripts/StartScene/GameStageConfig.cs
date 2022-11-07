using System.Collections.Generic;
using UnityEngine;

namespace GameStage 
{
    [CreateAssetMenu(fileName = "GameStageConfig", menuName = "CreateGameStageConfig")]
    public class GameStageConfig : ScriptableObject
    {
        public bool IsCheat = false;
        public float PlayerSpeed = 6f;

        [SerializeField]
        public List<GameStageData> StageDataList;
    }
}