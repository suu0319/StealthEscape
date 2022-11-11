using UnityEngine;
using GameLoading;

namespace Mediator 
{
    public abstract class BaseMediator : MonoBehaviour
    {
        /// <summary>
        /// Singleton初始化
        /// </summary>
        protected abstract void InitSingleton();

        /// <summary>
        /// 畫面淡出
        /// </summary>
        protected virtual void ScreenFadeOut()
        {
            GameLoadingPanel.Instance.ScreenFadeOut();
        }
    }
}