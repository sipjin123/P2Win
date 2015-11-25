using UnityEngine;
using System.Collections;

public class GameManager_ReelChef : MonoBehaviour {

	private static GameManager_ReelChef _instance;
	public static GameManager_ReelChef Instance { get { return _instance; } }


	public float SpinSpeed;
	public float SpinStrength;
	public tk2dTextMesh BetText;
	public tk2dTextMesh AutoSpinText;

	public int Score;
	public float BetCounter;
	public float BonusCounter;
	public float AutoSpinCounter;

	public bool LowerBarIsActive;
	public GameObject InfoWindowObject;
	public GameObject PlayTableObject;
	public GameObject LowerBarObject;
	public GameObject LowerBarObjectPosOn;
	public GameObject LowerBarObjectPosOff;
	public MeshRenderer PullLowerBarUp, PullLowerBarDown;
	public GameObject AutoSpinItems;
	public GameObject[] BonusHighlights;
	public GameObject[] Stars;
	
	public int Chips;
	public int Level;
	public int Gems;
	public int Exp;
	void Awake ()
	{
		_instance = this;
	}
	void Start()
	{
        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", ProfileUIType.BAR_FRENZY);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);

		Chips =  (int)PlayerPrefs.GetFloat("PLAYER_CHIPS");
		Level =  (int)PlayerPrefs.GetFloat("PLAYER_LEVEL");
		Gems =  (int)PlayerPrefs.GetFloat("PLAYER_POINTS",100);
		Exp =  (int)PlayerPrefs.GetFloat("PLAYER_EXP");
		Score = Chips;
		
		BetCounter = 1;
		BetText.text = "1";

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_INTRO);
	}
	public void AddScore(float _score)
	{
		LowerBarIsActive = false;
		PlayerDataManager.Instance.AddChips((int)_score);
	}
	public void AddExp(float _exp)
	{
		PlayerDataManager.Instance.AddExp((int)_exp);
	}
	public void AddBet(bool _ifADD)
	{

		if(_ifADD)
		{
			BetCounter ++;
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
		}
		else
		{
			BetCounter --;
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_DECREASE);
		}
		BetCounter = Mathf.Clamp(BetCounter , 1 , 20);
		BetText.text = ""+BetCounter;
	}
	public void ShowAutoSpinItems()
	{
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		if(AutoSpinItems.activeSelf == false)
			AutoSpinItems.SetActive(true);
		else
			AutoSpinItems.SetActive(false);
	}
	public void AutoSpinSet(int _counter)
	{
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		AutoSpinCounter = _counter;
		AutoSpinText.text = ""+AutoSpinCounter;
		ShowAutoSpinItems();
	}
	public void ShowLowerBar()
	{
		StartCoroutine(AnimateLowerBar(!LowerBarIsActive));
	}
	public IEnumerator AnimateLowerBar(bool isUP)
	{
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SPIN_TICK);
		if(isUP)
		{
			
			PullLowerBarUp.enabled = false;
			PullLowerBarDown.enabled = true;
			iTween.MoveTo(LowerBarObject,iTween.Hash(
				"x"   , LowerBarObjectPosOn.transform.position.x,
				"y"	,  LowerBarObjectPosOn.transform.position.y,
				"z"	, LowerBarObjectPosOn.transform.position.z,
				"time", 0.5f
				));
			
			yield return new WaitForSeconds ( 0.5f);
			LowerBarIsActive = true;
		}
		else
		{
			PullLowerBarUp.enabled = true;
			PullLowerBarDown.enabled = false;
			iTween.MoveTo(LowerBarObject,iTween.Hash(
				"x"   , LowerBarObjectPosOff.transform.position.x,
				"y"	,  LowerBarObjectPosOff.transform.position.y,
				"z"	, LowerBarObjectPosOff.transform.position.z,
				"time", 0.5f
				));
			yield return new WaitForSeconds ( 0.5f);

			LowerBarIsActive = false;
		}
	}
	public void BetMaxButton()
	{
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
		BetCounter = 20;
		BetText.text = ""+BetCounter;
	}
	public void ShowPlayTable(bool _switch)
	{
		PlayTableObject.SetActive(_switch);
	}
	public void ShowInfoWindow(bool _switch)
	{
		InfoWindowObject.SetActive(_switch);
	}
	public void GoBackToLevelSelect()
	{
		AudioManager.Instance.ResumeBGM();
		Application.LoadLevel("GameMenu");
	}
}
