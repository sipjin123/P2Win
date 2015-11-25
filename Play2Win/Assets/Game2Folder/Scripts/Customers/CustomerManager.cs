using UnityEngine;
using System.Collections;

public class CustomerManager : MonoBehaviour {

	private static CustomerManager _instance;
	public static CustomerManager Instance { get { return _instance; } }


	public int CustomerCount;
	public int TableUsedCount;

	public GameObject[] ScoreEfxStart;
	public GameObject ScoreEfxEnd;

	public GameObject CustomersInside;
	public GameObject CustomersOutside;
	public GameObject Door;
	public GameObject TableParent;

	public GameObject[] Customers;
	public GameObject[] Tables;

	public GameObject[] ScoreEffectsList;

	void Start () {
		_instance = this;
		CustomerCount = 0;

		int i = 0;
		foreach (Transform child in CustomersOutside.transform)
		{
			Customers[i] = child.gameObject;
			i++;
		}
		int q = 0;
		foreach (Transform child in TableParent.transform)
		{
			Tables[q] = child.gameObject;
			q++;
		}
		StartCoroutine(StartSpawn());
	}

	public IEnumerator StartSpawn()
	{
		yield return new WaitForSeconds(2);
		SpawnCustomer();
	}
	public void CheckTable()
	{
		if(TableUsedCount == 6)
		{
			SlotDetection.Instance.CustomerSeatedFinished = true;
			SlotDetection.Instance.CheckIfSpinCanBeActive();
		}
	}
	public void SpawnCustomer()
	{
		if(CustomerCount < 6)
		{
			for(int i = 0 ; i < Customers.Length ; i++)
			{
				if(CustomerCount < 6)
				{
					if(Customers[i].GetComponent<CustomerScript>()._customerState == CustomerScript.CustomerState.IDLE)
					{
						//BCOS OF ASSETS
						//Customers[i].transform.position = Door.transform.position;
						Customers[i].transform.parent = CustomersInside.transform;
						CustomerCount ++;
						Customers[i].GetComponent<CustomerScript>()._customerState = CustomerScript.CustomerState.ENTER;
					}
				}
			}
		}
	}
	public void ServeCustomerOrders()
	{
		int numberofcheckedCustomers = 0;
		for( int i = 0; i < 3 ;i++)
		{
			foreach (Transform child in CustomersInside.transform)
			{
				if((int)(SlotDetection.Instance._possibleMatches[i]) == (int)(child.GetComponent<CustomerScript>()._customerOrder+1))
				{
					if(child.GetComponent<CustomerScript>()._HungerMeter >= 1000)
					child.GetComponent<CustomerScript>()._customerOrder = CustomerScript.CustomerOrder.NONE;

					child.GetComponent<CustomerScript>().Sparkles.GetComponent<MeshRenderer>().enabled = true;
					child.GetComponent<CustomerScript>().Sparkles.GetComponent<tk2dSpriteAnimator>().SetFrame(0);
					child.GetComponent<CustomerScript>().Sparkles.GetComponent<tk2dSpriteAnimator>().Play("Sparkles");
					child.GetComponent<CustomerScript>().myAnimator.Play("WithDrink");
					StartCoroutine(HighlightMatchedOrder(child.gameObject,i));
					numberofcheckedCustomers ++;
				}
			}
		}
		if(numberofcheckedCustomers == 0 && CustomerCount >= 6)
		{
			SlotDetection.Instance.CustomerSeatedFinished = true;
			SlotDetection.Instance.CheckIfSpinCanBeActive();
		}
		SlotDetection.Instance.CustomerServeFinished = true;
		SlotDetection.Instance.CheckIfSpinCanBeActive();
	}

	public IEnumerator HighlightMatchedOrder(GameObject _obj,int _counter)
	{

		float ScoreMultiplier = 0;
		if( SlotDetection.Instance._possibleMatches[_counter] == SlotDetection.SLOTSList.IMAGE7)
		{
			//Debug.LogError("Tequilla");
			ScoreMultiplier = 210;
		}
		if( SlotDetection.Instance._possibleMatches[_counter] == SlotDetection.SLOTSList.IMAGE6)
		{
			//Debug.LogError("Martini");
			ScoreMultiplier = 150;
		}
		if( SlotDetection.Instance._possibleMatches[_counter] == SlotDetection.SLOTSList.IMAGE5)
		{
			//Debug.LogError("Vodka");
			ScoreMultiplier = 45;
		}
		if( SlotDetection.Instance._possibleMatches[_counter] == SlotDetection.SLOTSList.IMAGE4)
		{
			//Debug.LogError("Rum");
			ScoreMultiplier = 45;
		}
		if( SlotDetection.Instance._possibleMatches[_counter] == SlotDetection.SLOTSList.IMAGE3)
		{
			//Debug.LogError("Wine");
			ScoreMultiplier = 30;
		}
		if( SlotDetection.Instance._possibleMatches[_counter] == SlotDetection.SLOTSList.IMAGE2)
		{
			//Debug.LogError("Beer");
			ScoreMultiplier = 20;
		}
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_MATCH_DRINKS);
		Debug.Log(ScoreMultiplier *  ((float)GameManager_ReelChef.Instance.BetCounter)+" Points");
		StartCoroutine(ScoreEffects(ScoreMultiplier *  ((float)GameManager_ReelChef.Instance.BetCounter), _obj));
		
		yield return new WaitForSeconds(0.5f);
		_obj.GetComponent<CustomerScript>().myAnimator.Play("Cheers");
		yield return new WaitForSeconds(1f);
		_obj.GetComponent<CustomerScript>().myAnimator.Play("Drink");
		yield return new WaitForSeconds(1f);
		_obj.GetComponent<CustomerScript>().myAnimator.Play("Empty");
		yield return new WaitForSeconds(0.5f);
		_obj.GetComponent<CustomerScript>().myAnimator.Play("EmptyBottle");
		yield return new WaitForSeconds(0.1f);
		_obj.GetComponent<CustomerScript>().myAnimator.Play("Demand");
		yield return new WaitForSeconds(0.5f);

	}
	public IEnumerator ScoreEffects(float _score, GameObject _obj)
	{
		for(int i = 0; i < 3 ;i++)
		{
			if(ScoreEffectsList[i].gameObject.activeSelf == false && SlotDetection.Instance._possibleMatches[i] != SlotDetection.SLOTSList.NONE)
			{
				ScoreEffectsList[i].gameObject.SetActive(true);
				
				ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = ""+_score;
				yield return new WaitForSeconds (1);
				iTween.MoveBy(ScoreEffectsList[i].gameObject,iTween.Hash(
					"x"   , ScoreEfxEnd.transform.position.x,
					"y"	,  ScoreEfxEnd.transform.position.y,
					"time", 0.25f
					));
				yield return new WaitForSeconds( 0.24f);
				iTween.Stop(ScoreEffectsList[i]);
				ScoreEffectsList[i].gameObject.SetActive(false);
				ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = "";
				ScoreEffectsList[i].transform.position = ScoreEfxStart[i].transform.position;

			}
		}
		/*
		for(int i = 0; i < 3 ;i++)
		{
			if(ScoreEffectsList[i].gameObject.activeSelf == false)
			{
				ScoreEffectsList[i].gameObject.SetActive(true);

				ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = ""+_score;
				yield return new WaitForSeconds (1);
				iTween.MoveBy(ScoreEffectsList[i].gameObject,iTween.Hash(
					"x"   , ScoreEfxEnd.transform.position.x,
					"y"	,  ScoreEfxEnd.transform.position.y,
					"time", 0.25f
					));
				yield return new WaitForSeconds( 0.24f);
				iTween.Stop(ScoreEffectsList[i]);
				GameManager_ReelChef.Instance.AddScore(_score);
				ScoreEffectsList[i].gameObject.SetActive(false);
				ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = "";
				ScoreEffectsList[i].transform.position = ScoreEfxStart[i].transform.position;

				CustomerScript _customerScript = _obj.GetComponent<CustomerScript>();
				
				_customerScript._HungerMeter+= _score;
				_customerScript.transform.FindChild("Text").gameObject.GetComponent<tk2dTextMesh>().text = ""+_customerScript._HungerMeter;
				if(_customerScript._HungerMeter >= 1000)
				{
					_customerScript.OrderSprite.SetActive(false);
					_customerScript._customerState = CustomerScript.CustomerState.EXIT;	
					_customerScript.transform.FindChild("Text").gameObject.GetComponent<tk2dTextMesh>().text = "0";
				}
				else
				{
			
					SlotDetection.Instance.CustomerSeatedFinished = true;
					SlotDetection.Instance.CheckIfSpinCanBeActive();
				}
				break;
			} 
		}*/
		
		CustomerScript _customerScript = _obj.GetComponent<CustomerScript>();

		float DelayTime = 1.525f;
		for( int i = 0 ; i < 9 ; i++)
		{
			tk2dSprite slotSprite = SlotDetection.Instance._imageSprites[i].GetComponent<tk2dSprite>();
			if(slotSprite.CurrentSprite.name == _customerScript.OrderSprite.GetComponent<tk2dSprite>().CurrentSprite.name)
			{
				if(slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject.activeSelf == false)
				{
					slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject.SetActive(true);
					//slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject.GetComponent<tk2dSprite>().color = Color.blue;


					iTween.MoveTo(slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject ,iTween.Hash(
						"x" , _obj.transform.position.x   ,
						"y"	,  _obj.transform.position.y  ,
						"z"	,  _obj.transform.position.z  ,
						"time", DelayTime
						));

				}
			}
		}
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_POINTS);

		yield return new WaitForSeconds(DelayTime*.75f);
		for(int i = 0 ; i < 9 ; i++)
		{
			tk2dSprite slotSprite = SlotDetection.Instance._imageSprites[i].GetComponent<tk2dSprite>();
			slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject.SetActive(false);
			slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject.transform.position = slotSprite.transform.position;
			iTween.Stop(slotSprite.transform.parent.gameObject.GetComponent<Itemscript>().PointsObject);
		}

		GameManager_ReelChef.Instance.AddScore(_score);				
		_customerScript._HungerMeter+= _score;
		_customerScript.transform.FindChild("Text").gameObject.GetComponent<tk2dTextMesh>().text = ""+_customerScript._HungerMeter;
		/*
		if(_customerScript._HungerMeter >= 1000)
		{
			yield return new WaitForSeconds(1);
			_customerScript.OrderSprite.SetActive(false);
			_customerScript._customerState = CustomerScript.CustomerState.EXIT;	
			_customerScript.transform.FindChild("Text").gameObject.GetComponent<tk2dTextMesh>().text = "0";
		}*/
		if(CustomerManager.Instance.CustomerCount >= 6)
		{
			SlotDetection.Instance.CustomerSeatedFinished = true;
			SlotDetection.Instance.CheckIfSpinCanBeActive();
		}

	}
}
