using UnityEngine;
using Position;

namespace SyncData
{
    public class SyncScriptableObject : MonoBehaviour
    {
        [HideInInspector]
        public PatrolPointsData PatrolPointsData;
        [HideInInspector]
        public PositionData PositionData;

        #region PatrolPointsData相關
        [HideInInspector]
        public Vector3[] PatrolPosition;
        #endregion

        #region PositionData相關
        [HideInInspector]
        public Vector3 Position;
        [HideInInspector]
        public Vector3 RotationVector3;
        [HideInInspector]
        public Vector3 Scale;
        [HideInInspector]
        public Quaternion Rotation;
        #endregion

        #region Foldout相關
        [HideInInspector]
        public bool IsShowEnemySettings = false;
        [HideInInspector]
        public bool IsShowTrapSettings = false;
        #endregion
    }
}