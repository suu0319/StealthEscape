using UnityEngine;
using UnityEditor;

public abstract class MyScriptableObjectEditor : Editor
{
    protected GameObject Obj;

    protected bool IsChoose = false;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        IsChoose = false;

        if (Obj != null) 
        {
            DestroyImmediate(Obj);
        }

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    protected abstract void OnSceneGUI(SceneView sv);
}