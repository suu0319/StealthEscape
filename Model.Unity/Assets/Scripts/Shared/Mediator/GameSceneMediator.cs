using UnityEngine;
using UnityEngine.SceneManagement;
using OptionMenu;
using GameOver;
using Manager;

namespace Mediator
{
    public abstract class GameSceneMediator : BaseMediator
    {
        [Header("SceneData")]
        [SerializeField]
        public string SceneName;

        [Header("Audio")]
        [SerializeField]
        protected AudioSource AudioSource;

        protected override void InitSingleton() { }

        /// <summary>
        /// 初始化遊戲場景資料
        /// </summary>
        protected void InitGameSceneData()
        {
            GameManager.Instance.GetStageIndex();
            GameManager.Instance.InitGameSceneData();

            AudioSource.clip = GameManager.Instance.GameSceneData.BGM;
            AudioSource.Play();
        }

        /// <summary>
        /// 初始化選項選單
        /// </summary>
        protected void InitOptionMenu()
        {
            OptionMenuController.Instance.AddOnClickListener();
        }

        /// <summary>
        /// 初始化死亡畫面(新增OnClick Event)
        /// </summary>
        protected void InitGameOverMenu()
        {
            GameOverController.Instance.AddOnClickListener();
        }

        /// <summary>
        /// 取得場景名稱
        /// </summary>
        protected void GetSceneIndex()
        {
            SceneName = SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// 顯示遊戲場景資料
        /// </summary>
        protected abstract void ShowGameSceneData();
    }
}