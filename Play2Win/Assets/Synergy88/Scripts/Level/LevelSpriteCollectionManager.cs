using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSpriteCollectionManager : MonoBehaviour {

	[System.Serializable]
	public struct LevelCollectionData {
		public string name;
		public int level;
		public tk2dSpriteCollectionData spriteCollection;
		public PatternSet pattern;
		public AudioManager.GlobalAudioType bgm;
	}

	private static LevelSpriteCollectionManager _instance;
	public static LevelSpriteCollectionManager Instance { get { return _instance; } }

//	private const string COLLECTION_PREFIX = "SlotLevel";

	[SerializeField]
	private LevelCollectionData[] _levels;

	public int ActiveLevel = 1;

	private Dictionary<int, LevelCollectionData> _levelDataDictionary;


	void Start() {
		_instance = this;

		_levelDataDictionary = new Dictionary<int, LevelCollectionData>();
		foreach(LevelCollectionData level in _levels) {
			if (_levelDataDictionary.ContainsKey(level.level)) {
				Debug.LogWarning("Level Data Dictionary has a duplicate entry for level : " + level.level);
				continue;
			}
			_levelDataDictionary.Add(level.level, level);
		}
	}

	public tk2dSpriteCollectionData GetSpriteCollectionData() {

		if (_levelDataDictionary[ActiveLevel].spriteCollection == null) {
			return BundleLoader.Instance.GetCachedCollectionData(ActiveLevel);
		} else {
			return _levelDataDictionary[ActiveLevel].spriteCollection;
		}
	}

	public PatternSet GetMaxPattern() {
		return _levelDataDictionary[ActiveLevel].pattern;
	}

	public int GetLevelCount() {
		return _levels.Length;
	}

	public AudioManager.GlobalAudioType GetBGM() {
		return _levelDataDictionary[ActiveLevel].bgm;
	}

	public void AssignLevelCollectionData(int level, tk2dSpriteCollectionData data) {
		_levels[level - 1].spriteCollection = data;
	}

//	public void FixAudioEntries_Editor(int level) {
//		switch(_levels[level].bgm) {
//		case AudioManager.GlobalAudioType.BUTTON_GENERIC:
//			_levels[level].bgm = AudioManager.GlobalAudioType.BGM_FANFARE;
//			break;
//		case AudioManager.GlobalAudioType.BUTTON_ADD:
//			_levels[level].bgm = AudioManager.GlobalAudioType.BGM_JUNGLE;
//			break;
//		case AudioManager.GlobalAudioType.BUTTON_DECREASE:
//			_levels[level].bgm = AudioManager.GlobalAudioType.BGM_MONTAGE;
//			break;
//		case AudioManager.GlobalAudioType.SLOT_SEGMENT_STOP:
//			_levels[level].bgm = AudioManager.GlobalAudioType.BGM_PEACEFUL;
//			break;
//		case AudioManager.GlobalAudioType.WIN_1:
//			_levels[level].bgm = AudioManager.GlobalAudioType.BGM_SEXY;
//			break;
//		}
//	}
}