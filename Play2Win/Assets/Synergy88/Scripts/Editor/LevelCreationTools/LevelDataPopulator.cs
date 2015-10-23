using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

public class LevelDataPopulator {

	public const string REMOVE_TEXTURE_MENU = "Remove Texture References/";
	public const string CONNECT_TEXTURE_MENU = "Re-Connect Texture References/";

//	[MenuItem(LevelCreationStatics.MENU_BASE_NAME + "Auto-assign asset bundle names to textures")]
//	public static void AssignAssetBundles() {
//		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
//			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
//			AssetImporter.GetAtPath(LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0.png").assetBundleName = "level" + i + "texture.unity3d";
//		}
//	}

	[MenuItem(LevelCreationStatics.MENU_BASE_NAME + "Move Textures to external folder")]
	public static void RemoveReferencesForBundle() {

		if (!Directory.Exists(LevelCreationStatics.SLOT_COLLECTION_EXTERNAL_PATH)) {
			Directory.CreateDirectory(LevelCreationStatics.SLOT_COLLECTION_EXTERNAL_PATH);
		}

		// Remove All References
		RemoveTextureReferencesForMaterials();
		RemoveTextureReferencesForCollectionData();

		// Move All Files to Temp Folder
		MoveTexturesOutside();

		// Move Temp to External Folder
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i + ".png";
			FileUtil.MoveFileOrDirectory(LevelCreationStatics.SLOT_COLLECTION_TEMP_PATH + levelName, LevelCreationStatics.SLOT_COLLECTION_EXTERNAL_PATH + levelName);
		}
	}

	[MenuItem(LevelCreationStatics.MENU_BASE_NAME + "Return External Files to Project (Triggers Re-import)")]
	public static void ReturnReferencesToProject() {
		
		if (!Directory.Exists(LevelCreationStatics.SLOT_COLLECTION_EXTERNAL_PATH)) {
			Directory.CreateDirectory(LevelCreationStatics.SLOT_COLLECTION_EXTERNAL_PATH);
		}
		
		// Retrieve from External Folder to Temp
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i + ".png";
			FileUtil.MoveFileOrDirectory(LevelCreationStatics.SLOT_COLLECTION_EXTERNAL_PATH + levelName, LevelCreationStatics.SLOT_COLLECTION_TEMP_PATH + levelName);
		}
	}


	[MenuItem(LevelCreationStatics.MENU_BASE_NAME + "Return Textures to resources")]
	public static void ReturnReferencesToCollections() {
		// Move All Files to Temp Folder
		RetrieveTexturesOutside();

		// Remove All References
		ConnectTextureReferencesToMaterials();
		ConnectTextureReferencesToCollectionData();
	}

	public static void RemoveTextureReferencesForMaterials() {
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
			string materialPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0 material.mat";
			Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
			
			if (mat == null) {
				Debug.Log ("Could not load Material from path [" + materialPath + "]. Skipping.");
			} else {
				mat.mainTexture = null;
			}
		}
	}

	public static void RemoveTextureReferencesForCollectionData() {
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
			string dataPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/" + levelName + ".prefab";
			tk2dSpriteCollectionData data = AssetDatabase.LoadAssetAtPath<tk2dSpriteCollectionData>(dataPath);
			
			if (data == null) {
				Debug.Log ("Could not load Collection Data from path [" + dataPath + "]. Skipping.");
			} else {
				data.textures[0] = null;
			}
		}
	}


	public static void ConnectTextureReferencesToMaterials() {
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
			string materialPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0 material.mat";
			string texturePath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0.png";
			Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
			Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
			
			if (mat == null) {
				Debug.Log ("Could not load Material from path [" + materialPath + "]. Skipping.");
			} else if (texture == null) {
				Debug.Log ("Could not load Texture from path [" + texturePath + "]. Skipping.");
			} else {
				mat.mainTexture = texture;
			}
		}
	}
	
	public static void ConnectTextureReferencesToCollectionData() {
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
			string dataPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/" + levelName + ".prefab";
			string texturePath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0.png";
			tk2dSpriteCollectionData data = AssetDatabase.LoadAssetAtPath<tk2dSpriteCollectionData>(dataPath);
			Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
			
			if (data == null) {
				Debug.Log ("Could not load Collection Data from path [" + dataPath + "]. Skipping.");
			} else if (texture == null) {
				Debug.Log ("Could not load Texture from path [" + texturePath + "]. Skipping.");
			} else {
				data.textures[0] = texture;
			}
		}
	}
	
	public static void MoveTexturesOutside() {
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
			string dataPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0.png";
			string externalPath = LevelCreationStatics.SLOT_COLLECTION_TEMP_PATH + levelName + ".png";
			AssetDatabase.MoveAsset(dataPath, externalPath);
		}
	}
	
	public static void RetrieveTexturesOutside() {
		for (int i = LevelCreationStatics.MIN_LEVEL_PRELOADED + 1; i <= LevelCreationStatics.MAX_LEVEL_AVAILABLE; i++) {
			string levelName = LevelCreationStatics.SLOT_COLLECTION_PREFIX + i;
			string dataPath = LevelCreationStatics.SLOT_COLLECTION_DATA_PATH + levelName + "/" + levelName + " Data/atlas0.png";
			string externalPath = LevelCreationStatics.SLOT_COLLECTION_TEMP_PATH + levelName + ".png";
			AssetDatabase.MoveAsset(externalPath, dataPath);
		}
	}
}
