using UnityEngine;
using System.Collections;

public class LobbyPrompt : MonoBehaviour, ISignalListener {

	public static string LEVEL_REQUIRED_PARAM = "Level_Required";
	public static string COST_PARAM = "Cost";
	public static string LEVEL_PARAM = "Level";

	private static LobbyPrompt _instance;
	public static LobbyPrompt Instance { get { return _instance; } }

	private const string MESSAGE_LOCKED_PREFIX = "To unlock the ";
	private const string MESSAGE_LOCKED_SUFFIX = " stage,\nyou need to reach level ";
	private const string MESSAGE_COST_PREFIX = "Purchase ";
	private const string MESSAGE_COST_SUFFIX = " Stage for ";

	[SerializeField]
	private tk2dTextMesh _message;

	[SerializeField]
	private GameObject _okOnlyParent;

	[SerializeField]
	private GameObject _buyParent;

	private int _levelSelected;

	void Awake() {
		_instance = this;
	}

	void Start() {
		SignalManager.Instance.Register(this, SignalType.MESSAGE_PROMPT_OK_ONLY);
		SignalManager.Instance.Register(this, SignalType.MESSAGE_PROMPT_BUY);
		SignalManager.Instance.Register(this, SignalType.MESSAGE_PROMPT_LEADERBOARDS);

		gameObject.SetActive(false);
	}

	void OnDestroy() {
		SignalManager.Instance.Remove(this, SignalType.MESSAGE_PROMPT_OK_ONLY);
		SignalManager.Instance.Remove(this, SignalType.MESSAGE_PROMPT_BUY);
		SignalManager.Instance.Remove(this, SignalType.MESSAGE_PROMPT_LEADERBOARDS);
	}

	public void OnClose() {
		gameObject.SetActive(false);
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	public void OnBuy() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		ConcreteSignalParameters param = new ConcreteSignalParameters();
		param.AddParameter(LEVEL_PARAM, _levelSelected);
		SignalManager.Instance.CallWithParam(SignalType.PURCHASE_FROM_PROMPT, param);
		gameObject.SetActive(false);
	}

	private string Ordinal(int num) {
		string suffix = "";
		int ones = num % 10;
		int tens = Mathf.FloorToInt(num/10) % 10;
		if (tens == 1) {
			suffix = "th";
		} else {
			switch (ones) {
			case 1 : suffix = "st"; break;
			case 2 : suffix = "nd"; break;
			case 3 : suffix = "rd"; break;
			default : suffix = "th"; break;
			}
		}
		return num.ToString() + suffix;
	}

	public void Execute(SignalType type, ISignalParameters param) {
		switch (type) {
		case SignalType.MESSAGE_PROMPT_OK_ONLY:
			gameObject.SetActive(true);
			_levelSelected = (int)param.GetParameter(LEVEL_PARAM);
			_message.text = MESSAGE_LOCKED_PREFIX + Ordinal(_levelSelected) + MESSAGE_LOCKED_SUFFIX + (string)param.GetParameter(LEVEL_REQUIRED_PARAM);
			_okOnlyParent.SetActive(true);
			_buyParent.SetActive(false);
			break;
			
		case SignalType.MESSAGE_PROMPT_BUY:
			gameObject.SetActive(true);
			_levelSelected = (int)param.GetParameter(LEVEL_PARAM);
			_message.text = MESSAGE_COST_PREFIX + Ordinal(_levelSelected) + MESSAGE_COST_SUFFIX + (string)param.GetParameter(COST_PARAM);
			_okOnlyParent.SetActive(false);
			_buyParent.SetActive(true);
			break;

		case SignalType.MESSAGE_PROMPT_LEADERBOARDS:
			gameObject.SetActive(true);
			_message.text = "Game Center is Unavailable." + "\n" + "Player is not logged in.";
			_okOnlyParent.SetActive(true);
			_buyParent.SetActive(false);
			break;
		}
	}
}
