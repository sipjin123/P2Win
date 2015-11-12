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
    public struct ProfileUIAnimationTransition {
        public ProfileUIType Type;
        public string AnimationName;
    }

    [System.Serializable]
    public struct ProfileUIAnimation {
        public ProfileUIType Type;
        public ProfileUIAnimationTransition[] Transitions;
        public Dictionary<ProfileUIType, ProfileUIAnimationTransition> TransitionList;
    }

    [System.Serializable]
    public struct ProfileUIItems {
        public tk2dTextMesh Coins;
        public tk2dTextMesh LoyaltyPoints;
        public tk2dTextMesh Level;
        public tk2dTextMesh Exp;
        public tk2dSprite ExpBar;
    }

	private const string INVALID_STRING = "----";
	private const float INCREMENT_TIME = 0.5f;
	private const float TICK_TIME = 0.01f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private ProfileUIItems[] _uiGroups;

    [SerializeField]
    private ProfileUIAnimation[] _hudAnimations;

    private Dictionary<ProfileUIType, ProfileUIAnimation> _hudAnimationList;

    private ProfileUIType _currentType = ProfileUIType.LOBBY;

	private int _previousCoinValue = -1;
	private int _coinsCurrentValue = 0;

	private bool _inputLocked = false;

	void Start() {

        // Re-arranging array data into Dictionaries for easier access later
        _hudAnimationList = new Dictionary<ProfileUIType, ProfileUIAnimation>();
        for (int i = 0; i < _hudAnimations.Length; i++) {
            _hudAnimations[i].TransitionList = new Dictionary<ProfileUIType, ProfileUIAnimationTransition>();
            for (int j = 0; j < _hudAnimations[i].Transitions.Length; j++) {
                _hudAnimations[i].TransitionList.Add(_hudAnimations[i].Transitions[j].Type, _hudAnimations[i].Transitions[j]);
            }

            _hudAnimationList.Add(_hudAnimations[i].Type, _hudAnimations[i]);
        }

        SignalManager.Instance.Register(this, SignalType.LOCAL_DATA_CHANGED);
        SignalManager.Instance.Register(this, SignalType.UPDATE_PROFILE_HUD);

        SignalManager.Instance.Register(this, SignalType.PGS_LOGIN);
        SignalManager.Instance.Register(this, SignalType.PGS_SHOW_LEADERBOARD);
        SignalManager.Instance.Register(this, SignalType.PGS_UPDATE_LEADERBOARD);

        SignalManager.Instance.Register(this, SignalType.FB_USER_LOGGEDIN);
        SignalManager.Instance.Register(this, SignalType.PARSE_DOWNLOAD);
        SignalManager.Instance.Register(this, SignalType.PARSE_UPDATE);

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

        SignalManager.Instance.Remove(this, SignalType.FB_USER_LOGGEDIN);
        SignalManager.Instance.Remove(this, SignalType.PARSE_DOWNLOAD);
        SignalManager.Instance.Remove(this, SignalType.PARSE_UPDATE);

        SignalManager.Instance.Remove(this, SignalType.SLOT_MACHINE_STATE_CHANGED);
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            animator.SetTrigger("MoveRight");
        } else if (Input.GetKeyDown(KeyCode.U)) {
            animator.SetTrigger("MoveLeft");
        }
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
            foreach (ProfileUIItems item in _uiGroups) {
                item.Coins.text = INVALID_STRING;
                item.LoyaltyPoints.text = INVALID_STRING;
                item.Level.text = INVALID_STRING;
                item.Exp.text = INVALID_STRING;
            }
			return;
		}

		UpdateCoinsText(Mathf.FloorToInt(PlayerDataManager.Instance.Chips));

        foreach (ProfileUIItems item in _uiGroups) {
            item.LoyaltyPoints.text = PlayerDataManager.Instance.Points.ToString();
            item.Level.text = PlayerDataManager.Instance.Level.ToString();
            item.Exp.text = PlayerDataManager.Instance.Experience.ToString();
            item.ExpBar.scale = new Vector3(PlayerDataManager.Instance.ExpRatio, 1f, 1f);
        }
	}

	private void UpdateBackground(ProfileUIType newType) {
        animator.SetTrigger(_hudAnimationList[newType].TransitionList[_currentType].AnimationName);
        _currentType = newType;
        //foreach (ProfileUIType type in _uiGroupList.Keys) {
        //    if (type == newType) {
        //        if (type != _currentType) {
        //            _uiGroupList[_currentType].Transform.SetActive(false);
        //            _uiGroupList[type].Transform.SetActive(true);
        //            if (_uiGroupList.)
        //            _currentType = newType;
        //        }
        //        break;
        //    } else if (_uiGroupList[type].SecondaryType == newType) {

        //    }



        //    _uiGroupList[type].Transform.SetActive(type == newType);
        //    if (_uiGroupList[type].SecondaryType != type) {
        //        if (_uiGroupList[type].SecondaryType == newType) {

        //        }
        //    }
        //}
	}

	private void UpdateCoinsText(int value) {
		if (_coinsCurrentValue == value) {
			if (_coinsCurrentValue == 0) {
                foreach (ProfileUIItems item in _uiGroups) {
                    item.Coins.text = "0";
                }
			} else {
                foreach (ProfileUIItems item in _uiGroups) {
                    item.Coins.text = _coinsCurrentValue.ToString("#,#");
                }
			}
			return;
		}

		_previousCoinValue = _coinsCurrentValue;
		_coinsCurrentValue = value;

		StopCoroutine(IncreaseCoinsText());
        foreach (ProfileUIItems item in _uiGroups) {
            item.Coins.text = _previousCoinValue.ToString("#,#");
        }

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
            foreach (ProfileUIItems item in _uiGroups) {
                item.Coins.text = _previousCoinValue.ToString("#,#");
            }
			yield return new WaitForSeconds(TICK_TIME);
		}

		_previousCoinValue = _coinsCurrentValue;
		if (_coinsCurrentValue == 0) {
            foreach (ProfileUIItems item in _uiGroups) {
                item.Coins.text = "0";
            }
		} else {
            foreach (ProfileUIItems item in _uiGroups) {
                item.Coins.text = _coinsCurrentValue.ToString("#,#");
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
