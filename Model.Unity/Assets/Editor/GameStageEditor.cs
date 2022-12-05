using System;
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
		private GameStageData _gameStageData;

		#region 全域相關
		private bool isCheat = false;
		private float playerSpeed = 6f;
		#endregion

		#region 關卡相關(基本)
		private Level stageLevel;
		private Sprite _stageImage;
		private string stageSceneName;
		private int stageNum = 0;
		private AudioClip _stageBGM;

		private bool isManual = false;
		#endregion

		#region 關卡士兵敵人設定
		private List<SoldierData> _stageSoldierDataList;
		private bool isShowStageSoldierAmountFoldout;
		private int stageSoldierAmount;
		#endregion

		#region 關卡地刺陷阱人設定
		private List<SpikeTrapData> _stageSpikeTrapDataList;
		private bool isShowStageSpikeTrapAmountFoldout;
		private int stageSpikeTrapAmount;
		#endregion

		#region 關卡箭矢陷阱人設定
		private List<ShootTrapData> _stageShootTrapDataList;
		private bool isShowStageShootTrapAmountFoldout;
		private int stageShootTrapAmount;
		#endregion

		#region 難度選擇參數(自動)
		internal int stageSoldierAmountAuto;
		internal float stageSoldierSpeedAuto;
		internal int stageSpikeTrapAmountAuto;
		internal float stageSpikeTrapIntervalAuto;
		internal int stageShootTrapAmountAuto;
		internal float stageShootTrapIntervalAuto;
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

			GetWindow(typeof(GameStageEditor), true, "GameStageEditor", true);
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
			_gameStageData = _gameStageConfig.StageDataList[stageNum];
			#endregion

			#region Editor、GameStageConfig資料同步
			stageLevel = _gameStageData.Level;
			_stageImage = _gameStageData.Image;
			stageSceneName = _gameStageData.SceneName;
			_stageBGM = _gameStageData.BGM;

			_stageSoldierDataList = _gameStageData.SoldierDataList;
			stageSoldierAmount = _stageSoldierDataList.Count;

			_stageSpikeTrapDataList = _gameStageData.SpikeTrapDataList;
			stageSpikeTrapAmount = _stageSpikeTrapDataList.Count;

			_stageShootTrapDataList = _gameStageData.ShootTrapDataList;
			stageShootTrapAmount = _stageShootTrapDataList.Count;


			stageSoldierAmountAuto = _gameStageData.SoldierAmountAuto;
			stageSoldierSpeedAuto = _gameStageData.SoldierSpeedAuto;
			stageSpikeTrapAmountAuto = _gameStageData.SpikeTrapAmountAuto;
			stageSpikeTrapIntervalAuto = _gameStageData.SpikeTrapIntervalAuto;
			stageShootTrapAmountAuto = _gameStageData.ShootTrapAmountAuto;
			stageShootTrapIntervalAuto = _gameStageData.ShootTrapIntervalAuto;

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
			_gameStageData.Image = (Sprite)EditorGUILayout.ObjectField(_stageImage, typeof(Sprite), true);
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 關卡標題
			EditorGUILayout.BeginHorizontal();
			_gameStageData.SceneName = EditorGUILayout.TextField("SceneName", stageSceneName);
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 關卡音樂
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("BGM");
			_gameStageData.BGM = (AudioClip)EditorGUILayout.ObjectField(_stageBGM, typeof(AudioClip), true);
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
				_gameStageData.Level = (Level)EditorGUILayout.EnumPopup("Level", stageLevel);
				InitStageLevelAutoSettings();
				EditorGUILayout.EndHorizontal();
				#endregion

				EditorGUILayout.HelpBox("Random Create", MessageType.Warning);

				#region 關卡士兵敵人數量(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				_gameStageData.SoldierAmountAuto = EditorGUILayout.IntField("Soldier Amount", stageSoldierAmountAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡士兵敵人速度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				_gameStageData.SoldierSpeedAuto = EditorGUILayout.FloatField("Soldier Speed", stageSoldierSpeedAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡地刺陷阱數量(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				_gameStageData.SpikeTrapAmountAuto = EditorGUILayout.IntField("SpikeTrap Amount", stageSpikeTrapAmountAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡地刺陷阱速度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				_gameStageData.SpikeTrapIntervalAuto = EditorGUILayout.FloatField("SpikeTrap Interval", stageSpikeTrapIntervalAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡箭矢陷阱數量(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				_gameStageData.ShootTrapAmountAuto = EditorGUILayout.IntField("ShootTrap Amount", stageShootTrapAmountAuto);
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 關卡箭矢陷阱速度(難度選擇自動)
				EditorGUILayout.BeginHorizontal();
				_gameStageData.ShootTrapIntervalAuto = EditorGUILayout.FloatField("ShootTrap Interval", stageShootTrapIntervalAuto);
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
				stageSoldierAmount = EditorGUILayout.IntField("Soldier Amount", _gameStageData.SoldierDataList.Count);

				RefreshEnemyTrapDataList<SoldierData>(stageSoldierAmount, _gameStageData.SoldierDataList);

				EditorGUILayout.EndHorizontal();

				if (_stageSoldierDataList.Count > 0)
				{
					EditorGUILayout.BeginHorizontal();
					isShowStageSoldierAmountFoldout = EditorGUILayout.Foldout(isShowStageSoldierAmountFoldout, "SoldierSettings");
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.TextArea("Suggest Settings:\n<b>Speed: 2f</b>", helpBoxStyle);

					if (isShowStageSoldierAmountFoldout)
					{
						try
						{
							for (int i = 0; i < _stageSoldierDataList.Count; i++)
							{
								EditorGUILayout.BeginHorizontal();
								_gameStageData.SoldierDataList[i].Speed = EditorGUILayout.FloatField("Soldier " + (i + 1) + " Speed:", _stageSoldierDataList[i].Speed);
								_gameStageData.SoldierDataList[i].PatrolPointsData = (PatrolPointsData)EditorGUILayout.ObjectField("\tPatrolPoints:", _stageSoldierDataList[i].PatrolPointsData, _gameStageData.SoldierPatrolPointsConfig.PatrolPointsDataList[0].GetType(), true);
								EditorGUILayout.EndHorizontal();
							}
						}
						catch (Exception e)
						{
							Debug.LogError("GameStageData.SoldierPatrolPointsConfig is null, " + e.Message);
						}
					}
				}
				#endregion
				GUILayout.Label(string.Empty);

				GUILayout.Label("SpikeTrap Settings", EditorStyles.largeLabel);
				#region 關卡地刺陷阱設定
				EditorGUILayout.BeginHorizontal();
				stageSpikeTrapAmount = EditorGUILayout.IntField("SpikeTrap Amount", _gameStageData.SpikeTrapDataList.Count);

				RefreshEnemyTrapDataList<SpikeTrapData>(stageSpikeTrapAmount, _gameStageData.SpikeTrapDataList);

				EditorGUILayout.EndHorizontal();

				if (_stageSpikeTrapDataList.Count > 0)
				{
					EditorGUILayout.BeginHorizontal();
					isShowStageSpikeTrapAmountFoldout = EditorGUILayout.Foldout(isShowStageSpikeTrapAmountFoldout, "SpikeTrapSetting");
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.TextArea("Suggest Settings:\n<b>Interval: 2f</b>", helpBoxStyle);

					if (isShowStageSpikeTrapAmountFoldout)
					{
						try
						{
							for (int i = 0; i < _stageSpikeTrapDataList.Count; i++)
							{
								EditorGUILayout.BeginHorizontal();
								_gameStageData.SpikeTrapDataList[i].Interval = EditorGUILayout.FloatField("SpikeTrap " + (i + 1) + " Interval:", _stageSpikeTrapDataList[i].Interval);
								_gameStageData.SpikeTrapDataList[i].PositionData = (PositionData)EditorGUILayout.ObjectField("\tPosition:", _stageSpikeTrapDataList[i].PositionData, _gameStageData.SpikeTrapPositionConfig.PositionDataList[0].GetType(), true);
								EditorGUILayout.EndHorizontal();
							}
						}
						catch (Exception e)
						{
							Debug.LogError("GameStageData.SpikeTrapPositionConfig is null, " + e.Message);
						}
					}
				}
				#endregion
				GUILayout.Label(string.Empty);

				GUILayout.Label("ShootTrap Settings", EditorStyles.largeLabel);
				#region 關卡箭矢陷阱設定
				EditorGUILayout.BeginHorizontal();
				stageShootTrapAmount = EditorGUILayout.IntField("ShootTrap Amount", _gameStageData.ShootTrapDataList.Count);

				RefreshEnemyTrapDataList<ShootTrapData>(stageShootTrapAmount, _gameStageData.ShootTrapDataList);
				
				EditorGUILayout.EndHorizontal();

				if (_stageSpikeTrapDataList.Count > 0)
				{
					EditorGUILayout.BeginHorizontal();
					isShowStageShootTrapAmountFoldout = EditorGUILayout.Foldout(isShowStageShootTrapAmountFoldout, "ShootTrapSetting");
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.TextArea("Suggest Settings:\n<b>Interval: 2f</b>\n<b>Speed: 100f</b>", helpBoxStyle);

					if (isShowStageShootTrapAmountFoldout)
					{
						try
						{
							for (int i = 0; i < _stageShootTrapDataList.Count; i++)
							{
								EditorGUILayout.BeginHorizontal();
								_gameStageData.ShootTrapDataList[i].Interval = EditorGUILayout.FloatField("ShootTrap " + (i + 1) + " Interval:", _stageShootTrapDataList[i].Interval);
								_gameStageData.ShootTrapDataList[i].Speed = EditorGUILayout.FloatField("\tSpeed:", _stageShootTrapDataList[i].Speed);
								_gameStageData.ShootTrapDataList[i].PositionData = (PositionData)EditorGUILayout.ObjectField("\tPosition:", _stageShootTrapDataList[i].PositionData, _gameStageData.ShootTrapPositionConfig.PositionDataList[0].GetType(), true);
								EditorGUILayout.EndHorizontal();
							}
						}
						catch (Exception e)
						{
							Debug.LogError("GameStageData.ShootTrapPositionConfig is null, " + e.Message);
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
		/// <typeparam name="T"></typeparam>
		/// <param name="amount">物件數量</param>
		/// <param name="objDataList">物件資料List</param>
		private void RefreshEnemyTrapDataList<T>(int amount, List<T> objDataList) where T : new()
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
						objDataList.Add(new T());
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