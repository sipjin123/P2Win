using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour, ISignalListener {

	private static PlayerDataManager _instance;
	public static PlayerDataManager Instance { get { return _instance; } }

	private const string PREF_CHIPS = "PLAYER_CHIPS";
	private const string PREF_POINTS = "PLAYER_POINTS";
	private const string PREF_LEVEL = "PLAYER_LEVEL";
	private const string PREF_EXP = "PLAYER_EXP";
	private const string PREF_USE_BGM = "SETTINGS_USE_BGM";
	private const string PREF_USE_SFX = "SETTINGS_USE_SFX";
	private const string PREF_BONUS_LASTUSED = "PLAYER_LASTBONUS_USED";
	private const string PREF_LAST_LOGIN = "PLAYER_LAST_LOGIN";
	private const string PREF_LAST_BONUS_SPIN = "PLAYER_LAST_BONUS_SPIN";
	private const string PREF_LOGIN_BONUS = "PLAYER_LOGIN_BONUS";

	private const string PREF_LASTBET = "SYSTEM_LASTBET";
	private const string PREF_LASTPATTERN = "SYSTEM_LASTPATTERN";

	private const string PREF_TOTALCHIPSEARNED = "TOTAL_CHIPS_EARNED";

	private const string DEFAULT_TIMESTRING_VALUE = "[EMPTY]";
	private const int BONUS_AMOUNT = 200;

	private TimeSpan BONUS_TIME_DURATION;

	private float _totalChipsEarned;
	private float _chips = 0;
	private int _points = 0;
	private int _level = 1;
	private float _exp = 0f;
	private bool _useBGM = true;
	private bool _useSFX = true;
	private DateTime _bonusTime;
	private string _lastLogIn;
	private string _lastBonusSpin;

	private int _lastBet;
	private int _lastPattern;
	private int _logInBonus;

	public List<int> _boughtIAPLevels;

	public float Chips { 
		get { return _chips; } 
	}
	
	public int Points { 
		get { return _points; } 
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

	public DateTime SpinBonusTimeLeft{
		get{return DateTime.Parse(_lastBonusSpin);}
	}

	public DateTime LastUserLogin{
		get{return DateTime.Parse(_lastLogIn);}
	}

	public int LogInBonus{
		get{return _logInBonus;}
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

		_totalChipsEarned = PlayerPrefs.GetFloat (PREF_TOTALCHIPSEARNED);
		_chips = PlayerPrefs.GetFloat(PREF_CHIPS, 300f);
		_points = PlayerPrefs.GetInt(PREF_POINTS, 5);
		_level = PlayerPrefs.GetInt(PREF_LEVEL, 1);
		_exp = PlayerPrefs.GetFloat(PREF_EXP, 0f);
		_lastBonusSpin = PlayerPrefs.GetString (PREF_LAST_BONUS_SPIN,DateTime.Now.ToString());
		_useBGM = (PlayerPrefs.GetInt(PREF_USE_BGM, 1) == 1);
		_useSFX = (PlayerPrefs.GetInt(PREF_USE_SFX, 1) == 1);
		_lastBet = PlayerPrefs.GetInt(PREF_LASTBET, 0);
		_lastPattern = PlayerPrefs.GetInt(PREF_LASTPATTERN, 1);
		_lastBonusSpin = PlayerPrefs.GetString (PREF_LAST_BONUS_SPIN, DateTime.Today.AddDays (-1).ToString());
		_lastLogIn = PlayerPrefs.GetString (PREF_LAST_LOGIN, DateTime.Today.ToString());
		_logInBonus = PlayerPrefs.GetInt (PREF_LOGIN_BONUS, 1);


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
		PlayerPrefs.SetFloat(PREF_TOTALCHIPSEARNED, _totalChipsEarned);
		PlayerPrefs.SetFloat(PREF_CHIPS, _chips);
		PlayerPrefs.SetInt(PREF_POINTS, _points);
		PlayerPrefs.SetInt(PREF_LEVEL, _level);
		PlayerPrefs.SetFloat(PREF_EXP, _exp);
		PlayerPrefs.SetInt(PREF_USE_BGM, (_useBGM ? 1 : 0));
		PlayerPrefs.SetInt(PREF_USE_SFX, (_useSFX ? 1 : 0));
		PlayerPrefs.SetInt(PREF_LASTBET, _lastBet);
		PlayerPrefs.SetInt(PREF_LASTPATTERN, _lastPattern);
		PlayerPrefs.SetString(PREF_BONUS_LASTUSED, _bonusTime.ToBinary().ToString());
		PlayerPrefs.SetString(PREF_LAST_LOGIN, _lastLogIn.ToString());
		PlayerPrefs.SetString (PREF_LAST_BONUS_SPIN, _lastBonusSpin.ToString ());
		PlayerPrefs.SetInt (PREF_LOGIN_BONUS, _logInBonus);

		PlayerPrefs.Save();
	}

	public void lastLogin(DateTime p_prevLogin){
		_lastLogIn = p_prevLogin.ToString();
		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
	}
	public void lastBonusSpin (DateTime p_lastBonusSpin){
		_lastBonusSpin = p_lastBonusSpin.ToString();
		SignalManager.Instance.Call (SignalType.LOCAL_DATA_CHANGED); 
	}

	public void curLoginBonus(int p_loginBonus){
		_logInBonus = p_loginBonus;
	}

	public void AddChips(int amount) {
		_chips += amount;
		_totalChipsEarned += amount;
		
		SignalManager.Instance.Call(SignalType.PGS_UPDATE_LEADERBOARD);
		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
		//SignalManager.Instance.Call(SignalType.PARSE_UPDATE);
	}

	public void UseChips(int amount) {
		_chips -= amount;

		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
		//SignalManager.Instance.Call(SignalType.PARSE_UPDATE);
	}

	public void UsePoints(int amount) {
		_points -= amount;
		SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
	}

	public void AddExp(int amount) {
		if (_level < GameDataManager.Instance.MaxLevel) {
			_exp += amount;
			if (_exp >= GameDataManager.Instance.LevelInfo.ExpToNextLevel) {
				_level++;
				SignalManager.Instance.Call(SignalType.LEVELED_UP);
			} 
			else {
				SignalManager.Instance.Call(SignalType.LOCAL_DATA_CHANGED);
			}
			//SignalManager.Instance.Call(SignalType.PARSE_UPDATE);
		}
	}

	public void AddPoints(int amount) {
		_points += amount;
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
			AddChips(BONUS_AMOUNT);
		}
	}
	public float getTotalChipsEarned() {
		return _totalChipsEarned;
	}

	public void SetLevelUpdate(int p_level) {
		_level = p_level;
	}
	// Local Data Changed - signal listener
	public void Execute(SignalType type, ISignalParameters param) {
		SaveAllData();
	}

}
