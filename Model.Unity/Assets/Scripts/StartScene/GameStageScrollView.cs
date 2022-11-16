using System.Collections.Generic;
using GamesTan.UI;
using UnityEngine;
using Mediator;

namespace GameStage
{
    public class GameStageItemData
    {
        internal int Idx;
        internal Sprite BtnImage;
        internal string Title;
    }

    public class GameStageScrollView : MonoBehaviour, ISuperScrollRectDataProvider
    {
        [SerializeField]
        private SuperScrollRect _scrollRect;

        private List<GameStageItemData> _datas = new List<GameStageItemData>();

        private void OnEnable()
        {
            InitScrollView();
        }

        /// <summary>
        /// 初始化ScrollView資料
        /// </summary>
        private void InitScrollView()
        {
            List<GameStageData> gameStageDataList = StartSceneMediator.Instance.GameStageConfig.StageDataList;

            for (int i = 0; i < gameStageDataList.Count; i++)
            {
                _datas.Add(new GameStageItemData()
                {
                    Idx = i,
                    BtnImage = gameStageDataList[i].Image,
                    Title = gameStageDataList[i].SceneName
                });
            }

            _scrollRect.DoAwake(this);
        }

        /// <summary>
        /// 取得元素數量
        /// </summary>
        /// <returns></returns>
        public int GetCellCount()
        {
            return _datas.Count;
        }

        /// <summary>
        /// 設定元素資料
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="index"></param>
        public void SetCell(GameObject cell, int index)
        {
            var item = cell.GetComponent<GameStageItem>();
            item.BindData(_datas[index]);
        }
    }
}