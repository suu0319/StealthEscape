using System;
using System.Collections.Generic;
using UnityEngine;

namespace Position
{
    [CreateAssetMenu(fileName = "PositionConfig", menuName = "GameScene/CreatePositionConfig")]
    public class PositionConfig : ScriptableObject
    {
        public List<PositionData> PositionDataList;

        public Type GetDataType()
        {
            return PositionDataList[0].GetType();
        }
    }
}