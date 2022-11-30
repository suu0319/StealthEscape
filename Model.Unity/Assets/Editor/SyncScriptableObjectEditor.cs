using UnityEngine;
using UnityEditor;
using Position;

namespace SyncData
{
    [CustomEditor(typeof(SyncScriptableObject))]
    [CanEditMultipleObjects]
    public class SyncScriptableObjectEditor : Editor
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
            SyncScriptableObject myTarget = (SyncScriptableObject)target;

            if (myTarget.PositionData != null)
            {
                myTarget.transform.localPosition = myTarget.PositionData.Position;
                myTarget.transform.localRotation = myTarget.PositionData.Rotation;
            }
        }

        /// <summary>
        /// 繪製Inspector
        /// </summary>
        private void DrawInspector()
        {
            DrawDefaultInspector();

            SyncScriptableObject myTarget = (SyncScriptableObject)target;

            myTarget.IsShowEnemySettings = EditorGUILayout.Foldout(myTarget.IsShowEnemySettings, "EnemySettings");

            if (myTarget.IsShowEnemySettings)
            {
                myTarget.PatrolPointsData = (PatrolPointsData)EditorGUILayout.ObjectField(myTarget.PatrolPointsData, typeof(PatrolPointsData), true);

                if (myTarget.PatrolPointsData != null)
                {
                    EditorGUILayout.HelpBox("Enemy PatrolPoints Array", MessageType.Info);

                    myTarget.PatrolPosition = new Vector3[myTarget.PatrolPointsData.Position.Length];

                    for (int i = 0; i < myTarget.PatrolPosition.Length; i++)
                    {
                        myTarget.PatrolPosition[i] = myTarget.PatrolPointsData.Position[i];
                        myTarget.PatrolPointsData.Position[i] = EditorGUILayout.Vector3Field("PatrolPoinrs", myTarget.PatrolPosition[i]);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("PatrolPointsData field Is null", MessageType.Error);
                }
            }

            myTarget.IsShowTrapSettings = EditorGUILayout.Foldout(myTarget.IsShowTrapSettings, "TrapSettings");

            if (myTarget.IsShowTrapSettings)
            {
                EditorGUI.BeginChangeCheck();
                myTarget.PositionData = (PositionData)EditorGUILayout.ObjectField(myTarget.PositionData, typeof(PositionData), true);

                if (EditorGUI.EndChangeCheck())
                {
                    if (myTarget.PositionData != null)
                    {
                        myTarget.transform.localPosition = myTarget.PositionData.Position;
                        myTarget.transform.localRotation = myTarget.PositionData.Rotation;
                        Debug.Log(myTarget.transform.localRotation.eulerAngles);
                    }
                    else
                    {
                        Debug.LogError("PositionData field is null");
                    }
                }

                if (myTarget.PositionData != null)
                {
                    EditorGUILayout.HelpBox("Trap Transform", MessageType.Info);

                    myTarget.Position = myTarget.transform.localPosition;
                    myTarget.transform.localPosition = EditorGUILayout.Vector3Field("Position", myTarget.Position);
                    myTarget.PositionData.Position = myTarget.transform.localPosition;

                    myTarget.Rotation = myTarget.transform.localRotation;
                    myTarget.RotationEuler = myTarget.Rotation.eulerAngles;
                    myTarget.RotationEuler = EditorGUILayout.Vector3Field("Rotation", myTarget.RotationEuler);
                    myTarget.PositionData.Rotation = myTarget.transform.localRotation = Quaternion.Euler(myTarget.RotationEuler);

                    myTarget.Scale = myTarget.transform.localScale;
                    EditorGUILayout.Vector3Field("Scale", myTarget.Scale);

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
            SyncScriptableObject myTarget = (SyncScriptableObject)target;

            if (myTarget.PositionData != null)
            {
                Handles.matrix = myTarget.transform.localToWorldMatrix;
                Handles.color = Color.red;
                Handles.DrawWireCube(Vector3.zero, myTarget.Scale);
            }

            if (myTarget.PatrolPointsData != null)
            {
                Handles.matrix = Matrix4x4.identity;
                Handles.color = Color.yellow;

                for (int i = 0; i < myTarget.PatrolPosition.Length; i++)
                {
                    Handles.DrawWireCube(myTarget.PatrolPosition[i], myTarget.Scale);
                }
            }
        }
    }
}