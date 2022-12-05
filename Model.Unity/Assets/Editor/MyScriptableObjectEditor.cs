using UnityEngine;
using UnityEditor;

public abstract class MyScriptableObjectEditor : Editor
{
    [SerializeField]
    protected GameObject Obj;
    protected GameObject[] ObjArray;

    protected bool IsChoose = false;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        IsChoose = false;

        if (ObjArray != null)
        {
            for (int i = 0; i < ObjArray.Length; i++)
            {
                DestroyImmediate(ObjArray[i]);
            }
        }

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    protected abstract void OnSceneGUI(SceneView sv);
}