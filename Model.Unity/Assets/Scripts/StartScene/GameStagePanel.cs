using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameStagePanel : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private Text _failLoadMsg;

    [Header("CanvasGroup")]
    [SerializeField]
    internal CanvasGroup CanvasGroup;

    /// <summary>
    /// 出現禁用的遊戲關卡訊息
    /// </summary>
    internal void AppearDisabledGameMsg()
    {
        if (CanvasGroup.alpha == 0f)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(CanvasGroup.DOFade(1, 1f).SetEase(Ease.Linear));
            sequence.Append(CanvasGroup.DOFade(0, 1f).SetEase(Ease.Linear));
        }
    }
}