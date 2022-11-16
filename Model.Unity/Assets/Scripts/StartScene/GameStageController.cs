using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameLoading;
using Mediator;
using Manager;

namespace GameStage
{
    public class GameStageController : MonoBehaviour
    {
        /// <summary>
        /// 載入關卡場景
        /// </summary>
        internal void LoadGame()
        {
           if (StartSceneMediator.Instance.GameStagePanel.CanvasGroup.alpha == 0f)
            {
                GameLoadingPanel.Instance.gameObject.SetActive(true);
                string sceneName = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name;
                GameLoadingAsync.Instance.LoadGame(sceneName);
                GameStateController.Instance.SwitchGameSceneState();
            }
        }
    }
}