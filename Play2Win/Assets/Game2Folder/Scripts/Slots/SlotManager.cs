using UnityEngine;
using System.Collections;

public class SlotManager : MonoBehaviour {
	
	private static SlotManager _instance;
	public static SlotManager Instance { get { return _instance; } }

	public GameObject[] SLOTSPINS;
	public int SpinCheckCounter;
	public bool isSpinning;
	public GameObject SpinButton;
	void Awake()
	{
		_instance = this;
	}
	void Start () 
	{
		SpinCheckCounter = 0;
		isSpinning = true;
		SlotManager.Instance.SpinButton.transform.FindChild("ButtonGraphic").GetComponent<tk2dSprite>().color = Color.red;

	}


	void Update()
	{

		if(Input.GetKeyDown(KeyCode.Space))
		{
			StartSpin();
		}
	}
	public void StartSpin()
	{
		if(!isSpinning)
		{
			SlotManager.Instance.SpinButton.transform.FindChild("ButtonGraphic").GetComponent<tk2dSprite>().color = Color.red;
			isSpinning = true;
			SlotDetection.Instance.EMPTYData();
			for(int i = 0 ; i <  3 ; i++)
			{
				SLOTSPINS[i].GetComponent<SlotSpin>()._gameState = SlotSpin.GameState.STARTSPIN;
				SLOTSPINS[i].GetComponent<SlotSpin>().SpinStrength = GameManager_ReelChef.Instance.SpinStrength * (i+1);
			}
		}
	}
	public void SpinChecker()
	{
		if(SpinCheckCounter >= 2)
		{
			SlotDetection.Instance.CheckSlots();
			SpinCheckCounter = 0;
			return;
		}
		SpinCheckCounter++;
	}
}
