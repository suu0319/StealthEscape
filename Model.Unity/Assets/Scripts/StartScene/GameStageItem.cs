using GamesTan.UI;
using UnityEngine;
using UnityEngine.UI;
using Mediator;
using Manager;

namespace GameStage
{
    public class GameStageItem : MonoBehaviour, IScrollCell
    {
        [Header("Item Image")]
        [SerializeField]
        private Image _btnImage;
        [SerializeField]
        private Color _defaultColor;
        [SerializeField]
        private Color _passColor;

        [Header("Item Button")]
        [SerializeField]
        private Button _btn;

        [Header("Item Text")]
        [SerializeField]
        private Text _title;

        /// <summary>
        /// 設定實例出來的元素GameObject
        /// </summary>
        /// <param name="data">元素資料</param>
        internal void BindData(GameStageItemData data)
        {
            _btnImage.sprite = data.BtnImage;

            _title.text = data.Title.ToString();

            _btn.onClick.RemoveListener(StartSceneMediator.Instance.GameStageController.LoadGame);
            _btn.onClick.RemoveListener(StartSceneMediator.Instance.GameStagePanel.AppearDisabledGameMsg);

            if (data.Idx <= GameManager.Instance.GameData.PassLevelNum)
            {
                _btnImage.color = _passColor;
                _btn.onClick.AddListener(StartSceneMediator.Instance.GameStageController.LoadGame);
            }
            else
            {
                _btnImage.color = _defaultColor;
                _btn.onClick.AddListener(StartSceneMediator.Instance.GameStagePanel.AppearDisabledGameMsg);
            }

            name = "GameStage " + data.Idx;
        }
    }
}