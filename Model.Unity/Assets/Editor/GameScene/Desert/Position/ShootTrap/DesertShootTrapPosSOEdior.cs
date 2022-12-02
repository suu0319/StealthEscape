using UnityEngine;
using UnityEditor;
using Position;

[CustomEditor(typeof(DesertShootTrapPositionData))]
public class DesertShootTrapPosSOEdior : MyScriptableObjectEditor
{
    protected override void OnSceneGUI(SceneView sv)
    {
        DesertShootTrapPositionData mySO = (DesertShootTrapPositionData)target;

        if (Obj != null)
        {
            Vector3 position = mySO.Position;
            Quaternion rotation = mySO.Rotation;

            if (!IsChoose)
            {
                IsChoose = true;
                Obj = Instantiate(Obj, position, rotation);
            }

            Obj.transform.localPosition = position = Handles.PositionHandle(position, rotation);
            Obj.transform.localRotation = rotation = Handles.RotationHandle(rotation, position);

            if (GUI.changed)
            {
                Undo.RecordObject(target, "Back to last change");

                Obj.transform.localPosition = mySO.Position = position;
                Obj.transform.localRotation = mySO.Rotation = rotation;
            }
        }
        else
        {
            Debug.LogError("Obj field is null");
        }
    }
}
