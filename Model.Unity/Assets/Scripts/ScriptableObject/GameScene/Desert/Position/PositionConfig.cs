using System.Collections.Generic;
using UnityEngine;

namespace Position
{
    [CreateAssetMenu(fileName = "PositionConfig", menuName = "GameScene/Desert/CreatePositionConfig")]
    public class PositionConfig : ScriptableObject
    {
        public List<PositionData> PositionDataList;
    }
}