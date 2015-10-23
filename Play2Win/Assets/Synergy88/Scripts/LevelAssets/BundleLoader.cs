using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BundleLevelData {
	public int level;
	public Texture2D downloadedTexture;
	public tk2dSpriteCollectionData collectionReference;
}

public class BundleLoader : MonoBehaviour {

	private const string ATLAS_NAME = "atlas0";
	private const string ATLAS_BUNDLE_PREFIX = "level";
	private const string ATLAS_BUNDLE_SUFFIX = "texture.unity3d";

	private static BundleLoader _instance;
	public static BundleLoader Instance { get { return _instance; } }

	public delegate void OnTextureAttachedDelegate(int level);

	private Dictionary<int, BundleLevelData> _loadedLevels;

	void Start() {
		_instance = this;
		_loadedLevels = new Dictionary<int, BundleLevelData>();
	}

	public bool TextureLoaded(int level) {
		return _loadedLevels.ContainsKey(level);
	}

	public tk2dSpriteCollectionData GetCachedCollectionData(int level) {
		return _loadedLevels[level].collectionReference;
	}

	public void LoadLevelTexture(int level, OnTextureAttachedDelegate onTextureAttached) {
		tk2dSpriteCollectionData data;

		if (TextureLoaded(level)) {
			data = GetCachedCollectionData(level);
		} else {
			data = Resources.Load("SlotLevel" + level + "/SlotLevel" + level + " Data/SlotLevel" + level, typeof(tk2dSpriteCollectionData)) as tk2dSpriteCollectionData;
		}

//		SetSpriteFromAssetBundle(level, data, onTextureAttached);
		StartCoroutine(SetSpriteFromAssetBundleWWWMode(level, data, onTextureAttached));
	}

	public void ClearTextureReferences() {
		foreach(int level in _loadedLevels.Keys) {
			_loadedLevels[level].downloadedTexture = null;
			_loadedLevels[level].collectionReference.textures[0] = null;
			_loadedLevels[level].collectionReference.materials[0].mainTexture = null;
			_loadedLevels[level].collectionReference = null;
		}
		_loadedLevels.Clear();
	}

//	private void SetSpriteFromAssetBundle(int level, tk2dSpriteCollectionData data, OnTextureAttachedDelegate endCallback) {
//
//		string url = BundleDownloadManager.Instance.GetLocalAssetPath() + BundleDownloadManager.Instance.GetLevelName(level);
//		
//		byte[] fileData = File.ReadAllBytes(url);
//
//#if UNITY_IOS
//		Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
//#else
//		Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, true);
//#endif
//		texture.LoadImage(fileData);
//
//		data.materials[0].SetTexture(0, texture);
//
//		if (!_loadedLevels.ContainsKey(level)) {
//			_loadedLevels.Add(level, new BundleLevelData());
//		}
//
//		BundleLevelData levelData = _loadedLevels[level];
//		levelData.level = level;
//		levelData.downloadedTexture = texture;
//		levelData.collectionReference = data;
//		_loadedLevels[level] = levelData;
//
//		endCallback(level);
//	}

	private IEnumerator SetSpriteFromAssetBundleWWWMode(int level, tk2dSpriteCollectionData data, OnTextureAttachedDelegate endCallback) {

		string url = "file://" + BundleDownloadManager.Instance.GetLocalAssetPath() + BundleDownloadManager.Instance.GetLevelName(level);

		Debug.LogWarning("Loading " + url);

//		pictureUrl = new WWW(System.Uri.EscapeUriString("file://" + Path.Combine(Application.persistentDataPath,"profilePicture.png")));
		WWW pictureUrl = new WWW(url);
		
		yield return pictureUrl;

		if (pictureUrl.error != null) {
			Debug.LogError(pictureUrl.error);
		}

		Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
		pictureUrl.LoadImageIntoTexture(texture);

		yield return new WaitForSeconds(0.1f);
		pictureUrl.Dispose();

		data.materials[0].mainTexture = texture;
		data.materials[0].SetTexture(0, texture);
		
		if (!_loadedLevels.ContainsKey(level)) {
			_loadedLevels.Add(level, new BundleLevelData());
		}
		
		BundleLevelData levelData = _loadedLevels[level];
		levelData.level = level;
		levelData.downloadedTexture = texture;
		levelData.collectionReference = data;
		_loadedLevels[level] = levelData;
		
		endCallback(level);
	}
}
