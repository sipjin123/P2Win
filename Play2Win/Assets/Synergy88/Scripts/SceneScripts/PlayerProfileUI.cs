using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ProfileUIType {
    LOBBY,
    GEM_SCENES,
    SLOTS
}

public class PlayerProfileUI : MonoBehaviour, ISignalListener {

    [System.Serializable]
    public struct ProfileUIGroup {
        public ProfileUIType Type;
        public GameObject Transform;
        public tk2dTextMesh Coins;
        public tk2dTextMesh LoyaltyPoints;
        public tk2dTextMesh Level;
        public tk2dTextMesh Exp;
        public tk2dSprite ExpBar;
    }

	private const string INVALID_STRING = "----";
	private const float INCREMENT_TIME = 0.5f;
	private const float TICK_TIME = 0.01f;

    //[SerializeField]
    //private GameObject _lobbyBG;

    //[SerializeField]
    //private GameObject _gameBG;

    //[SerializeField]
    //private GameObject _gemOnlyObject;


    //[SerializeField]
    //private tk2dTextMesh _coinsText;

    //[SerializeField]
    //private tk2dTextMesh _coinsInGameText;


    //[SerializeField]
    //private tk2dTextMesh _boostersText;



    //[SerializeField]
    //private tk2dTextMesh _levelText;

    //[SerializeField]
    //private tk2dTextMesh _levelInGameText;

    //[SerializeField]
    //private tk2dTextMesh _expText;

    //[SerializeField]
    //private tk2dSprite _expBar;

    //[SerializeField]
    //private tk2dSprite _expBarInGame;


    [SerializeField]
    private ProfileUIGroup[] _uiGroups;

    private Dictionary<ProfileUIType, ProfileUIGroup> _uiGroupList;

	private int _previousCoinValue = -1;
	private int _coinsCurrentValue = 0;

	private bool _inputLocked = false;

	void Start() {

        _uiGroupList = new Dictionary<ProfileUIType, ProfileUIGroup>();
        for (int i = 0; i < _uiGroups.Length; i++) {
            _uiGroupList.Add(_uiGroups[i].Type, _uiGroups[i]);
        }

        SignalManager.Instance.Register(this, SignalType.LOCAL_DATA_CHANGED);
        SignalManager.Instance.Register(this, SignalType.UPDATE_PROFILE_HUD);

		SignalManager.Instance.Register(this, SignalType.PGS_LOGIN);
		SignalManager.Instance.Register(this, SignalType.PGS_SHOW_LEADERBOARD);
		SignalManager.Instance.Register(this, SignalType.PGS_UPDATE_LEADERBOARD);

		SignalManager.Instance.Register (this, SignalType.FB_USER_LOGGEDIN);
		SignalManager.Instance.Register (this, SignalType.PARSE_DOWNLOAD);
		SignalManager.Instance.Register (this, SignalType.PARSE_UPDATE);

		SignalManager.Instance.Register(this, SignalType.SLOT_MACHINE_STATE_CHANGED);

		_coinsCurrentValue = Mathf.FloorToInt(PlayerDataManager.Instance.Chips);
		_previousCoinValue = Mathf.FloorToInt(PlayerDataManager.Instance.Chips);

		UpdateDisplay();
		UpdateBackground(ProfileUIType.LOBBY);
	}
	
	void OnDestroy() {
		SignalManager.Instance.Remove(this, SignalType.LOCAL_DATA_CHANGED);
        SignalManager.Instance.Remove(this, SignalType.UPDATE_PROFILE_HUD);

		SignalManager.Instance.Remove(this, SignalType.PGS_LOGIN);
		SignalManager.Instance.Remove(this, SignalType.PGS_SHOW_LEADERBOARD);
		SignalManager.Instance.Remove(this, SignalType.PGS_UPDATE_LEADERBOARD);

		SignalManager.Instance.Remove (this, SignalType.FB_USER_LOGGEDIN);
		SignalManager.Instance.Remove (this, SignalType.PARSE_DOWNLOAD);
		SignalManager.Instance.Remove (this, SignalType.PARSE_UPDATE);

		SignalManager.Instance.Remove(this, SignalType.SLOT_MACHINE_STATE_CHANGED);
	}

	public void GoToLobby() {
		if (_inputLocked) {
			return;		
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        //GameManager.Instance.LoadScene(GameState.GAME_MENU);
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
	}

	public void OpenSettings() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		SignalManager.Instance.Call(SignalType.SHOW_SETTINGS);
	}

	public void OpenMoreCoins() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);

		if (_inputLocked) {
			return;		
		}
		
		SignalManager.Instance.Call(SignalType.BUTTON_MORE_COINS);
	}
	public void OpenMoreBoost() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);

		if (_inputLocked) {
			return;		
		}

		SignalManager.Instance.Call(SignalType.BUTTON_MORE_BOOSTERS);
	}
	
	public void OpenLeaderboards() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);

		if (_inputLocked) {
			return;		
		}

		SignalManager.Instance.Call(SignalType.PGS_SHOW_LEADERBOARD);
	}
	
	public void OpenAchievements() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);

		if (_inputLocked) {
			return;		
		}
	}

	private void UpdateDisplay() {
		if (PlayerDataManager.Instance == null) {
            foreach (ProfileUIType type in _uiGroupList.Keys) {
                _uiGroupList[type].Coins.text = INVALID_STRING;
                _uiGroupList[type].LoyaltyPoints.text = INVALID_STRING;
                _uiGroupList[type].Level.text = INVALID_STRING;
                _uiGroupList[type].Exp.text = INVALID_STRING;
            }
            //_coinsText.text = INVALID_STRING;
            //_coinsInGameText.text = INVALID_STRING;
            //_boostersText.text = INVALID_STRING;
            //_levelText.text = INVALID_STRING;
            //_levelInGameText.text = INVALID_STRING;
			return;
		}

		UpdateCoinsText(Mathf.FloorToInt(PlayerDataManager.Instance.Chips));

        foreach (ProfileUIType type in _uiGroupList.Keys) {
            _uiGroupList[type].LoyaltyPoints.text = PlayerDataManager.Instance.Points.ToString();
            _uiGroupList[type].Level.text = PlayerDataManager.Instance.Level.ToString();
            _uiGroupList[type].Exp.text = PlayerDataManager.Instance.Experience.ToString();
            _uiGroupList[type].ExpBar.scale = new Vector3(PlayerDataManager.Instance.ExpRatio, 1f, 1f);
        }
        //_boostersText.text = PlayerDataManager.Instance.Points.ToString();
        //_levelText.text = PlayerDataManager.Instance.Level.ToString();
        //_levelInGameText.text = PlayerDataManager.Instance.Level.ToString();
        //_expText.text = PlayerDataManager.Instance.Experience.ToString();
        //_expBar.scale = new Vector3(PlayerDataManager.Instance.ExpRatio, 1f, 1f);
        //_expBarInGame.scale = new Vector3(PlayerDataManager.Instance.ExpRatio, 1f, 1f);
	}

	private void UpdateBackground(ProfileUIType newType) {
        foreach (ProfileUIType type in _uiGroupList.Keys) {
            _uiGroupList[type].Transform.SetActive(type == newType);
        }
	}

	private void UpdateCoinsText(int value) {
		if (_coinsCurrentValue == value) {
			if (_coinsCurrentValue == 0) {
                foreach (ProfileUIType type in _uiGroupList.Keys) {
                    _uiGroupList[type].Coins.text = "0";
                }
                //_coinsText.text = "0";
                //_coinsInGameText.text = "0";
			} else {
                foreach (ProfileUIType type in _uiGroupList.Keys) {
                    _uiGroupList[type].Coins.text = _coinsCurrentValue.ToString("#,#");
                }
                //_coinsText.text = _coinsCurrentValue.ToString("#,#");
                //_coinsInGameText.text = _coinsCurrentValue.ToString("#,#");
			}
			return;
		}

		_previousCoinValue = _coinsCurrentValue;
		_coinsCurrentValue = value;

		StopCoroutine(IncreaseCoinsText());
        foreach (ProfileUIType type in _uiGroupList.Keys) {
            _uiGroupList[type].Coins.text = _previousCoinValue.ToString("#,#");
        }
        //_coinsText.text = _previousCoinValue.ToString("#,#");
        //_coinsInGameText.text =  _previousCoinValue.ToString("#,#");
		StartCoroutine(IncreaseCoinsText());
	}

	private IEnumerator IncreaseCoinsText() {
		int increase = _coinsCurrentValue - _previousCoinValue;
		int currentIndex = 0;
		int maxIndex = Mathf.FloorToInt(INCREMENT_TIME / TICK_TIME);
		int incrementAmount = Mathf.FloorToInt(increase / maxIndex);
		while (currentIndex < maxIndex) {
			currentIndex++;
			_previousCoinValue += incrementAmount;
            foreach (ProfileUIType type in _uiGroupList.Keys) {
                _uiGroupList[type].Coins.text = _previousCoinValue.ToString("#,#");
            }
			yield return new WaitForSeconds(TICK_TIME);
		}

		_previousCoinValue = _coinsCurrentValue;
		if (_coinsCurrentValue == 0) {
            foreach (ProfileUIType type in _uiGroupList.Keys) {
                _uiGroupList[type].Coins.text = "0";
            }
		} else {
            foreach (ProfileUIType type in _uiGroupList.Keys) {
                _uiGroupList[type].Coins.text = _coinsCurrentValue.ToString("#,#");
            }
		}
	}

	public void Execute(SignalType type, ISignalParameters param) {
		if (type == SignalType.LOCAL_DATA_CHANGED) {
			UpdateDisplay();
        } else if (type == SignalType.UPDATE_PROFILE_HUD) {
			UpdateBackground((ProfileUIType)param.GetParameter("ProfileUIType"));
		} else if (type == SignalType.SLOT_MACHINE_STATE_CHANGED) {
			_inputLocked = (bool)param.GetParameter(SlotMachineScene.PARAM_LOCK);
		}
	}

}
