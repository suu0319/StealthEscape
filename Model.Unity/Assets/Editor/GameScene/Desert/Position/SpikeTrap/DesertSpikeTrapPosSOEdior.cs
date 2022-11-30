using UnityEngine;
using UnityEditor;
using Position;

[CustomEditor(typeof(DesertSpikeTrapPositionData))]
public class DesertSpikeTrapPosSOEdior : MyScriptableObjectEditor
{
    protected override void OnSceneGUI(SceneView sv)
    {
        DesertSpikeTrapPositionData mySO = (DesertSpikeTrapPositionData)target;

        if (mySO.Prefab != null)
        {
            Vector3 position = mySO.Position;
            Quaternion rotation = mySO.Rotation;

            if (!IsChoose)
            {
                IsChoose = true;
                Obj = Instantiate(mySO.Prefab, position, rotation);
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
            Debug.LogError("Prefab field is null");
        }
    }
}
