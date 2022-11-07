using UnityEngine;

public class CanvasDontDestroy : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化(DontDestroyGameObject)
    /// </summary>
    private void Init()
    {
        DontDestroyOnLoad(this);
    }
}