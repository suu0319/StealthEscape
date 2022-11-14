using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Enemy;
using Trap;
using Position;

namespace GameStage
{
	public class GameStageEditor : EditorWindow
	{
		[SerializeField]
		private GameStageConfig _gameStageConfig;

		GameStageData gameStageData;

		#region 全域相關
		private bool isCheat = false;
		private float playerSpeed = 6f;

		private const string soldierName = "Soldier";
		private const string spikeTrapName = "SpikeTrap";
		private const string shootTrapName = "ShootTrap";
		#endregion

		#region 關卡相關(基本)
		private Level stageLevel;
		private Sprite _stageImage;
		private string stageTitle;
		private int stageNum = 0;
		private AudioClip _stageBGM;

		private bool isManual = false;
		#endregion

		#region 關卡士兵敵人設定
		private List<SoldierData> _stageSoldierDataList;
		private bool stageSoldierAmountFoldout;
		private int stageSoldierAmount;
		#endregion

		#region 關卡地刺陷阱人設定
		private List<SpikeTrapData> _stageSpikeTrapDataList;
		private bool stageSpikeTrapAmountFoldout;
		private int stageSpikeTrapAmount;
		#endregion

		#region 關卡箭矢陷阱人設定
		private List<ShootTrapData> _stageShootTrapDataList;
		private bool stageShootTrapAmountFoldout;
		private int stageShootTrapAmount;
		#endregion

		#region 難度選擇參數(自動)
		private int stageSoldierAmountAuto;
		private float stageSoldierSpeedAuto;
		private int stageSpikeTrapAmountAuto;
		private float stageSpikeTrapIntervalAuto;
		private int stageShootTrapAmountAuto;
		private float stageShootTrapIntervalAuto;
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
			GlobalSettings();
			SyncConfig();
			BaseSettings();
			AdvancedSettings();
		}

		/// <summary>
		/// 同步Editor、Config
		/// </summary>
		private void SyncConfig()
		{
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

			#region Variable縮寫
			gameStageData = _gameStageConfig.StageDataList[stageNum];
			#endregion

			#region Editor、GameStageConfig資料同步
			stageLevel = gameStageData.Level;
			_stageImage = gameStageData.Image;
			stageTitle = gameStageData.SceneName;
			_stageBGM = gameStageData.BGM;

			_stageSoldierDataList = gameStageData.SoldierDataList;
			stageSoldierAmount = _stageSoldierDataList.Count;

			_stageSpikeTrapDataList = gameStageData.SpikeTrapDataList;
			stageSpikeTrapAmount = _stageSpikeTrapDataList.Count;

			_stageShootTrapDataList = gameStageData.ShootTrapDataList;
			stageShootTrapAmount = _stageShootTrapDataList.Count;


			stageSoldierAmountAuto = gameStageData.SoldierAmountAuto;
			stageSoldierSpeedAuto = gameStageData.SoldierSpeedAuto;
			stageSpikeTrapAmountAuto = gameStageData.SpikeTrapAmountAuto;
			stageSpikeTrapIntervalAuto = gameStageData.SpikeTrapIntervalAuto;
			stageShootTrapAmountAuto = gameStageData.ShootTrapAmountAuto;
			stageShootTrapIntervalAuto = gameStageData.ShootTrapIntervalAuto;

			#endregion

			_scrollPosContent = EditorGUILayout.BeginScrollView(_scrollPosContent);
		}

		/// <summary>
		/// 全域設定
		/// </summary>
		private void GlobalSettings()
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
		}

		/// <summary>
		/// 基本設定
		/// </summary>
		private void BaseSettings()
		{
			GUILayout.Label("Base Settings", EditorStyles.boldLabel);
			#region 關卡縮圖
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Image");
			gameStageData.Image = (Sprite)EditorGUILayout.ObjectField(_stageImage, typeof(Sprite), true);
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 關卡標題
			EditorGUILayout.BeginHorizontal();
			gameStageData.SceneName = EditorGUILayout.TextField("SceneName", stageTitle);
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 關卡音樂
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("BGM");
			gameStageData.BGM = (AudioClip)EditorGUILayout.ObjectField(_stageBGM, typeof(AudioClip), true);
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 手動開啟(進階設定)
			isManual = _gameStageConfig.StageDataList[stageNum].IsManual;
			_gameStageConfig.StageDataList[stageNum].IsManual = EditorGUILayout.Toggle("Manual", isManual);
			#endregion

			using (new EditorGUI.DisabledScope(isManual))
			{
				#region 關卡難度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.Level = (Level)EditorGUILayout.EnumPopup("Level", stageLevel);
				InitStageLevelAutoSettings();
				EditorGUILayout.EndHorizontal();
				#endregion

				EditorGUILayout.HelpBox("Random Create", MessageType.Warning);

				#region 關卡士兵敵人數量(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.SoldierAmountAuto = EditorGUILayout.IntField("Soldier Amount", stageSoldierAmountAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡士兵敵人速度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.SoldierSpeedAuto = EditorGUILayout.FloatField("Soldier Speed", stageSoldierSpeedAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡地刺陷阱數量(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.SpikeTrapAmountAuto = EditorGUILayout.IntField("SpikeTrap Amount", stageSpikeTrapAmountAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡地刺陷阱速度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.SpikeTrapIntervalAuto = EditorGUILayout.FloatField("SpikeTrap Interval", stageSpikeTrapIntervalAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡箭矢陷阱數量(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.ShootTrapAmountAuto = EditorGUILayout.IntField("ShootTrap Amount", stageShootTrapAmountAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡箭矢陷阱速度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				gameStageData.ShootTrapIntervalAuto = EditorGUILayout.FloatField("ShootTrap Interval", stageShootTrapIntervalAuto);
				EditorGUILayout.EndHorizontal();
				#endregion
			}
		}

		/// <summary>
		/// 進階設定
		/// </summary>
		private void AdvancedSettings()
		{
			using (new EditorGUI.DisabledScope(!isManual))
			{
				GUIStyle helpBoxStyle = GUI.skin.GetStyle("HelpBox");
				helpBoxStyle.fontSize = 15;
				helpBoxStyle.richText = true;

				GUILayout.Label("Advanced Settings", EditorStyles.boldLabel);

				GUILayout.Label("Soldier Enemy Settings", EditorStyles.largeLabel);
				#region 關卡士兵敵人設定
				EditorGUILayout.BeginHorizontal();
				stageSoldierAmount = EditorGUILayout.IntField("Soldier Amount", gameStageData.SoldierDataList.Count);

				RefreshEnemyTrapDataList("Soldier", stageSoldierAmount, gameStageData.SoldierDataList);

				EditorGUILayout.EndHorizontal();

				if (_stageSoldierDataList.Count > 0)
				{
					EditorGUILayout.BeginHorizontal();
					stageSoldierAmountFoldout = EditorGUILayout.Foldout(stageSoldierAmountFoldout, "SoldierSettings");
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.TextArea("Suggest Settings:\n<b>Speed: 2f</b>", helpBoxStyle);

					if (stageSoldierAmountFoldout)
					{
						for (int i = 0; i < _stageSoldierDataList.Count; i++)
						{
							EditorGUILayout.BeginHorizontal();
							gameStageData.SoldierDataList[i].Speed = EditorGUILayout.FloatField("Soldier " + (i + 1) + " Speed:", _stageSoldierDataList[i].Speed);
							gameStageData.SoldierDataList[i].PatrolPointsData = (PatrolPointsData)EditorGUILayout.ObjectField("\tPatrolPoints:", _stageSoldierDataList[i].PatrolPointsData, gameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList[0].GetType(), true);
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				#endregion
				GUILayout.Label(string.Empty);

				GUILayout.Label("SpikeTrap Settings", EditorStyles.largeLabel);
				#region 關卡地刺陷阱設定
				EditorGUILayout.BeginHorizontal();
				stageSpikeTrapAmount = EditorGUILayout.IntField("SpikeTrap Amount", gameStageData.SpikeTrapDataList.Count);

				RefreshEnemyTrapDataList("SpikeTrap", stageSpikeTrapAmount, gameStageData.SpikeTrapDataList);

				EditorGUILayout.EndHorizontal();

				if (_stageSpikeTrapDataList.Count > 0)
				{
					EditorGUILayout.BeginHorizontal();
					stageSpikeTrapAmountFoldout = EditorGUILayout.Foldout(stageSpikeTrapAmountFoldout, "SpikeTrapSetting");
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.TextArea("Suggest Settings:\n<b>Interval: 2f</b>", helpBoxStyle);

					if (stageSpikeTrapAmountFoldout)
					{
						for (int i = 0; i < _stageSpikeTrapDataList.Count; i++)
						{
							EditorGUILayout.BeginHorizontal();
							gameStageData.SpikeTrapDataList[i].Interval = EditorGUILayout.FloatField("SpikeTrap " + (i + 1) + " Interval:", _stageSpikeTrapDataList[i].Interval);
							gameStageData.SpikeTrapDataList[i].PositionData = (PositionData)EditorGUILayout.ObjectField("\tPosition:", _stageSpikeTrapDataList[i].PositionData, gameStageData.SpikeTrapPositionConfig.PositionDataList[0].GetType(), true);
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				#endregion
				GUILayout.Label(string.Empty);

				GUILayout.Label("ShootTrap Settings", EditorStyles.largeLabel);
				#region 關卡箭矢陷阱設定
				EditorGUILayout.BeginHorizontal();
				stageShootTrapAmount = EditorGUILayout.IntField("ShootTrap Amount", gameStageData.ShootTrapDataList.Count);

				RefreshEnemyTrapDataList("ShootTrap", stageShootTrapAmount, gameStageData.ShootTrapDataList);
				
				EditorGUILayout.EndHorizontal();

				if (_stageSpikeTrapDataList.Count > 0)
				{
					EditorGUILayout.BeginHorizontal();
					stageShootTrapAmountFoldout = EditorGUILayout.Foldout(stageShootTrapAmountFoldout, "ShootTrapSetting");
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.TextArea("Suggest Settings:\n<b>Interval: 2f</b>\n<b>Speed: 100f</b>", helpBoxStyle);

					if (stageShootTrapAmountFoldout)
					{
						for (int i = 0; i < _stageShootTrapDataList.Count; i++)
						{
							EditorGUILayout.BeginHorizontal();
							gameStageData.ShootTrapDataList[i].Interval = EditorGUILayout.FloatField("ShootTrap " + (i + 1) + " Interval:", _stageShootTrapDataList[i].Interval);
							gameStageData.ShootTrapDataList[i].Speed = EditorGUILayout.FloatField("\tSpeed:", _stageShootTrapDataList[i].Speed);
							gameStageData.ShootTrapDataList[i].PositionData = (PositionData)EditorGUILayout.ObjectField("\tPosition:", _stageShootTrapDataList[i].PositionData, gameStageData.ShootTrapPositionConfig.PositionDataList[0].GetType(), true);
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				#endregion
				GUILayout.Label(string.Empty);
			}

			EditorGUILayout.EndScrollView();
		}

		/// <summary>
		/// 刷新物件資料List
		/// </summary>
		/// <typeparam name="T">IEditorLayout</typeparam>
		/// <param name="name">物件名稱</param>
		/// <param name="amount">物件數量</param>
		/// <param name="objDataList">物件資料List</param>
		private void RefreshEnemyTrapDataList<T>(string name, int amount, List<T> objDataList) where T : IEditorLayout
		{
			if (objDataList.Count != amount)
			{
				if (objDataList.Count > amount)
				{
					for (int i = objDataList.Count; i > amount; i--)
					{
						objDataList.RemoveAt(objDataList.Count - 1);
					}
				}
				else if (objDataList.Count < amount)
				{
					for (int i = objDataList.Count; i < amount; i++)
					{
						switch (name)
						{
							case soldierName:
								gameStageData.SoldierDataList.Add(new SoldierData());
								break;

							case spikeTrapName:
								gameStageData.SpikeTrapDataList.Add(new SpikeTrapData());
								break;

							case shootTrapName:
								gameStageData.ShootTrapDataList.Add(new ShootTrapData());
								break;

							default:
								Debug.LogWarning("Soldier or Trap doesn't Exist");
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// 初始化關卡難度設定
		/// </summary>
		private void InitStageLevelAutoSettings()
		{
			switch (stageLevel)
			{
				case Level.Easy:
					stageSoldierAmountAuto = 10;
					stageSoldierSpeedAuto = 3f;
					stageSpikeTrapAmountAuto = 45;
					stageSpikeTrapIntervalAuto = 5f;
					stageShootTrapAmountAuto = 13;
					stageShootTrapIntervalAuto = 4f;
					break;

				case Level.Normal:
					stageSoldierAmountAuto = 13;
					stageSoldierSpeedAuto = 5f;
					stageSpikeTrapAmountAuto = 60;
					stageSpikeTrapIntervalAuto = 3f;
					stageShootTrapAmountAuto = 13;
					stageShootTrapIntervalAuto = 4f;
					break;

				case Level.Hard:
					stageSoldierAmountAuto = 16;
					stageSoldierSpeedAuto = 6f;
					stageSpikeTrapAmountAuto = 75;
					stageSpikeTrapIntervalAuto = 2f;
					stageShootTrapAmountAuto = 16;
					stageShootTrapIntervalAuto = 3f;
					break;
			}
		}
	}
}