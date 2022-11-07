using UnityEngine;
using UnityEditor;

namespace GameStage
{
    public class GameStageEditor : EditorWindow
    {
        [SerializeField]
        private GameStageConfig _gameStageConfig;

        #region 全域相關
        private bool isCheat = false;
        private float playerSpeed = 6f;
        #endregion

        #region 關卡相關(基本)
        private Level stageLevel;
        private Sprite _stageImage;
        private string stageTitle;
        private int stageNum = 0;
        private AudioClip _stageBGM;
        #endregion

        #region 關卡相關(進階)
        private int stageSoldierEnemyAmount;
        private float stageSoldierEnemySpeed;
        private int stageSpikeTrapAmount;
        private float stageSpikeTrapInterval;
        private int stageShootTrapAmount;
        private float stageShootTrapInterval;
        #endregion

        #region 關卡UGUI(ScrollView)
        private Vector2 _scrollPosBtn;
        private Vector2 _scrollPosContent;
        #endregion

        /// <summary>
        /// 創建編輯器窗口
        /// </summary>
        [MenuItem("Window/GameStageEditor")]
        private static void ShowGameStageEditor()
        {
            Debug.Log("開啟GameStageEditor");

            EditorWindow.GetWindow(typeof(GameStageEditor), true, "GameStageEditor", true);
        }

        private void OnGUI()
        {
            DrawGameStageEditor();
        }

        /// <summary>
        /// 繪製編輯器
        /// </summary>
        private void DrawGameStageEditor() 
        {
            GUILayout.Label("Global Settings", EditorStyles.boldLabel);
            #region 作弊
            isCheat = _gameStageConfig.IsCheat;
            _gameStageConfig.IsCheat = EditorGUILayout.Toggle("IsCheat", isCheat);
            #endregion

            #region 玩家速度
            playerSpeed = _gameStageConfig.PlayerSpeed;
            _gameStageConfig.PlayerSpeed = EditorGUILayout.FloatField("PlayerSpeed", playerSpeed);
            #endregion

            EditorGUILayout.BeginHorizontal();

            _scrollPosBtn = EditorGUILayout.BeginScrollView(_scrollPosBtn, GUILayout.Width(250));

            #region 關卡列表Button
            for (int i = 0; i < _gameStageConfig.StageDataList.Count; i++)
            {
                if (GUILayout.Button("Stage" + (i + 1)))
                {
                    stageNum = i;
                }
            }
            #endregion

            EditorGUILayout.EndScrollView();

            #region Editor、GameStageConfig資料同步
            stageLevel = _gameStageConfig.StageDataList[stageNum].Level;
            _stageImage = _gameStageConfig.StageDataList[stageNum].Image;
            stageTitle = _gameStageConfig.StageDataList[stageNum].Title;
            _stageBGM = _gameStageConfig.StageDataList[stageNum].BGM;

            stageSoldierEnemyAmount = _gameStageConfig.StageDataList[stageNum].SoldierEnemyAmount;
            stageSoldierEnemySpeed = _gameStageConfig.StageDataList[stageNum].SoldierEnemySpeed;
            stageSpikeTrapAmount = _gameStageConfig.StageDataList[stageNum].SpikeTrapAmount;
            stageSpikeTrapInterval = _gameStageConfig.StageDataList[stageNum].SpikeTrapInterval;
            stageShootTrapAmount = _gameStageConfig.StageDataList[stageNum].ShootTrapAmount;
            stageShootTrapInterval = _gameStageConfig.StageDataList[stageNum].ShootTrapInterval;
            #endregion

            _scrollPosContent = EditorGUILayout.BeginScrollView(_scrollPosContent);

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            #region 關卡難度
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].Level = (Level)EditorGUILayout.EnumPopup("Level", stageLevel);
            InitStageLevelSettings();
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡縮圖
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Image");
            _gameStageConfig.StageDataList[stageNum].Image = (Sprite)EditorGUILayout.ObjectField(_stageImage, typeof(Sprite), true);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡標題
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].Title = EditorGUILayout.TextField("Title", stageTitle);        
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡音樂
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("BGM");
            _gameStageConfig.StageDataList[stageNum].BGM = (AudioClip)EditorGUILayout.ObjectField(_stageBGM, typeof(AudioClip), true);
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Label("Advanced Settings", EditorStyles.boldLabel);

            #region 關卡士兵敵人數量
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].SoldierEnemyAmount = EditorGUILayout.IntField("SoldierEnemyAmount", stageSoldierEnemyAmount);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡士兵敵人速度
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].SoldierEnemySpeed = EditorGUILayout.FloatField("SoldierEnemySpeed", stageSoldierEnemySpeed);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡地刺陷阱數量
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].SpikeTrapAmount = EditorGUILayout.IntField("SpikeTrapAmount", stageSpikeTrapAmount);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡地刺陷阱速度
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].SpikeTrapInterval = EditorGUILayout.FloatField("SpikeTrapSpeed", stageSpikeTrapInterval);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡箭矢陷阱數量
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].ShootTrapAmount = EditorGUILayout.IntField("ShootTrapAmount", stageShootTrapAmount);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 關卡箭矢陷阱速度
            EditorGUILayout.BeginHorizontal();
            _gameStageConfig.StageDataList[stageNum].ShootTrapInterval = EditorGUILayout.FloatField("ShootTrapSpeed", stageShootTrapInterval);
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 初始化關卡難度設定
        /// </summary>
        private void InitStageLevelSettings()
        {
            switch (stageLevel) 
            {
                case Level.Easy:
                    stageSoldierEnemyAmount = 10;
                    stageSoldierEnemySpeed = 3f;
                    stageSpikeTrapAmount = 6;
                    stageSpikeTrapInterval = 5f;
                    stageShootTrapAmount = 13;
                    stageShootTrapInterval = 4f;
                    break;

                case Level.Normal:
                    stageSoldierEnemyAmount = 13;
                    stageSoldierEnemySpeed = 5f;
                    stageSpikeTrapAmount = 8;
                    stageSpikeTrapInterval = 3f;
                    stageShootTrapAmount = 13;
                    stageShootTrapInterval = 4f;
                    break;

                case Level.Hard:
                    stageSoldierEnemyAmount = 16;
                    stageSoldierEnemySpeed = 6f;
                    stageSpikeTrapAmount = 11;
                    stageSpikeTrapInterval = 2f;
                    stageShootTrapAmount = 16;
                    stageShootTrapInterval = 3f;
                    break;
            }           
        }
    }
}