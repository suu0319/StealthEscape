using GamesTan.UI;
using UnityEngine;
using UnityEngine.UI;
using Mediator;
using Manager;

namespace GameStage
{
    public class GameStageItem : MonoBehaviour, IScrollCell
    {
        [SerializeField]
        private Image _btnImage;

        [SerializeField]
        private Button _btn;

        [SerializeField]
        private Text _title;

        /// <summary>
        /// 設定實例出來的元素GameObject
        /// </summary>
        /// <param name="data">元素資料</param>
        public void BindData(GameStageItemData data)
        {
            _btnImage.sprite = data.BtnImage;

            _title.text = data.Title.ToString();

            _btn.onClick.RemoveListener(StartSceneMediator.Instance.GameStageController.LoadGame);
            _btn.onClick.RemoveListener(StartSceneMediator.Instance.GameStagePanel.AppearDisabledGameMsg);

            if (data.Idx <= GameManager.Instance.PassLevelNum)
            {

                _btn.onClick.AddListener(StartSceneMediator.Instance.GameStageController.LoadGame);
            }
            else 
            {
                _btn.onClick.AddListener(StartSceneMediator.Instance.GameStagePanel.AppearDisabledGameMsg);
            }

            name = "GameStage " + data.Idx;
        }
    }
}