using UnityEngine;
using UnityEditor;
using Position;

[CustomEditor(typeof(DesertSoldierPatrolPointsData))]
[CanEditMultipleObjects]
public class DesertSoldierPartrolSOEdior : MyScriptableObjectEditor
{
    protected override void OnSceneGUI(SceneView sv)
    {
        for (int i = 0; i < targets.Length; i++) 
        {
            DesertSoldierPatrolPointsData mySO = (DesertSoldierPatrolPointsData)targets[i];

            Vector3[] patrolPoints = mySO.Position;

            for (int y = 0; y < patrolPoints.Length; y++)
            {
                patrolPoints[y] = Handles.PositionHandle(patrolPoints[y], Quaternion.identity);

                Handles.color = Color.yellow;
                Handles.DrawWireCube(patrolPoints[y], Vector3.one);
            }

            if (GUI.changed)
            {
                Undo.RegisterCompleteObjectUndo(target, "Back to last change");

                for (int y = 0; y < patrolPoints.Length; y++)
                {
                    mySO.Position[y] = patrolPoints[y];
                }
            }
        }
    }
}