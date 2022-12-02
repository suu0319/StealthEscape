using System;
using System.Collections.Generic;
using UnityEngine;

namespace Position
{
    [CreateAssetMenu(fileName = "PatrolPointsConfig", menuName = "GameScene/CreatePatrolPointsConfig")]
    public class PatrolPointsConfig : ScriptableObject
    {
        public List<PatrolPointsData> PatrolPointsDataList;

        public Type GetDataType()
        {
            return PatrolPointsDataList[0].GetType();
        }
    }
}