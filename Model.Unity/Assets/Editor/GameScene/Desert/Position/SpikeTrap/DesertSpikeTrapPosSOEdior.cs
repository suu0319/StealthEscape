using UnityEngine;
using UnityEditor;
using Position;

[CustomEditor(typeof(DesertSpikeTrapPositionData))]
[CanEditMultipleObjects]
public class DesertSpikeTrapPosSOEdior : MyScriptableObjectEditor
{
    protected override void OnSceneGUI(SceneView sv)
    {
        if (Obj != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                DesertSpikeTrapPositionData mySO = (DesertSpikeTrapPositionData)targets[i];

                Vector3 position = mySO.Position;
                Quaternion rotation = mySO.Rotation;

                if (!IsChoose)
                {
                    IsChoose = true;

                    ObjArray = new GameObject[targets.Length];

                    for (int y = 0; y < targets.Length; y++)
                    {
                        ObjArray[y] = Instantiate(Obj, position, rotation);
                    }
                }

                ObjArray[i].transform.localPosition = position = Handles.PositionHandle(position, rotation);
                ObjArray[i].transform.localRotation = rotation = Handles.RotationHandle(rotation, position);

                if (GUI.changed)
                {
                    Undo.RegisterCompleteObjectUndo(targets, "Back to last change");

                    ObjArray[i].transform.localPosition = mySO.Position = position;
                    ObjArray[i].transform.localRotation = mySO.Rotation = rotation;
                }
            }
        }
        else
        {
            Debug.LogError("Obj field is null");
        }
    }
}
