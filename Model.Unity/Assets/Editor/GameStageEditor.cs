using System;
using UnityEngine;
using UnityEditor;

namespace GameStage
{
	public class GameStageEditor : EditorWindow
	{
		private SerializedObject _serializedObject;
		private SerializedProperty _serializedProperty;

		[SerializeField]
		private GameStageConfig _gameStageConfig;
		private GameStageData _gameStageData;

		#region 關卡相關(基本)
		private Level stageLevel;
		private int stageNum = 0;
		private bool isManual = false;

		private bool isShowStageSoldierAmountFoldout = false;
		private bool isShowStageSpikeTrapAmountFoldout = false;
		private bool isShowStageShootTrapAmountFoldout = false;
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
			StageSettings();
		}

		/// <summary>
		/// 全域設定 + 初始化
		/// </summary>
		private void GlobalSettings()
		{
			#region 全域設定(上方)
			_serializedObject = new SerializedObject(_gameStageConfig);

			GUILayout.Label("Global Settings", EditorStyles.boldLabel);

			#region 作弊
			EditorGUILayout.PropertyField(_serializedObject.FindProperty("IsCheat"));
			#endregion

			#region 玩家速度
			EditorGUILayout.PropertyField(_serializedObject.FindProperty("PlayerSpeed"));
			#endregion

			_serializedObject.ApplyModifiedProperties();
			#endregion

			#region 關卡初始化(左半邊Button + 判斷關卡編號)
			_serializedProperty = _serializedObject.FindProperty("StageDataList");

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

			_gameStageData = _gameStageConfig.StageDataList[stageNum];
			stageLevel = _gameStageData.Level;
            #endregion
        }

        /// <summary>
        /// 關卡設定(右半邊)
        /// </summary>
        private void StageSettings()
		{
			_scrollPosContent = EditorGUILayout.BeginScrollView(_scrollPosContent);

			var property = _serializedProperty.GetArrayElementAtIndex(stageNum);

			GUILayout.Label("Base Settings", EditorStyles.boldLabel);

			#region 難度
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("Level"));
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 關卡縮圖
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("Image"));
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 場景名稱
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("SceneName"));
			EditorGUILayout.EndHorizontal();
			#endregion

			#region BGM
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("BGM"));
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 手動設定(客製化)
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("IsManual"));
			EditorGUILayout.EndHorizontal();
			#endregion

			isManual = property.FindPropertyRelative("IsManual").boolValue;

			EditorGUILayout.Space();

			GUILayout.Label("Advanced Settings", EditorStyles.boldLabel);

			#region 勾選手動(關閉以下)
			using (new EditorGUI.DisabledScope(isManual)) 
			{
				InitStageLevelAutoSettings();

				EditorGUILayout.HelpBox("Random Create", MessageType.Warning);

				#region 士兵數量
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SoldierAmountAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 士兵速度
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SoldierSpeedAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 地刺陷阱數量
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SpikeTrapAmountAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 地刺陷阱間隔
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SpikeTrapIntervalAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 箭矢陷阱數量
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapAmountAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 箭矢陷阱間隔
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapIntervalAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion

				#region 箭矢陷阱速度
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapSpeedAuto"));
				EditorGUILayout.EndHorizontal();
				#endregion
			}
			#endregion

			#region 不勾選手動(關閉以下)
			using (new EditorGUI.DisabledScope(!isManual)) 
			{
				GUIStyle helpBoxStyle = GUI.skin.GetStyle("HelpBox");
				helpBoxStyle.fontSize = 15;
				helpBoxStyle.richText = true;

				#region 士兵資料設定
				var soldierDataList = property.FindPropertyRelative("SoldierDataList");
				var soldierPatrolPointsConfig = property.FindPropertyRelative("SoldierPatrolPointsConfig");

				EditorGUILayout.BeginHorizontal();
				soldierDataList.arraySize = EditorGUILayout.IntField("Soldier Amount", soldierDataList.arraySize);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				isShowStageSoldierAmountFoldout = EditorGUILayout.Foldout(isShowStageSoldierAmountFoldout, "SoldierSettings");
				EditorGUILayout.EndHorizontal();

				if ((soldierPatrolPointsConfig.objectReferenceValue != null) && (soldierPatrolPointsConfig.objectReferenceValue.name == soldierPatrolPointsConfig.name))
				{
					var patrolPointsConfig = _gameStageData.SoldierPatrolPointsConfig;
					bool isComplete = patrolPointsConfig.IsComplete(patrolPointsConfig);

					if (isShowStageSoldierAmountFoldout) 
					{
						EditorGUILayout.TextArea("Soldier Suggest Settings:\n<b>Speed: 2f</b>", helpBoxStyle);

						for (int i = 0; i < soldierDataList.arraySize; i++)
						{
							var patrolPointsData = soldierDataList.GetArrayElementAtIndex(i).FindPropertyRelative("PatrolPointsData");

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(soldierDataList.GetArrayElementAtIndex(i).FindPropertyRelative("Speed"));
							patrolPointsData.objectReferenceValue = EditorGUILayout.ObjectField("Patrol Point:", patrolPointsData.objectReferenceValue, patrolPointsConfig.GetDataType(isComplete), true);
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				else
				{
					Debug.LogError("SoldierPatrolPointsConfig field is null or not correct");
				}
				#endregion

				EditorGUILayout.Space();

				#region 地刺陷阱資料設定
				var spikeTrapDataList = property.FindPropertyRelative("SpikeTrapDataList");
				var spikeTrapPositionConfig = property.FindPropertyRelative("SpikeTrapPositionConfig");

				EditorGUILayout.BeginHorizontal();
				spikeTrapDataList.arraySize = EditorGUILayout.IntField("SpikeTrap Amount", spikeTrapDataList.arraySize);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				isShowStageSpikeTrapAmountFoldout = EditorGUILayout.Foldout(isShowStageSpikeTrapAmountFoldout, "SpikeTrapSettings");
				EditorGUILayout.EndHorizontal();

				if ((spikeTrapPositionConfig.objectReferenceValue != null) && (spikeTrapPositionConfig.objectReferenceValue.name == spikeTrapPositionConfig.name))
				{
					var positionConfig = _gameStageData.SpikeTrapPositionConfig;
					bool isComplete = positionConfig.IsComplete(positionConfig);

					if (isShowStageSpikeTrapAmountFoldout) 
					{
						EditorGUILayout.TextArea("Suggest Settings:\n<b>Interval: 2f</b>", helpBoxStyle);

						for (int i = 0; i < spikeTrapDataList.arraySize; i++)
						{
							var positionData = spikeTrapDataList.GetArrayElementAtIndex(i).FindPropertyRelative("PositionData");

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(spikeTrapDataList.GetArrayElementAtIndex(i).FindPropertyRelative("Interval"));
							positionData.objectReferenceValue = EditorGUILayout.ObjectField("Position:", positionData.objectReferenceValue, positionConfig.GetDataType(isComplete), true);
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				else
				{
					Debug.LogError("SpikeTrap Position Config field is null or not correct");
				}
				#endregion

				EditorGUILayout.Space();

				#region 箭矢陷阱資料設定
				var shootTrapDataList = property.FindPropertyRelative("ShootTrapDataList");
				var shootTrapPositionConfig = property.FindPropertyRelative("ShootTrapPositionConfig");

				EditorGUILayout.BeginHorizontal();
				shootTrapDataList.arraySize = EditorGUILayout.IntField("ShootTrap Amount", shootTrapDataList.arraySize);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				isShowStageShootTrapAmountFoldout = EditorGUILayout.Foldout(isShowStageShootTrapAmountFoldout, "ShootTrapSettings");
				EditorGUILayout.EndHorizontal();

				if ((shootTrapPositionConfig.objectReferenceValue != null) && (shootTrapPositionConfig.objectReferenceValue.name == shootTrapPositionConfig.name))
				{
					var positionConfig = _gameStageData.ShootTrapPositionConfig;
					bool isComplete = positionConfig.IsComplete(positionConfig);

					if (isShowStageShootTrapAmountFoldout)
					{
						EditorGUILayout.TextArea("Suggest Settings:\n<b>Interval: 2f</b>\n<b>Speed: 100f</b>", helpBoxStyle);

						for (int i = 0; i < shootTrapDataList.arraySize; i++)
						{
							var positionData = shootTrapDataList.GetArrayElementAtIndex(i).FindPropertyRelative("PositionData");

							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PropertyField(shootTrapDataList.GetArrayElementAtIndex(i).FindPropertyRelative("Interval"));
							EditorGUILayout.PropertyField(shootTrapDataList.GetArrayElementAtIndex(i).FindPropertyRelative("Speed"));
							positionData.objectReferenceValue = EditorGUILayout.ObjectField("Position:", positionData.objectReferenceValue, positionConfig.GetDataType(isComplete), true);
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				else
				{
					Debug.LogError("ShootTrap Position Config field is null or not correct");
				}
				#endregion

				EditorGUILayout.Space();
			}
			#endregion

			GUILayout.Label("Configs", EditorStyles.boldLabel);

			#region 士兵資料Config
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("SoldierPatrolPointsConfig"));
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 地刺陷阱資料Config
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("SpikeTrapPositionConfig"));
			EditorGUILayout.EndHorizontal();
			#endregion

			#region 箭矢陷阱資料Config
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapPositionConfig"));
			EditorGUILayout.EndHorizontal();
			#endregion

			EditorGUILayout.EndScrollView();
			_serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// 初始化關卡難度設定
		/// </summary>
		private void InitStageLevelAutoSettings()
		{
			switch (stageLevel)
			{
				case Level.Easy:
					_gameStageData.SoldierAmountAuto = (int)Math.Round((float)_gameStageData.SoldierDataList.Count * 0.8f);
					_gameStageData.SoldierSpeedAuto = (6f * 0.6f);
					_gameStageData.SpikeTrapAmountAuto = (int)Math.Round((float)_gameStageData.SpikeTrapDataList.Count * 0.8f);
					_gameStageData.SpikeTrapIntervalAuto = (2f * 2.5f);
					_gameStageData.ShootTrapAmountAuto = (int)Math.Round((float)_gameStageData.ShootTrapDataList.Count * 0.8f);
					_gameStageData.ShootTrapIntervalAuto = (3f * 2f);
					break;

				case Level.Normal:
					_gameStageData.SoldierAmountAuto = (int)Math.Round((float)_gameStageData.SoldierDataList.Count * 0.8f);
					_gameStageData.SoldierSpeedAuto = (6f * 0.8f);
					_gameStageData.SpikeTrapAmountAuto = (int)Math.Round((float)_gameStageData.SpikeTrapDataList.Count * 0.8f);
					_gameStageData.SpikeTrapIntervalAuto = (2f * 1.5f);
					_gameStageData.ShootTrapAmountAuto = (int)Math.Round((float)_gameStageData.ShootTrapDataList.Count * 0.8f);
					_gameStageData.ShootTrapIntervalAuto = (3f * 1.2f);
					break;

				case Level.Hard:
					_gameStageData.SoldierAmountAuto = _gameStageData.SoldierDataList.Count;
					_gameStageData.SoldierSpeedAuto = 6f;
					_gameStageData.SpikeTrapAmountAuto = _gameStageData.SpikeTrapDataList.Count;
					_gameStageData.SpikeTrapIntervalAuto = 2f;
					_gameStageData.ShootTrapAmountAuto = _gameStageData.ShootTrapDataList.Count;
					_gameStageData.ShootTrapIntervalAuto = 3f;
					break;
			}
		}
	}
}