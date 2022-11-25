using UnityEngine;
using UnityEditor;
using Position;

namespace SyncData
{
    [CustomEditor(typeof(SyncScriptableObject))]
    [CanEditMultipleObjects]
    public class ScriptableObjectEditor : Editor
    {
        private void Awake()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            DrawInspector();
        }

        private void OnSceneGUI()
        {
            DrawHandles();
        }

        /// <summary>
        /// 初始化(剛點擊)
        /// </summary>
        private void Init()
        {
            SyncScriptableObject syncScriptableObject = (SyncScriptableObject)target;

            if (syncScriptableObject.PositionData != null)
            {
                syncScriptableObject.transform.localPosition = syncScriptableObject.PositionData.Position;
                syncScriptableObject.transform.localRotation = syncScriptableObject.PositionData.Rotation;
            }
        }

        /// <summary>
        /// 繪製Inspector
        /// </summary>
        private void DrawInspector()
        {
            DrawDefaultInspector();

            SyncScriptableObject syncScriptableObject = (SyncScriptableObject)target;

            syncScriptableObject.IsShowEnemySettings = EditorGUILayout.Foldout(syncScriptableObject.IsShowEnemySettings, "EnemySettings");

            if (syncScriptableObject.IsShowEnemySettings)
            {
                syncScriptableObject.PatrolPointsData = (PatrolPointsData)EditorGUILayout.ObjectField(syncScriptableObject.PatrolPointsData, typeof(PatrolPointsData), true);

                if (syncScriptableObject.PatrolPointsData != null)
                {
                    EditorGUILayout.HelpBox("Enemy PatrolPoints Array", MessageType.Info);

                    syncScriptableObject.PatrolPosition = new Vector3[syncScriptableObject.PatrolPointsData.Position.Length];

                    for (int i = 0; i < syncScriptableObject.PatrolPosition.Length; i++)
                    {
                        syncScriptableObject.PatrolPosition[i] = syncScriptableObject.PatrolPointsData.Position[i];
                        syncScriptableObject.PatrolPointsData.Position[i] = EditorGUILayout.Vector3Field("PatrolPoinrs", syncScriptableObject.PatrolPosition[i]);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("PatrolPointsData field Is null", MessageType.Error);
                }
            }

            syncScriptableObject.IsShowTrapSettings = EditorGUILayout.Foldout(syncScriptableObject.IsShowTrapSettings, "TrapSettings");

            if (syncScriptableObject.IsShowTrapSettings)
            {
                EditorGUI.BeginChangeCheck();
                syncScriptableObject.PositionData = (PositionData)EditorGUILayout.ObjectField(syncScriptableObject.PositionData, typeof(PositionData), true);

                if (EditorGUI.EndChangeCheck())
                {
                    if (syncScriptableObject.PositionData != null)
                    {
                        syncScriptableObject.transform.localPosition = syncScriptableObject.PositionData.Position;
                        syncScriptableObject.transform.localRotation = syncScriptableObject.PositionData.Rotation;
                    }
                    else
                    {
                        Debug.LogError("PositionData field is null");
                    }
                }

                if (syncScriptableObject.PositionData != null)
                {
                    EditorGUILayout.HelpBox("Trap Transform", MessageType.Info);

                    syncScriptableObject.Position = syncScriptableObject.transform.localPosition;
                    syncScriptableObject.transform.localPosition = EditorGUILayout.Vector3Field("Position", syncScriptableObject.Position);
                    syncScriptableObject.PositionData.Position = syncScriptableObject.transform.localPosition;

                    syncScriptableObject.Rotation = syncScriptableObject.transform.localRotation;
                    syncScriptableObject.RotationVector3 = syncScriptableObject.Rotation.eulerAngles;
                    syncScriptableObject.RotationVector3 = EditorGUILayout.Vector3Field("Rotation", syncScriptableObject.RotationVector3);
                    syncScriptableObject.PositionData.Rotation = syncScriptableObject.transform.rotation = Quaternion.Euler(syncScriptableObject.RotationVector3.x, syncScriptableObject.RotationVector3.y, syncScriptableObject.RotationVector3.z);

                    syncScriptableObject.Scale = syncScriptableObject.transform.localScale;
                    EditorGUILayout.Vector3Field("Scale", syncScriptableObject.Scale);

                }
                else
                {
                    EditorGUILayout.HelpBox("PositionData field Is null", MessageType.Error);
                }
            }
        }

        /// <summary>
        /// 繪製Handles(SceneView)
        /// </summary>
        private void DrawHandles()
        {
            SyncScriptableObject syncScriptableObject = (SyncScriptableObject)target;

            if (syncScriptableObject.PositionData != null)
            {
                Handles.matrix = syncScriptableObject.transform.localToWorldMatrix;
                Handles.color = Color.red;
                Handles.DrawWireCube(Vector3.zero, syncScriptableObject.Scale);
            }

            if (syncScriptableObject.PatrolPointsData != null)
            {
                Handles.matrix = Matrix4x4.identity;
                Handles.color = Color.yellow;

                for (int i = 0; i < syncScriptableObject.PatrolPosition.Length; i++)
                {
                    Handles.DrawWireCube(syncScriptableObject.PatrolPosition[i], syncScriptableObject.Scale);
                }
            }
        }
    }
}