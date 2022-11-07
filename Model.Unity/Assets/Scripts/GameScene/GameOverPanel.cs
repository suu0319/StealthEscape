using UnityEngine;
using DG.Tweening;

namespace GameOver 
{
    public class GameOverPanel : MonoBehaviour
    {
        private static GameOverPanel _instance;
        public static GameOverPanel Instance
        {
            get
            {
                return _instance;
            }
        }

        [SerializeField]
        private CanvasGroup _canvasGroupBackground, _canvasGroupMenu;

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void Init()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
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