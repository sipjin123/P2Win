using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SignalType {
	DEFAULT,
	LOCAL_DATA_CHANGED,
	GAME_ENTERED, // NOTE: Confirm if this is no longer needed, check if it breaks anything if removed
    LOBBY_ENTERED,  // NOTE: Confirm if this is no longer needed, check if it breaks anything if removed
	PATTERN_UPDATED,
	LEVELED_UP,
	EXTRA_REWARD_CLOSED,
	SETTINGS_CHANGED,
	SHOW_SETTINGS,
	SETTINGS_CLOSED,
	LEVELUPWINDOW_CLOSED,
	PGS_LOGIN,
	PGS_SHOW_LEADERBOARD,
	PGS_UPDATE_LEADERBOARD,
	FB_USER_LOGGEDIN,
	PARSE_DOWNLOAD,
	PARSE_UPDATE,
	BUTTON_MORE_COINS,
	BUTTON_MORE_BOOSTERS,
	BUTTON_MORE_50COINS,
	BUTTON_MORE_DOWNLOADCOINS,
	LEVEL_DOWNLOAD_FINISHED,
	LEVEL_DOWNLOAD_PROGRESS,
	SLOT_MACHINE_STATE_CHANGED,
	MESSAGE_PROMPT_OK_ONLY,
	MESSAGE_PROMPT_BUY,
	MESSAGE_PROMPT_LEADERBOARDS,
	PURCHASE_FROM_PROMPT,
    UPDATE_PROFILE_HUD, // This should replace GAME_ENTERED and LOBBY_ENTERED
    REWARD_ITEM_BOUGHT,
	HIDE_SLOT_ITEM,
	RETURN_SLOT_ITEM,
	UPDATE_SETTINGSBTN_SPRITE // Added for tigerslots settings button
}

public class SignalManager : MonoBehaviour {

	public const string PARAM_PATTERNCOUNT = "PARAM_PATTERNCOUNT";

	private static SignalManager _instance;
	public static SignalManager Instance { get { return _instance; } }

	private Dictionary<SignalType, List<ISignalListener>> _listeners;

	void Start() {
		_instance = this;
		InitializeListenerDictionary();
	}

	private void InitializeListenerDictionary() {
		_listeners = new Dictionary<SignalType, List<ISignalListener>>();
		InitializeListenerType(SignalType.DEFAULT);
		InitializeListenerType(SignalType.LOCAL_DATA_CHANGED);
		InitializeListenerType(SignalType.GAME_ENTERED);
		InitializeListenerType(SignalType.LOBBY_ENTERED);
		InitializeListenerType(SignalType.PATTERN_UPDATED);
		InitializeListenerType(SignalType.LEVELED_UP);
		InitializeListenerType(SignalType.EXTRA_REWARD_CLOSED);
		InitializeListenerType(SignalType.SETTINGS_CHANGED);
		InitializeListenerType(SignalType.SHOW_SETTINGS);
		InitializeListenerType(SignalType.SETTINGS_CLOSED);
		InitializeListenerType(SignalType.LEVELUPWINDOW_CLOSED);
		InitializeListenerType(SignalType.PGS_LOGIN);
		InitializeListenerType(SignalType.PGS_SHOW_LEADERBOARD);
		InitializeListenerType(SignalType.PGS_UPDATE_LEADERBOARD);
		InitializeListenerType(SignalType.FB_USER_LOGGEDIN);
		InitializeListenerType(SignalType.PARSE_DOWNLOAD);
		InitializeListenerType(SignalType.PARSE_UPDATE);
		InitializeListenerType(SignalType.BUTTON_MORE_COINS);
		InitializeListenerType(SignalType.BUTTON_MORE_BOOSTERS);
		InitializeListenerType(SignalType.BUTTON_MORE_50COINS);
		InitializeListenerType(SignalType.BUTTON_MORE_DOWNLOADCOINS);
		InitializeListenerType(SignalType.LEVEL_DOWNLOAD_FINISHED);
		InitializeListenerType(SignalType.LEVEL_DOWNLOAD_PROGRESS);
		InitializeListenerType(SignalType.SLOT_MACHINE_STATE_CHANGED);
		InitializeListenerType(SignalType.MESSAGE_PROMPT_OK_ONLY);
		InitializeListenerType(SignalType.MESSAGE_PROMPT_BUY);
		InitializeListenerType(SignalType.MESSAGE_PROMPT_LEADERBOARDS);
		InitializeListenerType(SignalType.PURCHASE_FROM_PROMPT);
        InitializeListenerType(SignalType.UPDATE_PROFILE_HUD);
        InitializeListenerType(SignalType.REWARD_ITEM_BOUGHT);
		InitializeListenerType(SignalType.HIDE_SLOT_ITEM);
		InitializeListenerType (SignalType.RETURN_SLOT_ITEM);
		InitializeListenerType (SignalType.UPDATE_SETTINGSBTN_SPRITE); // Added for tigerslots settings button
	}

	private void InitializeListenerType(SignalType type) {
		_listeners.Add(type, new List<ISignalListener>());
	}



	public void Register(ISignalListener listener, SignalType type) {
		if (_listeners[type].Contains(listener)) {
			return;
		}

		_listeners[type].Add(listener);
	}

	public void ClearRegisterType(SignalType type) {
		_listeners[type].Clear();
	}

	public void Remove(ISignalListener listener, SignalType type) {
		if (!_listeners[type].Contains(listener)) {
			return;
		}
		_listeners[type].Remove(listener);
	}

	public void Call(SignalType type) {
		foreach(ISignalListener listener in _listeners[type]) {
			listener.Execute(type, null);
		}
	}

	public void CallWithParam(SignalType type, ISignalParameters param) {
		foreach(ISignalListener listener in _listeners[type]) {
			listener.Execute(type, param);
		}
	}
}
