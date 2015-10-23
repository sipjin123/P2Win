using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PatternButton))]
public class PatternButtonEditor : Editor {


	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Refresh Sprite")) {
			foreach(Object currentTarget in targets) {
				PatternButton buttonScript = (PatternButton)currentTarget;
				buttonScript.SetSpriteInactive();
			}
		}

		if (GUILayout.Button("Set Button Action")) {
			foreach(Object currentTarget in targets) {
				PatternButton buttonScript = (PatternButton)currentTarget;

				tk2dUIItem button = buttonScript.GetComponent<tk2dUIItem>();
				button.SendMessageOnClickMethodName = "OnLineButtonClicked"; //button.sendMessageTarget.GetComponent<PatternManager>().OnLineButtonClicked;
			}
		}

	}

}
