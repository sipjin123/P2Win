using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PatternSet {
	MAX_9,
	MAX_15,
	MAX_25
}

public struct PatternReward {
	public List<SlotItem> items;
	public int totalReward;
	public bool isGlobal;

	public PatternReward(bool global) {
		items = new List<SlotItem>();
		totalReward = 0;
		isGlobal = global;
	}
}

public class PatternManager : MonoBehaviour, ISignalListener {

	[SerializeField]
	private PatternObjectSet _patternMax9_15;

	[SerializeField]
	private PatternObjectSet _patternMax25;

	
	[SerializeField]
	private PatternButtonSet _buttonsMax9;

	[SerializeField]
	private PatternButtonSet _buttonsMax15;

	[SerializeField]
	private PatternButtonSet _buttonsMax25;

	[SerializeField]
	private SlotItemDetectorManager _slotItemDetectorManager;

	private Dictionary<PatternSet, PatternObjectSet> _patternObjectList;
	private Dictionary<PatternSet, PatternButtonSet> _patternButtonList;

	private PatternSet _activePattern = PatternSet.MAX_9;

	private PatternReward _extraWildBonus;
	private PatternReward _pendingSpinTheWheel;
	private PatternReward _freeSpinsBonus;
	private PatternReward _extraBoostersBonus;

	public bool HasWinningPattern { get { return _patternObjectList[_activePattern].WinningPatternCount > 0; } }
		
	public PatternReward ExtraWildBonus { get { return _extraWildBonus; } }
	public PatternReward PendingSpinTheWheel { get { return _pendingSpinTheWheel; } }
	public PatternReward FreeSpinsBonus { get { return _freeSpinsBonus; } }
	public PatternReward ExtraBoostersBonus { get { return _extraBoostersBonus; } }

	private bool _inputLocked = false;

	public void Initialize() {
		InitializeDictionaries();

		SignalManager.Instance.Register(this, SignalType.PATTERN_UPDATED);
		SignalManager.Instance.Register(this, SignalType.SLOT_MACHINE_STATE_CHANGED);
	}

	void Destroy() {
		SignalManager.Instance.ClearRegisterType(SignalType.PATTERN_UPDATED);
		SignalManager.Instance.Remove(this, SignalType.SLOT_MACHINE_STATE_CHANGED);
	}

	public void SetActivePattern(PatternSet pattern) {
        //_activePattern = pattern;
        _activePattern = PatternSet.MAX_25;
		LoadPatternObject(_activePattern);
		LoadPatternButtons(_activePattern);
	}


	public void RegisterPatterns() {
		_patternObjectList[_activePattern].RegisterWinningPatterns();

		_extraWildBonus = _slotItemDetectorManager.CheckWildExtraReward();
		_pendingSpinTheWheel = _slotItemDetectorManager.CheckForSpinTheWheel();
		_freeSpinsBonus = _slotItemDetectorManager.CheckScatterReward();
		_extraBoostersBonus = _slotItemDetectorManager.CheckBoosterReward();
	}

	public int GetTotalWinnings(int coinsBet) {
		return _patternObjectList[_activePattern].GetTotalWinnings(coinsBet);
	}
	

	/***
	 * Winning pattern display
	 ***/

	public void RotateWinningPattern(bool autoplayActive) {
		if (autoplayActive) {
			_patternObjectList[_activePattern].ShowAllWinningPatterns();
		} else {
			_patternObjectList[_activePattern].ShowWinningPatterns();
		}
	}

	public void HideWinningPattern() {
		_patternObjectList[_activePattern].StopShowingWinningPatterns();
	}


	/**
	 * Pattern Updates
	 **/

	public int MaxLinesAllowed() {
        return GetLineInt(PatternSet.MAX_25);
        //return GetLineInt(_activePattern);
	}

	public int GetLineInt(PatternSet pattern) {
		switch(pattern) {
		case PatternSet.MAX_9:
			return 9;
			
		case PatternSet.MAX_15:
			return 15;
			
		case PatternSet.MAX_25:
			return 25;
			
		default:
			return 9;
		}
	}

	public void OnLineButtonClicked(tk2dUIItem source) {

		if (_inputLocked) {
			return;
		}

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);

		PatternButton buttonSource = source.GetComponent<PatternButton>();

		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(SignalManager.PARAM_PATTERNCOUNT, buttonSource.LineNumber);
		SignalManager.Instance.CallWithParam(SignalType.PATTERN_UPDATED, param);
	}

	private void UpdateLinesToUse(int linesToUse) {
		if (linesToUse > MaxLinesAllowed()) {
			linesToUse = MaxLinesAllowed();
		}

		_patternObjectList[_activePattern].SetActiveLines(linesToUse);
	}

	/**
	 * Initializations
	 **/

	private void InitializeDictionaries() {
		InitializePatternObjects();
		InitializePatternButtons();
	}
	
	private void InitializePatternObjects() {
		_patternObjectList = new Dictionary<PatternSet, PatternObjectSet>();
		_patternObjectList.Add(PatternSet.MAX_9, _patternMax9_15);
		_patternObjectList.Add(PatternSet.MAX_15, _patternMax9_15);
		_patternObjectList.Add(PatternSet.MAX_25, _patternMax25);
	}
	
	private void InitializePatternButtons() {
		_patternButtonList = new Dictionary<PatternSet, PatternButtonSet>();
		_patternButtonList.Add(PatternSet.MAX_9, _buttonsMax9);
		_patternButtonList.Add(PatternSet.MAX_15, _buttonsMax15);
		_patternButtonList.Add(PatternSet.MAX_25, _buttonsMax25);
	}

	private void LoadPatternObject(PatternSet pattern) {
		HideAllPatternObject();
		_patternObjectList[pattern].Activate();
	}

	private void LoadPatternButtons(PatternSet pattern) {
		HideAllPatternButtons();
        //_patternButtonList[pattern].Activate();
	}

	private void HideAllPatternObject() {
		_patternMax9_15.Deactivate();
		_patternMax25.Deactivate();
	}

	private void HideAllPatternButtons() {
		_buttonsMax9.Deactivate();
		_buttonsMax15.Deactivate();
		_buttonsMax25.Deactivate();
	}

	public void Execute(SignalType type, ISignalParameters param) {
		switch(type) {
		case SignalType.PATTERN_UPDATED:
			UpdateLinesToUse((int)param.GetParameter(SignalManager.PARAM_PATTERNCOUNT));
			break;
		case SignalType.SLOT_MACHINE_STATE_CHANGED:
			_inputLocked = (bool)param.GetParameter(SlotMachineScene.PARAM_LOCK);
			break;
		}
	}

}
