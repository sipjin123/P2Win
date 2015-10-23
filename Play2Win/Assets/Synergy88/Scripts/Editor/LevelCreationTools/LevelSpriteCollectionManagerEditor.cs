using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSpriteCollectionManager))]
public class LevelSpriteCollectionManagerEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Auto-populate sprite collection data")) {
			LevelSpriteCollectionManager collectionManager = (LevelSpriteCollectionManager)target;

			int levelCount = collectionManager.GetLevelCount();
			for (int i = 0; i < levelCount; i++) {
				int currentlevel = i + 1;
				string assetName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + currentlevel;
				string assetPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + assetName + "/" + assetName + " Data/" + assetName + ".prefab";
				tk2dSpriteCollectionData collectionData = AssetDatabase.LoadAssetAtPath<tk2dSpriteCollectionData>(assetPath);

				Debug.Log ("Assigning data [" + collectionData.name + "] from path [" + assetPath + "] for level " + currentlevel);
				collectionManager.AssignLevelCollectionData(currentlevel, collectionData);
			}

		}

//		if (GUILayout.Button("FIX AUDIO ENTRIES")) {
//			LevelSpriteCollectionManager collectionManager = (LevelSpriteCollectionManager)target;
//
//			int levelCount = collectionManager.GetLevelCount();
//			for (int i = 0; i < levelCount; i++) {
//				collectionManager.FixAudioEntries_Editor(i);
//			}
//		}

	}

}
