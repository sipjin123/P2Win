using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour, ISignalListener {

	private static PlayerDataManager _instance;
	public static PlayerDataManager Instance { get { return _instance; } }

	private const string PREF_COINS = "PLAYER_COINS";
	private const string PREF_BOOSTERS = "PLAYER_BOOSTERS";
	private const string PREF_LEVEL = "PLAYER_LEVEL";
	private const string PREF_EXP = "PLAYER_EXP";
	private const string PREF_USE_BGM = "SETTINGS_USE_BGM";
	private const string PREF_USE_SFX = "SETTINGS_USE_SFX";
	private const string PREF_BONUS_LASTUSED = "PLAYER_LASTBONUS_USED";

	private const string PREF_LASTBET = "SYSTEM_LASTBET";
	private const string PREF_LASTPATTERN = "SYSTEM_LASTPATTERN";

	private const string PREF_TOTALCOINSEARNED = "TOTAL_COINS_EARNED";

	private const string DEFAULT_TIMESTRING_VALUE = "[EMPTY]";
	private const int BONUS_AMOUNT = 200;

	private TimeSpan BONUS_TIME_DURATION;

	private float _totalCoinsEarned;
	private float _coins = 0;
	private int _boosters = 0;
	private int _level = 1;
	private float _exp = 0f;
	private bool _useBGM = true;
	private bool _useSFX = true;
	private DateTime _bonusTime;

	private int _lastBet;
	private int _lastPattern;

	public List<int> _boughtIAPLevels;

	public float Coins { 
		get { return _coins; } 
	}
	
	public int Boosters { 
		get { return _boosters; } 
	}

	public int Level { 
		get { return _level; } 
	}

	public float Experience { 
		get { return _exp; } 
	}

	public bool UseBGM { 
		get { return _useBGM; } 
	}

	public bool UseSFX { 
		get { return _useSFX; } 
	}

	public int LastBet { 
		get { return _lastBet; } 
		set { _lastBet = value; } 
	}

	public int LastPattern { 
		get { return _lastPattern; } 
		set { _lastPattern = value; } 
	}

	public TimeSpan BonusTimeLeft { 
		get { 
			if (BonusAvailable)
				return TimeSpan.Zero;
			else
				return BONUS_TIME_DURATION.Subtract(DateTime.Now.Subtract(_bonusTime));
		} 
	}
	public string BonusTimeLeftString { 
		get {
			TimeSpan ret = BonusTimeLeft;
			return string.Format("{0:D2}:{1:D2}:{2:D2}", ret.Hours, ret.Minutes, ret.Seconds);
		} 
	}
	public bool BonusAvailable { get { return BONUS_TIME_DURATION <= DateTime.Now.Subtract(_bonusTime); } }

	public float ExpRatio { 
		get { 
			if (_level >= GameDataManager.Instance.MaxLevel) {
				return 1f;
			} else {
				return (_exp - GameDataManager.Instance.GetCurrentBaseEXP()) / (GameDataManager.Instance.LevelInfo.ExpToNextLevel - GameDataManager.Instance.GetCurrentBaseEXP()); 
			}
		} 
	}

	void Start() {

		_instance = this;
		BONUS_TIME_DURATION = TimeSpan.FromHours(1);
		LoadAllData();

		_boughtIAPLevels = new List<int>();
	}

	public void InitializeListener() {
		SignalManager.Instance.Register(this, SignalType.LOCAL_DATA_CHANGED);
	}

	private void LoadAllData() {

		_totalCoinsEarned = PlayerPrefs.GetFloat (PREF_TOTALCOINSEARNED);
		_coins = PlayerPrefs.GetFloat(PREF_COINS, 300f);
		_boosters = PlayerPrefs.GetInt(PREF_BOOSTERS, 5);
		_level = PlayerPrefs.GetInt(PREF_LEVEL, 1);
		_exp = PlayerPrefs.GetFloat(PREF_EXP, 0f);
		_useBGM = (PlayerPrefs.GetInt(PREF_USE_BGM, 1) == 1);
		_useSFX = (PlayerPrefs.GetInt(PREF_USE_SFX, 1) == 1);
		_lastBet = PlayerPrefs.GetInt(PREF_LASTBET, 0);
		_lastPattern = PlayerPrefs.GetInt(PREF_LASTPATTERN, 1);


		if (_level < 1) {
			_level = 1;
		} 
		else if (_level > GameDataManager.Instance.MaxLevel) {
			_level = GameDataManager.Instance.MaxLevel;
		}

		string timeString = PlayerPrefs.GetString(PREF_BONUS_LASTUSED, DEFAULT_TIMESTRING_VALUE);

		if (timeString == DEFAULT_TIMESTRING_VALUE) {
			_bonusTime = DateTime.Now; //DateTime.MinValue;
		} 
		else {
			_bonusTime = DateTime.FromBinary(Convert.ToInt64(timeString));
		}
	}

	private void SaveAllData() {
		PlayerPrefs.SetFloat(PREF_TOTALCOINSEARNED, _totalCoinsEarned);
		PlayerPrefs.SetFloat(PREF_COINS, _coins);
		PlayerPrefs.SetInt(PREF_BOOSTERS, _boosters);
		PlayerPrefs.SetInt(PREF_LEVEL, _level);
		PlayerPrefs.SetFloat(PREF_EXP, _exp);
		PlayerPrefs.SetInt(PREF_USE_BGM, (_useBGM ? 1 : 0));
		PlayerPrefs.SetInt(PREF_USE_SFX, (_useSFX ? 1 : 0));
		PlayerPrefs.SetInt(PREF_LASTBET, _lastBet);
		PlayerPrefs.SetInt(PREF_LASTPATTERN, _lastPattern);
		PlayerPrefs.SetString(PREF_BONUS_LASTUSED, _bonusTime.ToBinary().ToString());

		PlayerPrefs.Save();
	}


	public void AddCoins(int amount) {
		_coins += amount;
		_totalCoinsEarned += amount;
		setBalance(true);
		
		SignalManager.Instance.Call(SignalType.PGS_UPDATE_LEADERBOARD);
		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
		//SignalManager.Instance.Call(SignalType.PARSE_UPDATE);
	}

	public void UseCoins(int amount) {
		_coins -= amount;
		setBalance(true);

		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
		//SignalManager.Instance.Call(SignalType.PARSE_UPDATE);
	}

	public void UseBoosters(int amount) {
		_boosters -= amount;
		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
	}

	public void AddExp(int amount) {
		if (_level < GameDataManager.Instance.MaxLevel) {
			_exp += amount;
			if (_exp >= GameDataManager.Instance.LevelInfo.ExpToNextLevel) {
				setLevel(true);
				_level++;
				SignalManager.Instance.Call(SignalType.LEVELED_UP);
			} 
			else {
				SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
			}
			//SignalManager.Instance.Call(SignalType.PARSE_UPDATE);
		}
	}

	public void AddBoosters(int amount) {
		_boosters += amount;
		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
	}

	public void ToggleBGM() {
		_useBGM = !_useBGM;
		SignalManager.Instance.Call(SignalType.SETTINGS_CHANGED);
	}

	public void ToggleSFX() {
		_useSFX = !_useSFX;
		SignalManager.Instance.Call(SignalType.SETTINGS_CHANGED);
	}

	public void CollectBonus() {
		if (BonusAvailable) {
			_bonusTime = DateTime.Now;
			AddCoins(BONUS_AMOUNT);
		}
	}
	public float getTotalCoinsEarned() {
		return _totalCoinsEarned;
	}

	public void SetCoinUpdate(float p_coin) {
		_coins = p_coin;
	}

	public void SetLevelUpdate(int p_level) {
		_level = p_level;
	}
	// Local Data Changed - signal listener
	public void Execute(SignalType type, ISignalParameters param) {
		SaveAllData();
	}


	//---- parse manager
	bool balanceChanged = true;
	bool levelChanged = true;
	
	public bool getBalance(){
		return balanceChanged;
	}
	
	public bool getLevel(){
		return levelChanged;
	}
	
	public void setBalance(bool value){
		balanceChanged = value;
	}

	public void setLevel(bool value){
		levelChanged = value;
	}
}
