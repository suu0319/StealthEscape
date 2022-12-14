using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Position
{
    [CreateAssetMenu(fileName = "PositionConfig", menuName = "GameScene/CreatePositionConfig")]
    public class PositionConfig : ScriptableObject
    {
        public List<PositionData> PositionDataList;

        public bool IsComplete(ScriptableObject positionConfig)
        {
            var resultSame = PositionDataList.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(x => new { Element = x.GetType(), Count = x.Count() })
                            .ToList();

            var resultNull = from x in PositionDataList
                             where (x == null)
                             select x;

            if ((resultSame.Count > 0) || (resultNull.Count() > 0))
            {
                Debug.LogError(positionConfig.name + "'s dataList elements field have same or null");
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
                return PositionDataList[0].GetType();
            }
            else 
            {
                return null;
            }
        }
    }
}