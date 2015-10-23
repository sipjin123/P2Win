using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameDataManager))]
public class GameDataManagerEditor : Editor {
//
//	private int startLevel = 1;
//	private int endLevel = 30;
//	private int startingMaxCoinBet = 500;
//	private int additionalMaxCoinBet = 250;
//	private PatternSet maxPatternToUse = PatternSet.MAX_25;
//	private int expRequiredMultiplier = 875;
//	private int previousExpRequired = 640000;

	public override void OnInspectorGUI()
	{
//		GameDataManager dataManager = (GameDataManager)target;
//
//		startLevel = EditorGUILayout.IntField("Start", startLevel);
//		endLevel = EditorGUILayout.IntField("End", endLevel);
//
//		if (endLevel < startLevel)
//			endLevel = startLevel;
//
//		startingMaxCoinBet = EditorGUILayout.IntField("Base Bet (Exclusive)", startingMaxCoinBet);
//		previousExpRequired = EditorGUILayout.IntField("Base EXP Required", previousExpRequired);
//		additionalMaxCoinBet = EditorGUILayout.IntField("Additional Bet", additionalMaxCoinBet);
//		maxPatternToUse = (PatternSet)EditorGUILayout.EnumPopup("Max Patterns" , maxPatternToUse);
//		expRequiredMultiplier = EditorGUILayout.IntField("Exp Required Multiplier", expRequiredMultiplier);
//
//
//		if (GUILayout.Button("Populate")) {
//			int currentBetAmount = startingMaxCoinBet;
//			int lastexpRequired = previousExpRequired;
//			for (int x = startLevel; x <= endLevel; x++) {
//				currentBetAmount += additionalMaxCoinBet;
//
//				int expRequired = lastexpRequired + (currentBetAmount * expRequiredMultiplier);
//				lastexpRequired = expRequired;
//				PlayerLevelData newData = new PlayerLevelData(x, currentBetAmount, maxPatternToUse, expRequired);
//				dataManager.AddLevelProgression_Editor(newData);
//			}
//		}
//	
//		EditorGUILayout.Separator();

		DrawDefaultInspector();
	}
	
}
