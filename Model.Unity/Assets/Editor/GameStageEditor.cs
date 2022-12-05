using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameStage
{
	public class GameStageEditor : EditorWindow
	{
		private SerializedObject _globalSerializedObject;
		private SerializedProperty _serializedProperty;

		[SerializeField]
		private GameStageConfig _gameStageConfig;
		private GameStageData _gameStageData;

		#region 關卡相關(基本)
		private Level stageLevel;
		private int stageNum = 0;
		private bool isManual = false;
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
			StageSettings();
		}

		/// <summary>
		/// 同步Editor、Config
		/// </summary>
		private void SyncConfig()
		{
			_serializedProperty = _globalSerializedObject.FindProperty("StageDataList");

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
			#endregion

			_scrollPosContent = EditorGUILayout.BeginScrollView(_scrollPosContent);
		}

		/// <summary>
		/// 全域設定
		/// </summary>
		private void GlobalSettings()
		{
			_globalSerializedObject = new SerializedObject(_gameStageConfig);

			GUILayout.Label("Global Settings", EditorStyles.boldLabel);
			#region 作弊
			EditorGUILayout.PropertyField(_globalSerializedObject.FindProperty("IsCheat"), false);
			#endregion

			#region 玩家速度
			EditorGUILayout.PropertyField(_globalSerializedObject.FindProperty("PlayerSpeed"), false);
			#endregion

			_globalSerializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// 關卡設定
		/// </summary>
		private void StageSettings()
		{
            var property = _serializedProperty.GetArrayElementAtIndex(stageNum);

			GUILayout.Label("Base Settings", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Level"));
            EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("Image"));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("SceneName"));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("BGM"));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property.FindPropertyRelative("IsManual"));
			EditorGUILayout.EndHorizontal();

			isManual = property.FindPropertyRelative("IsManual").boolValue;

			EditorGUILayout.Space();
			GUILayout.Label("Advanced Settings", EditorStyles.boldLabel);

			using (new EditorGUI.DisabledScope(isManual)) 
			{
				InitStageLevelAutoSettings();
				EditorGUILayout.HelpBox("Random Create", MessageType.Warning);

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SoldierAmountAuto"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SoldierSpeedAuto"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SpikeTrapAmountAuto"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SpikeTrapIntervalAuto"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapAmountAuto"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapIntervalAuto"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapSpeedAuto"));
				EditorGUILayout.EndHorizontal();
			}

			using (new EditorGUI.DisabledScope(!isManual)) 
			{
				GUIStyle helpBoxStyle = GUI.skin.GetStyle("HelpBox");
				helpBoxStyle.fontSize = 15;
				helpBoxStyle.richText = true;

				EditorGUILayout.TextArea("Soldier Suggest Settings:\n<b>Speed: 2f</b>", helpBoxStyle);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SoldierDataList"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.TextArea("SpikeTrap Suggest Settings:\n<b>Interval: 2f</b>", helpBoxStyle);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("SpikeTrapDataList"));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.TextArea("ShootTrap Suggest Settings:\n<b>Interval: 2f</b>\n<b>Speed: 100f</b>", helpBoxStyle);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("ShootTrapDataList"));
				EditorGUILayout.EndHorizontal();
			}

			_globalSerializedObject.ApplyModifiedProperties();
			EditorGUILayout.EndScrollView();
		}

		/// <summary>
		/// 初始化關卡難度設定
		/// </summary>
		private void InitStageLevelAutoSettings()
		{
			switch (stageLevel)
			{
				case Level.Easy:
					_gameStageData.SoldierAmountAuto = 10;
					_gameStageData.SoldierSpeedAuto = 3f;
					_gameStageData.SpikeTrapAmountAuto = 45;
					_gameStageData.SpikeTrapIntervalAuto = 5f;
					_gameStageData.ShootTrapAmountAuto = 13;
					_gameStageData.ShootTrapIntervalAuto = 4f;
					break;

				case Level.Normal:
					_gameStageData.SoldierAmountAuto = 13;
					_gameStageData.SoldierSpeedAuto = 5f;
					_gameStageData.SpikeTrapAmountAuto = 60;
					_gameStageData.SpikeTrapIntervalAuto = 3f;
					_gameStageData.ShootTrapAmountAuto = 13;
					_gameStageData.ShootTrapIntervalAuto = 4f;
					break;

				case Level.Hard:
					_gameStageData.SoldierAmountAuto = 16;
					_gameStageData.SoldierSpeedAuto = 6f;
					_gameStageData.SpikeTrapAmountAuto = 75;
					_gameStageData.SpikeTrapIntervalAuto = 2f;
					_gameStageData.ShootTrapAmountAuto = 16;
					_gameStageData.ShootTrapIntervalAuto = 3f;
					break;
			}
		}
	}
}