using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotMachineScene : MonoBehaviour, ISignalListener {

	public const string PARAM_LOCK = "locked";

	private const string BUTTON_AUTOPLAY_INACTIVE = "button_spinhold01";
	private const string BUTTON_AUTOPLAY_ACTIVE = "button_spinactive01";
	private const float AUTOPLAY_HOLD_DURATION = 1f;
	private const float AUTOSPIN_DELAY = 0.5f;
	private const float AUTOSPIN_WIN_DELAY = 3f;

	[SerializeField]
	private tk2dSprite _spinButtonSprite;

	[SerializeField]
	private tk2dTextMesh _coinBetText;

	[SerializeField]
	private tk2dTextMesh _lineBetText;

	[SerializeField]
	private tk2dTextMesh _boosterBetText;

	[SerializeField]
	private tk2dTextMesh _totalBetText;

	[SerializeField]
	private tk2dTextMesh _previousWinningsText;


	[SerializeField]
	private PatternManager _patternManager;

	[SerializeField]
	private SlotSegment[] _slots;

	[SerializeField]
	private SpinTheWheelManager _spinTheWheel;
	
	[SerializeField]
	private BoostersWinManager _boostersWin;
	
	[SerializeField]
	private NotEnoughCoins _notEnoughCoins;

	[SerializeField]
	private NotEnoughBoosters _notEnoughBoosters;

	[SerializeField]
	private FreeSpinsWinManager _freeSpins;

	[SerializeField]
	private BigWinManager _bigWin;

	[SerializeField]
	private SlideInObject _remainingFreeSpins;

	[SerializeField]
	private tk2dTextMesh _freeSpinsText;

	[SerializeField]
	private PaytableManager _paytable;
	
	[SerializeField]
	private float[] _betAmounts;

	private float _totalBet;

	private float _currentCoinBet;
	private int _currentCoinBetIndex; // Temp(?)

	private int _linesActive;
	private int _boostersToUse;
	private int _previousWinnings;	

//	private float _holdTimerStart;
	private float _holdTimerRemaining;
	private bool _spinButtonHeld;

	private int _freeSpinsActive = 0;
	private bool _autoplayActive = false;
	private bool _spinning = false;
	private bool _wasAutoplaying = false;

	private bool _extraPopupShown = false;

	private List<IExtraRewardWindow> _extraRewardsWindow;

	void Start() {
		SignalManager.Instance.Call(SignalType.GAME_ENTERED);
		SignalManager.Instance.Register(this, SignalType.PATTERN_UPDATED);
		SignalManager.Instance.Register(this, SignalType.EXTRA_REWARD_CLOSED);
		SignalManager.Instance.Register(this, SignalType.LEVELED_UP);
		SignalManager.Instance.Register(this, SignalType.LEVELUPWINDOW_CLOSED);

		_linesActive = PlayerDataManager.Instance.LastPattern;
		_currentCoinBetIndex = PlayerDataManager.Instance.LastBet;
		
		_patternManager.Initialize();
		SetPatternUsed();

		if (_linesActive > _patternManager.MaxLinesAllowed()) {
			_linesActive = _patternManager.MaxLinesAllowed();
		}

		if (GetBetAmount(_currentCoinBetIndex) > GameDataManager.Instance.LevelInfo.MaxCoinBet) {
			_currentCoinBet = 0;
		}

		UpdateTotalBet();
		UpdateBoostersUsed();
		UpdateBetAmount();

		_extraRewardsWindow = new List<IExtraRewardWindow>();

		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(SignalManager.PARAM_PATTERNCOUNT, _linesActive);
		SignalManager.Instance.CallWithParam(SignalType.PATTERN_UPDATED, param);

		ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
		stateParam.AddParameter(PARAM_LOCK, false);
		SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);

		AudioManager.Instance.SwitchBGM(LevelSpriteCollectionManager.Instance.GetBGM());

		// TODO: Put this after a proper loading
		_spinTheWheel.Hide();
		_boostersWin.Hide();
		_notEnoughCoins.Hide();
		_notEnoughBoosters.Hide();
		_freeSpins.Hide();
		_bigWin.Hide();
		_paytable.Load();
		_paytable.Hide();
	}

	void Update() {
		if (_autoplayActive || _spinning)
			return;

		if (Input.GetKeyDown(KeyCode.Escape)) {
			GameManager.Instance.LoadScene(GameState.GAME_MENU);
		}

		if (_spinButtonHeld) {
			_holdTimerRemaining -= Time.deltaTime;
			if (_holdTimerRemaining <= 0f) {
				SwitchAutoplay();
				_spinButtonHeld = false;
				AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
			}
		}
	}

	void OnDestroy() {
		SignalManager.Instance.ClearRegisterType(SignalType.PATTERN_UPDATED);
		SignalManager.Instance.ClearRegisterType(SignalType.EXTRA_REWARD_CLOSED);
		SignalManager.Instance.Remove(this, SignalType.LEVELED_UP);
		SignalManager.Instance.Remove(this, SignalType.LEVELUPWINDOW_CLOSED);
	}

	private void SetPatternUsed() {
		if (_patternManager.GetLineInt(GameDataManager.Instance.LevelInfo.MaxLines) < _patternManager.GetLineInt(LevelSpriteCollectionManager.Instance.GetMaxPattern())) {
			_patternManager.SetActivePattern(GameDataManager.Instance.LevelInfo.MaxLines);
		} 
		else {
			_patternManager.SetActivePattern(LevelSpriteCollectionManager.Instance.GetMaxPattern());
		}
	}

	public void StartSlots() {
//		if (_totalBet > PlayerDataManager.Instance.Coins) {
//			return;
//		}
//		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);

//		if (_autoplayActive && !_holdingForAutoplay) {
//			SwitchAutoplay();
//			return;
//		}
//
//		if (_spinning) {
//			return;
//		}
//
//		Spin();
	}

	public void StartButtonHoldStart() {
		/*
		if (_totalBet > PlayerDataManager.Instance.Coins) {
			return;
		}*/

//		_holdTimerStart = Time.timeSinceLevelLoad;
		_holdTimerRemaining = AUTOPLAY_HOLD_DURATION;
		_spinButtonHeld = true;
	}

	public void StartButtonHoldEnd() {
		/*
		if (_totalBet > PlayerDataManager.Instance.Coins) {
			return;
		}*/

		// If not enough time to trigger autoplay
		if (_spinButtonHeld) {
			_holdTimerRemaining = 0f;
			_spinButtonHeld = false;

			if (_autoplayActive) {
				SwitchAutoplay();
			}

			if (!_spinning) {
				Spin();
			}
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		}
	}

	public void RotationEnded() {
		AudioManager.Instance.PauseBGM();
		CheckForWinnings();
//		ContinueSpinning();
	}

	public void AutoStartSpin() {
		if (_patternManager.HasWinningPattern) {
			Invoke("Spin", AUTOSPIN_WIN_DELAY);
		} 
		else {
			Spin();
		}
	}

	public void AddCoinBet() {
		if (_autoplayActive || _spinning)
			return;

		if (GetBetAmount(_currentCoinBetIndex + 1) > GameDataManager.Instance.LevelInfo.MaxCoinBet) {
			return;
		}

		if (GetBetAmount(_currentCoinBetIndex + 1) * _linesActive > PlayerDataManager.Instance.Chips) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
		_currentCoinBetIndex++;
		UpdateBetAmount();
	}

	public void DecreaseCoinBet() {
		if (_autoplayActive || _spinning)
			return;

		if (_currentCoinBetIndex <= 0) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_DECREASE);
		_currentCoinBetIndex--;
		UpdateBetAmount();
	}

	public void AddLines() {
		if (_autoplayActive || _spinning)
			return;

		if (GameDataManager.Instance == null) {
			Debug.LogWarning("Data manager not yet loaded");
			return;
		}
		
		if (_linesActive >= _patternManager.MaxLinesAllowed()) {
			return;
		}

		if (_currentCoinBet * (_linesActive + 1) > PlayerDataManager.Instance.Chips) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(SignalManager.PARAM_PATTERNCOUNT, _linesActive + 1);
		SignalManager.Instance.CallWithParam(SignalType.PATTERN_UPDATED, param);
	}

	public void DecreaseLines() {
		if (_autoplayActive || _spinning)
			return;

		if (_linesActive <= 1) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_DECREASE);
		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(SignalManager.PARAM_PATTERNCOUNT, _linesActive - 1);
		SignalManager.Instance.CallWithParam(SignalType.PATTERN_UPDATED, param);
	}

	public void AddBooster() {
		if (_autoplayActive || _spinning)
			return;

		if (PlayerDataManager.Instance == null) {
			Debug.LogWarning("Data manager not yet loaded");
			return;
		}

		if (_boostersToUse >= PlayerDataManager.Instance.Points) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
		_boostersToUse++;
		UpdateBoostersUsed();
	}

	public void DecreaseBooster() {
		if (_autoplayActive || _spinning)
			return;

		if (_boostersToUse <= 0) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_DECREASE);
		_boostersToUse--;
		UpdateBoostersUsed();
	}

	public void UseMaxLines() {
		if (_autoplayActive || _spinning)
			return;

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(SignalManager.PARAM_PATTERNCOUNT, _patternManager.MaxLinesAllowed());
		SignalManager.Instance.CallWithParam(SignalType.PATTERN_UPDATED, param);

		StartCoroutine(SpinAfterMaxLine());
	}

	public IEnumerator SpinAfterMaxLine() {
		if (_totalBet > PlayerDataManager.Instance.Chips) {
			yield break;
		}

		yield return null;
		if (!_spinning) {
			Spin();
		}
	}

	public void ShowBetTable() {
		if (_autoplayActive || _spinning)
			return;

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		_paytable.Show();
	}

	private void Spin() {
		if (_freeSpinsActive <= 0 && _totalBet > PlayerDataManager.Instance.Chips) {
			_notEnoughCoins.Show();
		} else if (_freeSpinsActive <= 0 && _boostersToUse > PlayerDataManager.Instance.Points) {
			_notEnoughBoosters.Show();
		} 
		else {
			_patternManager.HideWinningPattern ();

			_spinning = true;

			ConcreteSignalParameters stateParam = new ConcreteSignalParameters ();
			stateParam.AddParameter (PARAM_LOCK, (_autoplayActive || _spinning));
			SignalManager.Instance.CallWithParam (SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);

			StartCoroutine(DelayedSpin((_freeSpinsActive > 0) ? 0.01f : 1.8f));

			if (_freeSpinsActive > 0) {
				_freeSpinsActive--;
				_freeSpinsText.text = _freeSpinsActive.ToString ();

				if (_freeSpinsActive <= 0) {
					_remainingFreeSpins.Hide ();
				}
			} 
			else {
				PlayerDataManager.Instance.UseChips (Mathf.FloorToInt (_totalBet));
				PlayerDataManager.Instance.UsePoints (_boostersToUse);
			}

			AudioManager.Instance.ResumeBGM();
		}
	}

	private IEnumerator DelayedSpin(float spinTime) {
		SlotSegment.RotationEndDelegate finishedDelegate = RotationEnded;
		for (int x = _slots.Length - 1; x >= 0; x--) {
			_slots[x].SetRotationEnd(finishedDelegate);
			finishedDelegate = _slots[x].StartStopping;
		}

		for (int i = 0; i < _slots.Length; i++) {
			bool useLongerRotation = false;

			if (_freeSpinsActive <= 0) {
				if (i == 3) {
					useLongerRotation = (Random.Range(0f, 100f) > 90f);
				}

				if (i == 4) {
					useLongerRotation = (Random.Range(0f, 100f) > 75f);
				}
			}

			_slots[i].Rotate(useLongerRotation);
		}

		yield return new WaitForSeconds(spinTime);

		finishedDelegate();
	}

	private void SwitchAutoplay() {
		_autoplayActive = !_autoplayActive;
		_spinButtonSprite.SetSprite(_autoplayActive ? BUTTON_AUTOPLAY_ACTIVE : BUTTON_AUTOPLAY_INACTIVE);

		ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
		stateParam.AddParameter(PARAM_LOCK, (_autoplayActive || _spinning));
		SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);

		if (!_spinning) {
			Spin();
		}
	}

	private void CheckForWinnings() {
		if (_patternManager.FreeSpinsBonus.totalReward == 0) {
			PlayerDataManager.Instance.AddExp (Mathf.FloorToInt (_totalBet));
		}

		Invoke("DelayedCheckForWinnings", 0.2f);
	}

	private void DelayedCheckForWinnings() {
		_patternManager.RegisterPatterns();
		
		_previousWinnings = _patternManager.GetTotalWinnings(Mathf.FloorToInt(_currentCoinBet) * (_boostersToUse + 1));
		if (_previousWinnings == 0) {
			_previousWinningsText.text = "0";
		} 
		else {
			_previousWinningsText.text = _previousWinnings.ToString("#,#");
		}
		PlayerDataManager.Instance.AddChips(_previousWinnings);

		if ((float)_previousWinnings / _totalBet > 15.0f) {
			_bigWin.SetAmount(_previousWinnings);
			_extraRewardsWindow.Add(_bigWin);
		}

		if (_patternManager.ExtraBoostersBonus.totalReward > 0) {
			_boostersWin.SetAmount(_patternManager.ExtraBoostersBonus.totalReward);
			PlayerDataManager.Instance.AddPoints(_patternManager.ExtraBoostersBonus.totalReward);
			_extraRewardsWindow.Add(_boostersWin);
			Debug.Log ("Activating Extra Boosters!");
		}

		if (_patternManager.PendingSpinTheWheel.totalReward > 0) {
			_spinTheWheel.SetMultiplier(_patternManager.PendingSpinTheWheel.totalReward);

			_spinTheWheel.SetCoins(_currentCoinBet);

			_extraRewardsWindow.Add(_spinTheWheel);
			Debug.Log ("Activating Spin the wheel!");
		}

		if (_patternManager.FreeSpinsBonus.totalReward > 0) {
			_freeSpins.SetAmount(_patternManager.FreeSpinsBonus.totalReward);

			if (_freeSpinsActive <= 0) {
				_remainingFreeSpins.Show();
			}

			_freeSpinsActive += _patternManager.FreeSpinsBonus.totalReward;
			_freeSpinsText.text = _freeSpinsActive.ToString();
			_extraRewardsWindow.Add(_freeSpins);
			Debug.Log ("Activating Free Spins!");
		}
		/*
		if (_boostersToUse > PlayerDataManager.Instance.Boosters) {
			_boostersToUse = PlayerDataManager.Instance.Boosters;
			UpdateBoostersUsed();
		}*/

		CheckForBonusWindows();
	}

	private void PlayWinningSound() {
		if ((float)_previousWinnings / _totalBet > 15f) {
//			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WIN_3);
		} 

		else if ((float)_previousWinnings / _totalBet > 2f) {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WIN_2);
		} 

		else if (_previousWinnings > 0) {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WIN_1);
		}
	}

	private void CheckForBonusWindows() {
		if (_extraPopupShown)
			return;

		if (_extraRewardsWindow.Count > 0) {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BONUS);
			_extraRewardsWindow[0].Show();
		} 
		else {
			PlayWinningSound();
			ContinueSpinning();
		}
	}

	public void Close(){
		_boostersToUse = 0;
		UpdateBoostersUsed();
		SpinAgain ();
	}

	void SpinAgain(){
		if(_wasAutoplaying)
			SwitchAutoplay ();
		_wasAutoplaying = false;

		ContinueSpinning();
	}

	private void ContinueSpinning() {
		_patternManager.RotateWinningPattern(_autoplayActive);

		if (_autoplayActive && (_totalBet > PlayerDataManager.Instance.Chips || _boostersToUse > PlayerDataManager.Instance.Points)) {
			if(_boostersToUse > PlayerDataManager.Instance.Points)
				_wasAutoplaying = true;

			SwitchAutoplay();
			_spinning = false;
			Spin();
		} 

		else if (_autoplayActive || _freeSpinsActive > 0) {
			Invoke("AutoStartSpin", AUTOSPIN_DELAY);
		} 

		else {
			_spinning = false;
		}

		ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
		stateParam.AddParameter(PARAM_LOCK, (_autoplayActive || _spinning));
		SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);
	}


	private void UpdateTotalBet() {
		_totalBet = _currentCoinBet * _linesActive;
		_totalBetText.text = _totalBet.ToString("#,#");
	}

	private void UpdateBetAmount() {
		_currentCoinBet = GetBetAmount(_currentCoinBetIndex);
		PlayerDataManager.Instance.LastBet = _currentCoinBetIndex;
		_coinBetText.text = _currentCoinBet.ToString("#,#");
		UpdateTotalBet();
	}

	private float GetBetAmount(int index) {
		if (index > _betAmounts.Length - 1) {
			return _betAmounts[_betAmounts.Length - 1] + ((index - (_betAmounts.Length - 1)) * 250);
		} 

		else {
			return _betAmounts[index]; 
		}
	}

	private void UpdateLinesActive() {
		_lineBetText.text = _linesActive.ToString();
		UpdateTotalBet();
	}

	private void UpdateBoostersUsed() {
		_boosterBetText.text = _boostersToUse.ToString();
	}


	public void Execute(SignalType type, ISignalParameters param) {
		switch(type) {
		case SignalType.PATTERN_UPDATED:
			_linesActive = (int)param.GetParameter(SignalManager.PARAM_PATTERNCOUNT);
			PlayerDataManager.Instance.LastPattern = _linesActive;
			UpdateLinesActive();
			break;

		case SignalType.EXTRA_REWARD_CLOSED:
			_extraRewardsWindow.RemoveAt(0);
			CheckForBonusWindows();
			break;

		case SignalType.LEVELUPWINDOW_CLOSED:
			_extraPopupShown = false;

			CheckForBonusWindows();
			break;

		case SignalType.LEVELED_UP:
			_extraPopupShown = true;

			SetPatternUsed();

			if (_autoplayActive) {
				SwitchAutoplay();
			}
			
			// Set to Max Lines
			ConcreteSignalParameters patternParam = new ConcreteSignalParameters();
			patternParam.AddParameter(SignalManager.PARAM_PATTERNCOUNT, _patternManager.MaxLinesAllowed());
			SignalManager.Instance.CallWithParam(SignalType.PATTERN_UPDATED, patternParam);

			// Set to Max Bet
			while (GetBetAmount(_currentCoinBetIndex + 1) <= GameDataManager.Instance.LevelInfo.MaxCoinBet) {
				_currentCoinBetIndex++;
			}
			UpdateBetAmount();

			break;
		}
	}
}
