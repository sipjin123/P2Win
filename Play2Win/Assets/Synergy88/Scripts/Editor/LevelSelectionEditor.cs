using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSelection))]
public class LevelSelectionEditor : Editor {

	public override void OnInspectorGUI() {

        //if (GUILayout.Button("Auto-populate Images")) {
        //    LevelSelection levelSelection = (LevelSelection)target;
        //    levelSelection.AutoPopulateImages();
        //}

		DrawDefaultInspector();
	}
}
