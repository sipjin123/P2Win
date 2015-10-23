using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PatternObject))]
public class PatternObjectEditor : Editor {
	
	
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("1_TOP")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(0, 0);
		}
		if (GUILayout.Button("1_MID")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(0, 1);
		}
		if (GUILayout.Button("1_BOT")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(0, 2);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("2_TOP")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(1, 0);
		}
		if (GUILayout.Button("2_MID")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(1, 1);
		}
		if (GUILayout.Button("2_BOT")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(1, 2);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("3_TOP")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(2, 0);
		}
		if (GUILayout.Button("3_MID")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(2, 1);
		}
		if (GUILayout.Button("3_BOT")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(2, 2);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("4_TOP")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(3, 0);
		}
		if (GUILayout.Button("4_MID")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(3, 1);
		}
		if (GUILayout.Button("4_BOT")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(3, 2);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("5_TOP")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(4, 0);
		}
		if (GUILayout.Button("5_MID")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(4, 1);
		}
		if (GUILayout.Button("5_BOT")) {
			PatternObject patternScript = (PatternObject)target;
			patternScript.SetSlotItemDetector(4, 2);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal();

		DrawDefaultInspector();
	}
	
}
