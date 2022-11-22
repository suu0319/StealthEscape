using UnityEngine;
using DG.Tweening;

namespace GameOver
{
    public class GameOverPanel : MonoBehaviour
    {
        public static GameOverPanel Instance;

        [Header("CanvasGroup")]
        [SerializeField]
        private CanvasGroup _canvasGroupBackground;
        [SerializeField]
        private CanvasGroup _canvasGroupMenu;

        private void Awake()
        {
            InitSingleton();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 出現GameOver畫面
        /// </summary>
        internal void AppearGameOverScreen()
        {
            GameOverController.Instance.GameOverPanel.SetActive(true);

            var time = 0;
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_canvasGroupBackground.DOFade(1, 5f).SetEase(Ease.Linear));
            sequence.Append(DOTween.To(() => time, x => time = x, 1, 2f));
            sequence.Append(_canvasGroupMenu.DOFade(1, 3f).SetEase(Ease.Linear));
        }
    }
}