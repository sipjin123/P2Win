using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotDetection : MonoBehaviour,ISignalListener {

	#region DECLARATIONS
	private static SlotDetection _instance;
	public static SlotDetection Instance { get { return _instance; } }

	public enum SLOTSList
	{
		IMAGE1,
		IMAGE2,
		IMAGE3,
		IMAGE4,
		IMAGE5,
		IMAGE6,
		IMAGE7,
		IMAGE8,
		IMAGE9,
		IMAGE10,
		IMAGE11,
		IMAGE12,
		NONE
	}
	public SLOTSList[] _slotsList;
	public GameObject[] SlotDetectors;
	RaycastHit[] SlotRays;

	
	public SLOTSList[] _possibleMatches;
	public int[] _imageMatchCounter;
	public GameObject [] _imageSprites;
	public GameObject[] TableLightings;

	[SerializeField] BFRouletteManager _BFRouletteManager;
	private List<IExtraRewardWindow> _extraRewardsWindow;

	public bool HighlightFinished, BonusHighlightFinished, WildandScatterFinished, CustomerServeFinished, CustomerSeatedFinished;

	public GameObject PatternManager;
	public LinePattern[] _linePattern;
	#endregion

	void Awake()
	{
		_instance = this;
	}

	void Start () 
	{
		_extraRewardsWindow = new List<IExtraRewardWindow>();
		HighlightFinished = true;
		BonusHighlightFinished = true;
		WildandScatterFinished = true;
		CustomerServeFinished = true;
		CustomerSeatedFinished = false;

		_imageMatchCounter = new int[12];
		_slotsList = new SLOTSList[9];
		_possibleMatches = new SLOTSList[3];
		_imageSprites = new GameObject[9];

		SlotRays = new RaycastHit[9];

		int q = 0;
		foreach (Transform child in transform)
		{
			SlotDetectors[q] = child.gameObject;
			q++;
		}
		EMPTYData();
		SignalManager.Instance.Register (this, SignalType.LEVELUPWINDOW_CLOSED);
		SignalManager.Instance.Register (this, SignalType.EXTRA_REWARD_CLOSED);
	}

	void OnDestroy(){
		SignalManager.Instance.Remove (this, SignalType.EXTRA_REWARD_CLOSED);
		SignalManager.Instance.Remove (this, SignalType.LEVELUPWINDOW_CLOSED);
	}


	public void Execute(SignalType type, ISignalParameters param) {
		switch (type) {
		case SignalType.EXTRA_REWARD_CLOSED:
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_INTRO);
			_extraRewardsWindow.RemoveAt (0);
			CheckForBonusWindows ();
			break;
			
		case SignalType.LEVELUPWINDOW_CLOSED:			
			CheckForBonusWindows ();
			break;
		}
	}

	void CheckForBonusWindows(){
		if (_extraRewardsWindow.Count > 0) {
			//AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BONUS);
			_extraRewardsWindow[0].Show();
		} 
	}
	
	public void EMPTYData()
	{
		
		for(int i = 0 ; i < 12 ; i++)
		{
			_imageMatchCounter[i] = 0;
			if( i < 3)
			_possibleMatches[i] = SLOTSList.NONE;
			
			if( i < 9)
			{
				if(_imageSprites[i] != null)
				{
					_imageSprites[i].transform.parent.GetComponent<Itemscript>().HighlightObject.SetActive(false);
					_imageSprites[i].transform.parent.GetComponent<Itemscript>().RewardsObject.SetActive(false);
					
					TableLightings[i].GetComponent<MeshRenderer>().enabled = false;
					_imageSprites[i] = null;
				}
				_slotsList[i] = SLOTSList.NONE;
			}
		}
	}

	#region SLOTS CHECKER/MATCHER/REGISTER
	public void CheckSlots()
	{
		EMPTYData();
		for(int i = 0 ; i < SlotDetectors.Length ; i++)
		{
			if(Physics.Raycast(SlotDetectors[i].transform.position, -SlotDetectors[i].transform.up * 5f, out SlotRays[i]))
			{
				if(SlotRays[i].collider.gameObject.tag == "Slots")
				{
					GameObject temp = SlotRays[i].collider.gameObject.GetComponent<Itemscript>().SlotIcon.gameObject;
					for(int q = 0; q < 12 ; q ++)
					{
						if(temp.gameObject.name == "slot_item"+q)
						{
							_slotsList[i] = (SLOTSList)q;
							_imageSprites[i] = temp;
						}
					}
				}
			}
			//Debug.DrawRay(SlotDetectors[i].transform.position, -SlotDetectors[i].transform.up * 5f);
		}
		CheckForMatches();
	}

	public void CheckForMatches()
	{
		for(int i = 0 ; i < 9 ; i ++)
		{	
			for(int q = 0 ; q < 12 ; q ++)
			{
				if(_slotsList[i] == (SLOTSList)q)
				{
					_imageMatchCounter[q] ++;
				}
			}
		}
		RegisterMatchedSlots();
	}

	public void RegisterMatchedSlots()
	{
		for( int o = 0 ; o < 12 ; o++)
		{
			for(int k = 0; k < 3 ; k++)
			{
				if(o != 8 && o != 7 && o != 9)//incase bonus icons match
				if(_possibleMatches[k] == SLOTSList.NONE && _imageMatchCounter[o] >= 3)
				{
					_possibleMatches[k] = (SLOTSList)o;
					_imageMatchCounter[o] = -_imageMatchCounter[o];
				}
			}
		}
		if(_imageMatchCounter[8] >=2 )
		{
			StartCoroutine(BonusHighlight());
		}
		else
		{
			BonusHighlightFinished = true;
		}

		if(Mathf.Abs(_imageMatchCounter[7])  >=3 || Mathf.Abs(_imageMatchCounter[9]) >=3)
		{
			StartCoroutine(WildNScatterHighlight());
		}
		else
		{
			WildandScatterFinished = true;
		}
		StartCoroutine( HighLightMatches());
	}
	#endregion

	public IEnumerator BonusHighlight()
	{
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_MATCH_DRINKS);
		for(int i = 0 ; i < 9 ; i++)
		{
			if(_imageSprites[i].gameObject.name == "slot_item8")
			{
				TableLightings[i].GetComponent<MeshRenderer>().enabled = true;
			}
		}
		yield return new WaitForSeconds(1);
		float BonusCounter = 0;


		BonusCounter ++;
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_STAR);

		int _bonusCounter = (int)GameManager_ReelChef.Instance.BonusCounter;
		CustomerManager.Instance.StarSparklesEffects[_bonusCounter].SetActive(true);
		yield return new WaitForSeconds(1f);
		CustomerManager.Instance.StarSparklesEffects[_bonusCounter].SetActive(false);
		if(BonusCounter > 0)
		{
			for( int i = 0 ; i < 5 ; i++)
			{
				if(GameManager_ReelChef.Instance.BonusHighlights[i].transform.parent.GetComponent<BonusCheckerScript>().BonusShow.activeSelf == false )
				{
					GameManager_ReelChef.Instance.BonusHighlights[i].transform.parent.GetComponent<BonusCheckerScript>().BonusShow.SetActive(true);
					GameManager_ReelChef.Instance.BonusCounter++;
					break;
				}
			}
		}
		if(GameManager_ReelChef.Instance.BonusCounter >= 5)
		{
			for(int i = 0 ; i < 5 ; i++)
			{
				GameManager_ReelChef.Instance.BonusHighlights[i].transform.parent.GetComponent<BonusCheckerScript>().BonusShow.SetActive(false);
				GameManager_ReelChef.Instance.BonusCounter = 0;
			}
			Debug.LogError("SHOW SPINNING WHEEL DHENZ");			
			_extraRewardsWindow.Add(_BFRouletteManager);
			CheckIfSpinCanBeActive();
		}
		BonusHighlightFinished = true;
		CheckIfSpinCanBeActive();
		CheckForBonusWindows ();

	}
	#region TO REMOVE
	public IEnumerator WildNScatterHighlight()
	{
	
			for(int q = 0 ; q < 3 ; q++)
			{
				for(int i = 0 ; i < 9 ; i++)
				{
					if(_imageSprites[i].gameObject.name == "slot_item7"  && _imageMatchCounter[7] >= 3)
					{
						foreach (Transform child in CustomerManager.Instance.CustomersInside.transform)
						{
								TableLightings[i].GetComponent<MeshRenderer>().enabled = true;
							
						}
					}
					if(_imageSprites[i].gameObject.name == "slot_item9"  && _imageMatchCounter[9] >= 3)
					{
						foreach (Transform child in CustomerManager.Instance.CustomersInside.transform)
						{
							TableLightings[i].GetComponent<MeshRenderer>().enabled = true;
							
						}
					}
				}
			}
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_MATCH_DRINKS);
			yield return new WaitForSeconds (1);

			for(int q = 0 ; q < 3 ; q++)
			{
				for(int i = 0 ; i < 9 ; i++)
				{
					if(_imageSprites[i].gameObject.name == "slot_item7" && _imageMatchCounter[7] >= 3 )
					{
						foreach (Transform child in CustomerManager.Instance.CustomersInside.transform)
						{
							GameObject temp = _imageSprites[i].transform.parent.GetComponent<Itemscript>().RewardsObject;
							_imageSprites[i].transform.parent.GetComponent<Itemscript>().RewardsObject.SetActive(true);
						}
					}
					if(_imageSprites[i].gameObject.name == "slot_item9"&& _imageMatchCounter[9] >= 3)
					{
						foreach (Transform child in CustomerManager.Instance.CustomersInside.transform)
						{
							GameObject temp = _imageSprites[i].transform.parent.GetComponent<Itemscript>().RewardsObject;
							_imageSprites[i].transform.parent.GetComponent<Itemscript>().RewardsObject.SetActive(true);
						}
					}
				}
			}
		
		yield return new WaitForSeconds(0.5f);
		for(int i = 0 ; i  < 9 ;i++)
		_imageSprites[i].transform.parent.GetComponent<Itemscript>().RewardsObject.SetActive(false);

		if(_imageMatchCounter[7] >= 3 )
		{
			for(int i = 0  ; i < 3 ; i++)
			{
				if(CustomerManager.Instance.ScoreEffectsList[i].gameObject.activeSelf == false)
				{
					CustomerManager.Instance.ScoreEffectsList[i].gameObject.SetActive(true);
					AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.PURCHASE);
					
					CustomerManager.Instance.ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = "" + (250 * GameManager_ReelChef.Instance.BetCounter );
					yield return new WaitForSeconds (1);

					iTween.Stop(CustomerManager.Instance.ScoreEffectsList[i]);
					CustomerManager.Instance.ScoreEffectsList[i].gameObject.SetActive(false);
					AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
					break;
				}
			}
			GameManager_ReelChef.Instance.AddScore((int) ( 250 * GameManager_ReelChef.Instance.BetCounter));
		}

		if(_imageMatchCounter[9] >= 3 )
		{
			for(int i = 0  ; i < 3 ; i++)
			{
				if(CustomerManager.Instance.ScoreEffectsList[i].gameObject.activeSelf == false)
				{
					CustomerManager.Instance.ScoreEffectsList[i].gameObject.SetActive(true);
					AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.PURCHASE);
					
					CustomerManager.Instance.ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = "" + (250 * GameManager_ReelChef.Instance.BetCounter );
					yield return new WaitForSeconds (1);
					iTween.Stop(CustomerManager.Instance.ScoreEffectsList[i]);
					CustomerManager.Instance.ScoreEffectsList[i].gameObject.SetActive(false);
					AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_ADD);
					CustomerManager.Instance.ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = "";
					break;
				}
			}
			GameManager_ReelChef.Instance.AddScore((int) ( 250 * GameManager_ReelChef.Instance.BetCounter));
		}
		yield return new WaitForSeconds(1);
		WildandScatterFinished = true;
		CheckIfSpinCanBeActive();
		CheckForBonusWindows ();

	}
	#endregion

	public IEnumerator HighLightMatches()
	{

		for(int q = 0 ; q < 3 ; q++)
		{
			for(int i = 0 ; i < 9 ; i++)
			{
				if(_imageSprites[i].gameObject.name == "slot_item"+(int)_possibleMatches[q])
				{
					foreach (Transform child in CustomerManager.Instance.CustomersInside.transform)
					{
						if(_imageSprites[i].GetComponent<tk2dSprite>().CurrentSprite.name == child.GetComponent<CustomerScript>().OrderSprite.GetComponent<tk2dSprite>().CurrentSprite.name)//added
						{
							TableLightings[i].GetComponent<MeshRenderer>().enabled = true;
							yield return new WaitForSeconds(0.1f);
						}
					}
				}
			}
		}
		HighlightFinished = true;
		CheckIfSpinCanBeActive();
		CustomerManager.Instance.ServeCustomerOrders();

		yield return new WaitForSeconds(0.1f);
	}


	public void CheckIfSpinCanBeActive()
	{
		if(HighlightFinished && BonusHighlightFinished && CustomerServeFinished && CustomerSeatedFinished && WildandScatterFinished)
		{
			StartCoroutine(DelaySpinButtonActive());
			BonusHighlightFinished = false;
			WildandScatterFinished = false;
			HighlightFinished = false;
			CustomerServeFinished = false;
			CustomerSeatedFinished = false;
		}
	}

	public IEnumerator DelaySpinButtonActive()
	{
		yield return new WaitForSeconds(1);
		SlotManager.Instance.isSpinning = false;
		SlotManager.Instance.SpinButton.transform.FindChild("Button").GetComponent<tk2dSprite>().color = Color.white;

		if(GameManager_ReelChef.Instance.AutoSpinCounter > 0)
		{
			SlotManager.Instance.StartSpin();
			GameManager_ReelChef.Instance.AutoSpinCounter --;
			GameManager_ReelChef.Instance.AutoSpinText.text = ""+GameManager_ReelChef.Instance.AutoSpinCounter;
		}
	}
}
