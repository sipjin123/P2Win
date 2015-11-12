using UnityEngine;
using System.Collections;

public class SlotDetection : MonoBehaviour {
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


	void Awake()
	{
		_instance = this;
	}
	void Start () 
	{
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
				if(_possibleMatches[k] == SLOTSList.NONE && _imageMatchCounter[o] >= 3)
				{
					_possibleMatches[k] = (SLOTSList)o;
					_imageMatchCounter[o] = -_imageMatchCounter[o];
				}
			}
		}
		if(_possibleMatches[0] == SLOTSList.NONE)
		{
			SlotManager.Instance.isSpinning = false;
		}
		StartCoroutine( HighLightMatches());
	}
	public IEnumerator HighLightMatches()
	{
		for(int q = 0 ; q < 3 ; q++)
		{
			for(int i = 0 ; i < 9 ; i++)
			{
				if(_imageSprites[i].gameObject.name == "slot_item"+(int)_possibleMatches[q])
				{
					_imageSprites[i].GetComponent<Itemscript>().HighlightObject.SetActive(true);
					yield return new WaitForSeconds(0.1f);
				}
			}
		}
		CustomerManager.Instance.ServeCustomerOrders();
	}
}
