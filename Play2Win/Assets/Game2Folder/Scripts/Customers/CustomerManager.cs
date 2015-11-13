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
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			SpawnCustomer();
		}

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
						Customers[i].transform.position = Door.transform.position;
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
					child.GetComponent<CustomerScript>()._customerOrder = CustomerScript.CustomerOrder.NONE;
					StartCoroutine(HighlightMatchedOrder(child.gameObject,i));
					numberofcheckedCustomers ++;
				}
			}
		}
		if(numberofcheckedCustomers == 0 && CustomerCount >= 6)
		{
			Debug.LogError("CUSTOMER ENTRY");
			SlotDetection.Instance.CustomerSeatedFinished = true;
			SlotDetection.Instance.CheckIfSpinCanBeActive();
		}
		SlotDetection.Instance.CustomerServeFinished = true;
		SlotDetection.Instance.CheckIfSpinCanBeActive();
	}

	public IEnumerator HighlightMatchedOrder(GameObject _obj,int _counter)
	{
		_obj.GetComponent<CustomerScript>().OrderSprite.GetComponent<tk2dSprite>().color = Color.red;
		yield return new WaitForSeconds(0.5f);
		_obj.GetComponent<CustomerScript>().OrderSprite.GetComponent<tk2dSprite>().color = Color.white;
		yield return new WaitForSeconds(0.5f);
		_obj.GetComponent<CustomerScript>().OrderSprite.GetComponent<tk2dSprite>().color = Color.red;
		yield return new WaitForSeconds(0.5f);
		_obj.GetComponent<CustomerScript>().OrderSprite.GetComponent<tk2dSprite>().color = Color.white;


		float ScoreMultiplier = (float)( Mathf.Abs(	SlotDetection.Instance._imageMatchCounter[(int)(SlotDetection.Instance._possibleMatches[_counter])]));

		StartCoroutine(ScoreEffects(100 *ScoreMultiplier, _obj));

	}
	public IEnumerator ScoreEffects(float _score, GameObject _obj)
	{
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
					"time", 0.5f
					));
				yield return new WaitForSeconds( 0.4f);
				iTween.Stop(ScoreEffectsList[i]);
				GameManager_ReelChef.Instance.AddScore(_score);
				ScoreEffectsList[i].gameObject.SetActive(false);
				ScoreEffectsList[i].GetComponent<tk2dTextMesh>().text = "";
				ScoreEffectsList[i].transform.position = ScoreEfxStart[i].transform.position;
				
				_obj.GetComponent<CustomerScript>().OrderSprite.SetActive(false);
				_obj.GetComponent<CustomerScript>()._customerState = CustomerScript.CustomerState.EXIT;
				break;
			}
		}
	}
}
