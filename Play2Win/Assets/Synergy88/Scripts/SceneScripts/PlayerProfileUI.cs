using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfileUI : MonoBehaviour, ISignalListener {

	private const string INVALID_STRING = "----";
	private const float INCREMENT_TIME = 0.5f;
	private const float TICK_TIME = 0.01f;

    //[SerializeField]
    //private GameObject _lobbyBG;

    //[SerializeField]
    //private GameObject _gameBG;

	[SerializeField]
	private tk2dTextMesh _coinsText;

	[SerializeField]
	private tk2dTextMesh _boostersText;

	[SerializeField]
	private tk2dTextMesh _levelText;

    [SerializeField]
    private tk2dTextMesh _expText;

	[SerializeField]
	private tk2dSprite _expBar;

	private int _previousCoinValue = -1;
	private int _coinsCurrentValue = 0;

	private bool _inputLocked = false;

    //[SerializeField]
    //GameObject _profilePicture;

	private Dictionary<string,string> profile;

	private string getDataValueForKey(Dictionary <string,object> dict ,string key){
		object objectForKey;
		if (dict.TryGetValue (key, out objectForKey)) {
			return(string)objectForKey;
		} 

		else {
			return "";
		}
	}




	void Start() {

		SignalManager.Instance.Register(this, SignalType.LOCAL_DATA_CHANGED);
		SignalManager.Instance.Register(this, SignalType.LOBBY_ENTERED);
		SignalManager.Instance.Register(this, SignalType.GAME_ENTERED);

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
		UpdateBackground(false);
	}
	
	private void HandleAuthenticated(bool success){
		Debug.Log("*** HandleAuthenticated: success = " + success);
	}
	
	void OnDestroy() {
		SignalManager.Instance.Remove(this, SignalType.LOCAL_DATA_CHANGED);
		SignalManager.Instance.Remove(this, SignalType.LOBBY_ENTERED);
		SignalManager.Instance.Remove(this, SignalType.GAME_ENTERED);

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
		GameManager.Instance.LoadScene(GameState.GAME_MENU);
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
			_coinsText.text = INVALID_STRING;
			_boostersText.text = INVALID_STRING;
			_levelText.text = INVALID_STRING;
			return;
		}

		UpdateCoinsText(Mathf.FloorToInt(PlayerDataManager.Instance.Chips));
		_boostersText.text = PlayerDataManager.Instance.Points.ToString();
		_levelText.text = PlayerDataManager.Instance.Level.ToString();
        _expText.text = PlayerDataManager.Instance.Experience.ToString();
		_expBar.scale = new Vector3(PlayerDataManager.Instance.ExpRatio, 1f, 1f);
	}

	private void UpdateBackground(bool inGame) {
        //_lobbyBG.SetActive(!inGame);
        //_gameBG.SetActive(inGame);
	}

	private void UpdateCoinsText(int value) {
		if (_coinsCurrentValue == value) {
			if (_coinsCurrentValue == 0) {
				_coinsText.text = "0";
			} 

			else {
				_coinsText.text = _coinsCurrentValue.ToString("#,#");
			}
			return;
		}

		_previousCoinValue = _coinsCurrentValue;
		_coinsCurrentValue = value;

		StopCoroutine(IncreaseCoinsText());
		_coinsText.text = _previousCoinValue.ToString("#,#");
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
			_coinsText.text = _previousCoinValue.ToString("#,#");
			yield return new WaitForSeconds(TICK_TIME);
		}

		_previousCoinValue = _coinsCurrentValue;
		if (_coinsCurrentValue == 0) {
			_coinsText.text = "0";
		} 

		else {
			_coinsText.text = _coinsCurrentValue.ToString("#,#");
		}
	}

	public void Execute(SignalType type, ISignalParameters param) {
		if (type == SignalType.LOCAL_DATA_CHANGED) {
			UpdateDisplay();
		} 

		else if (type == SignalType.GAME_ENTERED) {
			UpdateBackground(true);
		} 

		else if (type == SignalType.LOBBY_ENTERED) {
			UpdateBackground(false);
		} else if (type == SignalType.SLOT_MACHINE_STATE_CHANGED) {
			_inputLocked = (bool)param.GetParameter(SlotMachineScene.PARAM_LOCK);
		}
	}

}
