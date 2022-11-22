using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameLoading
{
    public class GameLoadingPanel : MonoBehaviour
    {
        public static GameLoadingPanel Instance;

        [Header("Loading Text")]
        [SerializeField]
        private Text _loadingText;

        [Header("CanvasGroup")]
        [SerializeField]
        private CanvasGroup _canvasGroup;

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
        /// 讀取畫面趴數
        /// </summary>
        internal void LoadingPercent(int loadingPercent)
        {
            _loadingText.text = loadingPercent.ToString() + "%";
        }

        /// <summary>
        /// 畫面淡出
        /// </summary>
        internal void ScreenFadeOut()
        {
            _loadingText.text = string.Empty;
            _canvasGroup.alpha = 1f;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, 2f).SetEase(Ease.Linear).onComplete += Disable;
        }

        /// <summary>
        /// 讀取畫面淡入
        /// </summary>
        internal void ScreenFadeIn()
        {
            _canvasGroup.alpha = 0f;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, 2f).SetEase(Ease.Linear);
        }

        /// <summary>
        /// DeActivate讀取畫面物件
        /// </summary>
        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}