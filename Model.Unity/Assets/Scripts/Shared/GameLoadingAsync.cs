using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameLoading 
{
    public class GameLoadingAsync : MonoBehaviour
    {
        private static GameLoadingAsync _instance;
        public static GameLoadingAsync Instance 
        {
            get 
            {
                return _instance;
            }
        }

        [SerializeField]
        private Image _loadingImage;

        internal int LoadingPercent = 0;

        private float fadeInSecond = 2f;

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
        /// 載入場景
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        public void LoadGame(string sceneName) 
        {
            Debug.Log("載入場景:" + sceneName);

            _loadingImage.raycastTarget = true;
            GameLoadingPanel.Instance.LoadingFadeIn();
            StartCoroutine(LoadingScreenTimer(sceneName));
        }

        /// <summary>
        /// 讀取畫面(非同步)
        /// </summary>
        private IEnumerator LoadingScreenTimer(string sceneName) 
        {
            yield return new WaitForSeconds(fadeInSecond);

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.9f) 
            {
                LoadingPercent = (int)(async.progress * 100);
                GameLoadingPanel.Instance.LoadingPercent(LoadingPercent);
                yield return null;
            }

            LoadingPercent = 100;
            GameLoadingPanel.Instance.LoadingPercent(LoadingPercent);
            async.allowSceneActivation = true;
        }
    }
}