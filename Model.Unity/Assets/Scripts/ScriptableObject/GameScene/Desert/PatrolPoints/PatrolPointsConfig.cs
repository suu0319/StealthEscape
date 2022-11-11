using System.Collections.Generic;
using UnityEngine;

namespace Position
{
    [CreateAssetMenu(fileName = "PatrolPointsConfig", menuName = "GameScene/Desert/CreatePatrolPointsConfig")]
    public class PatrolPointsConfig : ScriptableObject
    {
        public List<PatrolPointsData> PatrolPointsDataList;
    }
}