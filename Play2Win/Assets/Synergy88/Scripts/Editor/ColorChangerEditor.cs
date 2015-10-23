using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ColorChanger))]
public class ColorChangerEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		if (GUILayout.Button("Set all color to 100% alpha")) {
			foreach(Object currentTarget in targets) {
				ColorChanger spriteScript = (ColorChanger)currentTarget;
				spriteScript.SetColorAlpha_Editor();
			}
		}

	}

}
