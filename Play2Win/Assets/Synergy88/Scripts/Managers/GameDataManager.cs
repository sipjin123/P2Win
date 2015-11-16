using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct PlayerLevelData {
	[SerializeField]
	private int _level;

	[SerializeField]
	private int _maxCoinBet;

	[SerializeField]
	private PatternSet _maxLines;

	[SerializeField]
	private int _expToNextLevel;

    [SerializeField]
    private int _onLevelUpChips;

    [SerializeField]
    private int _onLevelUpGems;

	public PlayerLevelData(int level, int maxCoinBet, PatternSet maxLines, int expToNextLevel, int onLevelUpChips, int onLevelUpGems) {
		_level = level;
		_maxCoinBet = maxCoinBet;
		_maxLines = maxLines;
		_expToNextLevel = expToNextLevel;
        _onLevelUpChips = onLevelUpChips;
        _onLevelUpGems = onLevelUpGems;
	}

	public int Level { get { return _level; } }
	public int MaxCoinBet { get { return _maxCoinBet; } } // Index
	public PatternSet MaxLines { get { return _maxLines; } }
	public int ExpToNextLevel { get { return _expToNextLevel; } }
    public int OnLevelUpChips { get { return _onLevelUpChips; } }
    public int OnLevelUpGems { get { return _onLevelUpGems; } }
}

public class GameDataManager : MonoBehaviour {

	public const bool USE_CHEATS = false;

	private static GameDataManager _instance;
	public static GameDataManager Instance { get { return _instance; } }

	[SerializeField]
	private PlayerLevelData[] _levelProgression;
	private Dictionary<int, PlayerLevelData> _levelProgressionDictionary;

	public PlayerLevelData LevelInfo { get { return _levelProgressionDictionary[PlayerDataManager.Instance.Level]; } }


	private int _maxLevel = 0;
	public int MaxLevel { get { return _maxLevel; } }

	private bool _lobbyWindowsLoaded = false;
	public bool LobbyWindowsLoaded { get { return _lobbyWindowsLoaded; } }

	void Start() {
		_instance = this;
		InitializeProgressionDictionary();
	}

	private void InitializeProgressionDictionary() {
		_levelProgressionDictionary = new Dictionary<int, PlayerLevelData>();

		foreach(PlayerLevelData data in _levelProgression) {
			if (_levelProgressionDictionary.ContainsKey(data.Level)) {
				Debug.LogError("Level Data entry already added: " + data.Level);
				continue;
			}

			if (_maxLevel < data.Level)
				_maxLevel = data.Level;

			_levelProgressionDictionary.Add(data.Level, data);
		}
	}

	public void SetLobbyWindowsLoaded() {
		_lobbyWindowsLoaded = true;
	}

	public int GetCurrentBaseEXP() {
		if (PlayerDataManager.Instance.Level == 0) {
			return 0;
		} else {
			return _levelProgressionDictionary[PlayerDataManager.Instance.Level - 1].ExpToNextLevel;
		}
	}

//
//	public void AddLevelProgression_Editor(PlayerLevelData newData) {
//		_levelProgression[newData.Level - 1] = newData;
//	}

}
