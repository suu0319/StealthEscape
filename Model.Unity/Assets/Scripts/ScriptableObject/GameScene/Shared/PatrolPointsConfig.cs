using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Position
{
    [CreateAssetMenu(fileName = "PatrolPointsConfig", menuName = "GameScene/CreatePatrolPointsConfig")]
    public class PatrolPointsConfig : ScriptableObject
    {
        public List<PatrolPointsData> PatrolPointsDataList;

        public bool IsComplete(ScriptableObject patrolPointsConfig)
        {
            var resultSame = PatrolPointsDataList.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(x => new { Element = x.GetType(), Count = x.Count() })
                            .ToList();

            var resultNull = from x in PatrolPointsDataList
                             where (x == null)
                             select x;

            if ((resultSame.Count > 0) || (resultNull.Count() > 0))
            {
                Debug.LogError(patrolPointsConfig.name + "'s dataList elements field have same or null");
                return false;
            }
            else
            {
                return true;
            }
        }

        public Type GetDataType(bool isComplete)
        {
            if (isComplete)
            {
                return PatrolPointsDataList[0].GetType();
            }
            else
            {
                return null;
            }
        }
    }
}