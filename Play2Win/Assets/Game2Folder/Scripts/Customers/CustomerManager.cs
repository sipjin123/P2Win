using UnityEngine;
using System.Collections;

public class CustomerManager : MonoBehaviour {

	#region DECLARATIONS
	private static CustomerManager _instance;
	public static CustomerManager Instance { get { return _instance; } }


	public int CustomerCount;
	public int TableUsedCount;

	public GameObject[] ScoreEfxStart;
	public GameObject ScoreEfxEnd;

	public GameObject CustomersInside;
	public GameObject CustomersOutside;

	public GameObject[] Customers;

	public GameObject[] ScoreEffectsList;
	public GameObject[] SparklesEffects;
	public GameObject[] StarSparklesEffects;
	#endregion

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

		
		yield return new WaitForSeconds(1.5f);

		_obj.GetComponent<CustomerScript>().myAnimator.Play("WithDrink");

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
		//SCORE AMOUNT INDICATOR
		for(int i = 0; i < 3 ;i++)
		{
			if(ScoreEffectsList[i].gameObject.activeSelf == false && SlotDetection.Instance._possibleMatches[i] != SlotDetection.SLOTSList.NONE)
			{
				ScoreEffectsList[i].gameObject.SetActive(true);
				ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = ""+_score;
				yield return new WaitForSeconds (1);
				ScoreEffectsList[i].gameObject.SetActive(false);

			}
		}

		//SLOT MATCH SPECIAL EFFECTS (SPARKLES GO TOWARDS CUSTOMERS)
		CustomerScript _customerScript = _obj.GetComponent<CustomerScript>();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_POINTS);
	
		for(int i = 1 ; i < 7 ; i++)
		{
			if(_obj.gameObject.name == "Customer"+i)
			{
				SparklesEffects[i-1].SetActive(true);
			}
		}
		yield return new WaitForSeconds(1.25f);	
		for(int i = 1 ; i < 7 ; i++)
		{
			if(_obj.gameObject.name == "Customer"+i)
			{
				SparklesEffects[i-1].SetActive(false);
				
				_obj.GetComponent<CustomerScript>().Sparkles.GetComponent<MeshRenderer>().enabled = true;
				_obj.GetComponent<CustomerScript>().Sparkles.GetComponent<tk2dSpriteAnimator>().SetFrame(0);
				_obj.GetComponent<CustomerScript>().Sparkles.GetComponent<tk2dSpriteAnimator>().Play("SparkleExplode");
			}
		}

		//ADD SCORE
		GameManager_ReelChef.Instance.AddScore(_score);		

		/*
		_customerScript._HungerMeter+= _score;
		_customerScript.transform.FindChild("Text").gameObject.GetComponent<tk2dTextMesh>().text = ""+_customerScript._HungerMeter;
		*/

		if(CustomerManager.Instance.CustomerCount >= 6)
		{
			SlotDetection.Instance.CustomerSeatedFinished = true;
			SlotDetection.Instance.CheckIfSpinCanBeActive();
		}

	}
}
