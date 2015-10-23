using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BundleDownloadManager : MonoBehaviour {

	private struct BundleDownloadData {
		public float progress;
		public int level;
	}

	private static BundleDownloadManager _instance;
	public static BundleDownloadManager Instance { get { return _instance; } }

	public const string SIGNAL_PARAM_LEVEL = "level";
	public const string SIGNAL_PARAM_PROGRESS = "progress";

	private const string SERVER_PATH = "https://s3-us-west-2.amazonaws.com/definiteslot-unity/";  //"http://202.147.26.32/DefiniteSlots/LevelAssets/";
	private const string ATLAS_PREFIX = "SlotLevel";
	private const string ATLAS_EXTENSION = ".png";

	private List<BundleDownloadData> _activeDownloads;

	public delegate void DownloadEndDelegate(int level);

	void Start() {
		_instance = this;
		_activeDownloads = new List<BundleDownloadData>();
	}

	public bool CheckForLevel(int level) {

		if (level <= LevelCreationStatics.MIN_LEVEL_PRELOADED) {
			return true;
		}
		
		if (!File.Exists(GetLocalAssetPath() + Path.DirectorySeparatorChar + GetLevelName(level))) {
			return false;
		}

		return true;
	}

	public bool IsDownloadingLevel(int level) {
		for (int i = 0; i < _activeDownloads.Count; i++) {
			if (_activeDownloads[i].level == level) {
				return true;
			}
		}
		return false;
	}

	public void DownloadLevel(int level) {
		StartCoroutine(DownloadLevelWithManifest(GetLevelName(level), level));
	}

	private IEnumerator DownloadLevelWithManifest(string bundleName, int level) {
		BundleDownloadData data = new BundleDownloadData();
		data.progress = 0f;
		data.level = level;
		_activeDownloads.Add(data);

		ConcreteSignalParameters progressParam = new ConcreteSignalParameters();
		progressParam.AddParameter(SIGNAL_PARAM_LEVEL, level);
		progressParam.AddParameter(SIGNAL_PARAM_PROGRESS, 0f);
		SignalManager.Instance.CallWithParam(SignalType.LEVEL_DOWNLOAD_PROGRESS, progressParam);

		yield return StartCoroutine(DownloadFile(bundleName, data));

		_activeDownloads.Remove(data);

		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(SIGNAL_PARAM_LEVEL, level);
		SignalManager.Instance.CallWithParam(SignalType.LEVEL_DOWNLOAD_FINISHED, param);
	}

	private IEnumerator DownloadFile(string filename, BundleDownloadData progressData) {

		string url = SERVER_PATH + filename;
		WWW www = new WWW(url);

		ConcreteSignalParameters progressParam = new ConcreteSignalParameters();

		while (!www.isDone) {
			progressData.progress = www.progress;

			progressParam.Clear();
			progressParam.AddParameter(SIGNAL_PARAM_LEVEL, progressData.level);
			progressParam.AddParameter(SIGNAL_PARAM_PROGRESS, progressData.progress);
			SignalManager.Instance.CallWithParam(SignalType.LEVEL_DOWNLOAD_PROGRESS, progressParam);

			// Skip 5 frames
			yield return null;
			yield return null;
			yield return null;
			yield return null;
			yield return null;
		}

		if (www.error != null && www.error != "") {
			Debug.LogError("Download failed! " + www.error);
			yield break;
		}

		if (www.bytesDownloaded < 10000) { // Average file size should be more than 1MB (1000000+ bytes).
			Debug.LogError("Download failed! Not enough bytes!");
			yield break;
		}

		Debug.Log ("Finished downloading file: [" + url + "] with bytes: " + www.bytesDownloaded);
		
		byte[] data = www.bytes;
		string docPath = GetLocalAssetPath() + filename;

		if (File.Exists(docPath)) {
			File.Delete(docPath);
		}

		Debug.Log ("Writing to: " + docPath);
		File.WriteAllBytes(docPath, data);

		yield return new WaitForSeconds(0.1f);

		www.Dispose();

		// Buffer
		yield return new WaitForSeconds(0.5f);

		Debug.Log ("Finish saving to streaming folder.");
	}
	
	public string GetLevelName(int level) {
		return ATLAS_PREFIX + level + ATLAS_EXTENSION; // "level" + level + "texture.unity3d";
	}

	public string GetLocalAssetPath() {
#if UNITY_STANDALONE || UNITY_EDITOR
		return Application.streamingAssetsPath + "/AssetBundles/";
#else
		return Application.persistentDataPath + Path.DirectorySeparatorChar;
#endif
	}
}
