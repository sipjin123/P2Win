using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotDetection : MonoBehaviour,ISignalListener {
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

	[SerializeField] BFRouletteManager _BFRouletteManager;
	private List<IExtraRewardWindow> _extraRewardsWindow;

	public bool HighlightFinished, BonusHighlightFinished, CustomerServeFinished, CustomerSeatedFinished;
	void Awake()
	{
		_instance = this;
	}
	void Start () 
	{
		_extraRewardsWindow = new List<IExtraRewardWindow>();
		HighlightFinished = true;
		BonusHighlightFinished = true;
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

	void OnGUIx()
	{
		GUI.Box( new Rect(0,30,300,30),"Hilyt: "+ HighlightFinished);
		GUI.Box( new Rect(0,60,300,30),"BonusHilyt: "+BonusHighlightFinished);
		GUI.Box( new Rect(0,90,300,30),"Serve: "+CustomerServeFinished);
		GUI.Box( new Rect(0,120,300,30),"Enter: "+CustomerSeatedFinished);
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
					_imageSprites[i].GetComponent<Itemscript>().HighlightObject.SetActive(false);
					_imageSprites[i].GetComponent<Itemscript>().RewardsObject.SetActive(false);
					_imageSprites[i] = null;
				}
				_slotsList[i] = SLOTSList.NONE;
			}
		}
	}
	public void CheckSlots()
	{
		EMPTYData();
		for(int i = 0 ; i < SlotDetectors.Length ; i++)
		{
			if(Physics.Raycast(SlotDetectors[i].transform.position, -SlotDetectors[i].transform.up * 5f, out SlotRays[i]))
			{
				if(SlotRays[i].collider.gameObject.tag == "Slots")
				{
					GameObject temp = SlotRays[i].collider.gameObject;
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
				if(o != 8)//incase bonus icons match
				if(_possibleMatches[k] == SLOTSList.NONE && _imageMatchCounter[o] >= 3)
				{
					_possibleMatches[k] = (SLOTSList)o;
					_imageMatchCounter[o] = -_imageMatchCounter[o];
				}
			}
		}
		if(_imageMatchCounter[8] >=1 )
		{
			StartCoroutine(BonusHighlight());
		}
		else
		{
			BonusHighlightFinished = true;
		}
		StartCoroutine( HighLightMatches());
	}
	public IEnumerator BonusHighlight()
	{
		yield return new WaitForSeconds(1);
		float BonusCounter = 0;
		for(int i = 0 ; i < 9 ; i++)
		{
			if(_imageSprites[i].GetComponent<tk2dSprite>().CurrentSprite.name == "slot_item" + 8)
			{
				for(int m = 0 ; m < 5 ; m++)
				{
					if(GameManager_ReelChef.Instance.BonusHighlights[m].transform.parent.GetComponent<BonusCheckerScript>().BonusShow.activeSelf == false)
					{
						BonusCounter ++;
						_imageSprites[i].GetComponent<Itemscript>().RewardsObject.SetActive(true);
						iTween.MoveTo(_imageSprites[i].GetComponent<Itemscript>().RewardsObject,iTween.Hash(
							"x"   , GameManager_ReelChef.Instance.BonusHighlights[m].transform.position.x,
							"y"	,  GameManager_ReelChef.Instance.BonusHighlights[m].transform.position.y,
							"z"	,  GameManager_ReelChef.Instance.BonusHighlights[m].transform.position.z,
							"time", 0.25f
							));
						//yield return new WaitForSeconds( 0.25f);
						//_imageSprites[i].GetComponent<Itemscript>().RewardsObject.SetActive(false);
						//iTween.Stop(_imageSprites[i]);
						break;
					}
				}
			}
		}
		yield return new WaitForSeconds( 0.25f);
		for(int i = 0 ; i < 9 ;i++)
		{
			_imageSprites[i].GetComponent<Itemscript>().RewardsObject.SetActive(false);
			iTween.Stop(_imageSprites[i]);
		}
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
							_imageSprites[i].GetComponent<Itemscript>().HighlightObject.GetComponent<tk2dSprite>().color = Color.white;
							_imageSprites[i].GetComponent<Itemscript>().HighlightObject.SetActive(true);
							yield return new WaitForSeconds(0.1f);
						}
					}
				}

			}
		}
		HighlightFinished = true;
		CheckIfSpinCanBeActive();
		CustomerManager.Instance.ServeCustomerOrders();
	}


	public void CheckIfSpinCanBeActive()
	{
		if(HighlightFinished && BonusHighlightFinished && CustomerServeFinished && CustomerSeatedFinished)
		{
			StartCoroutine(DelaySpinButtonActive());
			BonusHighlightFinished = false;
			HighlightFinished = false;
			CustomerServeFinished = false;
			CustomerSeatedFinished = false;
		}
	}
	public IEnumerator DelaySpinButtonActive()
	{
		yield return new WaitForSeconds(1);
		SlotManager.Instance.isSpinning = false;
		SlotManager.Instance.SpinButton.transform.FindChild("ButtonGraphic").GetComponent<tk2dSprite>().color = Color.white;

		
		if(GameManager_ReelChef.Instance.AutoSpinCounter > 1)
		{
			SlotManager.Instance.StartSpin();
			GameManager_ReelChef.Instance.AutoSpinCounter --;
			GameManager_ReelChef.Instance.AutoSpinText.text = ""+GameManager_ReelChef.Instance.AutoSpinCounter;
		}
	}
}
