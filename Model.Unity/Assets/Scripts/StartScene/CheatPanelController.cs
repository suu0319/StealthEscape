using UnityEngine;
using UnityEngine.UI;

public class CheatPanelController : MonoBehaviour
{
    public static bool IsCheat = false;

    [Header("Toggle")]
    [SerializeField]
    private Toggle cheatTogle;

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        cheatTogle.isOn = IsCheat;
    }

    /// <sumary>
    /// 判斷作弊
    /// </summary>
    public void DetectCheat()
    {
        IsCheat = cheatTogle.isOn;
    }
}