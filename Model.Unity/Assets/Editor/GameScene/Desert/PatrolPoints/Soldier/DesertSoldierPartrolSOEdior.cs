using UnityEngine;
using UnityEditor;
using Position;

[CustomEditor(typeof(DesertSoldierPatrolPointsData))]
public class DesertSoldierPartrolSOEdior : MyScriptableObjectEditor
{
    protected override void OnSceneGUI(SceneView sv)
    {
        DesertSoldierPatrolPointsData mySO = (DesertSoldierPatrolPointsData)target;

        Vector3[] patrolPoints = mySO.Position;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = Handles.PositionHandle(patrolPoints[i], Quaternion.identity);
            
            Handles.color = Color.yellow;
            Handles.DrawWireCube(patrolPoints[i], Vector3.one);
        }

        if (GUI.changed)
        {
            Undo.RegisterCompleteObjectUndo(target, "Back to last change");

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                mySO.Position[i] = patrolPoints[i];
            }
        }
    }
}