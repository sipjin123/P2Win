using UnityEngine;
using System.Collections;

public class CustomerScript : MonoBehaviour {

	public enum CustomerState{
		IDLE,
		ENTER,
		SIT,
		ORDER,
		EXIT
	}
	public enum CustomerOrder{
		Menu1,
		Menu2,
		Menu3,
		Menu4,
		Menu5,
		Menu6,
		Menu7,
		Menu8,
		NONE
	}
	public GameObject OrderSprite;
	public GameObject ObjectSprite;
	public CustomerState _customerState;
	public CustomerOrder _customerOrder;
	public GameObject Sparkles;
    [Range(0.0f, 1000.0f)] public float _HungerMeter;

	public GameObject _tableToSit;

	public float MovementSpeed;

	public tk2dSpriteAnimator myAnimator;
	void Start () {
		myAnimator = transform.FindChild("Sprite").gameObject.GetComponent<tk2dSpriteAnimator>();
		MovementSpeed =  Random.Range( 0.1f , 0.2f);
		_customerState = CustomerState.IDLE;
	}
	void Update()
	{
		if(_customerState == CustomerState.ENTER)
			LookForSeat();
		if(_customerState == CustomerState.SIT)
			TakeSeat();
		if(_customerState == CustomerState.EXIT)
			ExitRoomTransition();
	}


	public void LookForSeat()
	{
		//BCOS OF ASSETS
		OrderSprite.SetActive(false);
		RaycastHit TableDetector;
		_customerState = CustomerState.SIT;

		/*
		if(Physics.Raycast(transform.position , -transform.right * 5, out TableDetector))
		{
			if(TableDetector.collider.gameObject.tag == "Table")
			{
				_tableToSit = TableDetector.collider.gameObject;
				if(_tableToSit.GetComponent<TableScript>().isOccupied == true)
				{
					_tableToSit.GetComponent<TableScript>().isOccupied = false;
					_customerState = CustomerState.SIT;

					_customerOrder = (CustomerOrder) ( _tableToSit.GetComponent<TableScript>().TableNumber -1);
				}
				else
				{
					transform.position -= transform.forward * MovementSpeed;
				}
			}
			else
			{
				transform.position -= transform.forward * MovementSpeed;
			}
		}
		Debug.DrawRay(transform.position , -transform.right * 5, Color.red);*/
	}

	public void TakeSeat()
	{
		myAnimator.Play("Demand");
		_customerState = CustomerState.ORDER;
		ShowOrder();
		//BCOS OF ASSETS
		/*
		GameObject chair = _tableToSit.GetComponent<TableScript>().myChild;
		if(Vector3.Distance( chair.transform.position , transform.position) > 0.5f)
		{
			transform.position -= transform.right * MovementSpeed;
		}
		else
		{
			transform.position = chair.transform.position;
			_customerState = CustomerState.ORDER;
			ShowOrder();
		}
		*/
	}
	
	public void ShowOrder()
	{
		myAnimator.Play("WithoutDrink");
		CustomerManager.Instance.TableUsedCount++;
		CustomerManager.Instance.CheckTable();
		OrderSprite.SetActive(true);
		//if(_customerOrder != CustomerOrder.NONE)
		OrderSprite.GetComponent<tk2dSprite>().SetSprite("slot_item"+(((int)_customerOrder)+1));

	}

	public void ExitRoomTransition()
	{
		if(Vector3.Distance(transform.position , CustomerManager.Instance.Door.transform.position) < 1)
		{
			ExitFinish();
		}
		else
		{
			if(CustomerManager.Instance.Door.transform.position.x > transform.position.x)
				transform.position += transform.right * MovementSpeed;
			else
				transform.position += transform.forward * MovementSpeed;
		}

	}
	public void ExitFinish()
	{
		CustomerManager.Instance.CustomerCount --;
		CustomerManager.Instance.TableUsedCount --;
		CustomerManager.Instance.SpawnCustomer();

		_HungerMeter = 0;
		transform.position = CustomerManager.Instance.CustomersOutside.transform.position;
		transform.parent = CustomerManager.Instance.CustomersOutside.transform;
		_customerState = CustomerState.IDLE;
		_tableToSit.GetComponent<TableScript>().isOccupied = true;
	}
}
