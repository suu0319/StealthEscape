using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameLoading
{
    public class GameLoadingPanel : MonoBehaviour
    {
        private static GameLoadingPanel _instance;
        public static GameLoadingPanel Instance 
        {
            get 
            {
                return _instance;
            }
        }

        [SerializeField]
        private Text _loadingText;

        [SerializeField]
        private CanvasGroup _canvasGroup;

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
        internal void LoadingFadeIn()
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