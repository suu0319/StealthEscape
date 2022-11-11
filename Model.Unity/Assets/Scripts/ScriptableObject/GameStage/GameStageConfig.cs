using System.Collections.Generic;
using UnityEngine;

namespace GameStage 
{
    [CreateAssetMenu(fileName = "GameStageConfig", menuName = "StartScene/CreateGameStageConfig")]
    public class GameStageConfig : ScriptableObject
    {
        public bool IsCheat = false;
        public float PlayerSpeed = 6f;

        public List<GameStageData> StageDataList;
    }
}