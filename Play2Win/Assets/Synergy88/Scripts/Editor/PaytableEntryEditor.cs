using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PaytableEntry))]
public class PaytableEntryEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		if (GUILayout.Button("Auto-Assign Buttons")) {
			foreach(Object currentTarget in targets) {
				PaytableEntry entry = (PaytableEntry)currentTarget;
				entry.AutoSetSerializedValues();
			}
		}
	}
}
