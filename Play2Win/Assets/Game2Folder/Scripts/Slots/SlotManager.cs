using UnityEngine;
using System.Collections;

public class SlotManager : MonoBehaviour {
	
	private static SlotManager _instance;
	public static SlotManager Instance { get { return _instance; } }

	public GameObject[] SLOTSPINS;
	public int SpinCheckCounter;
	public bool isSpinning;
	void Awake()
	{
		_instance = this;
	}
	void Start () 
	{
		SpinCheckCounter = 0;
		isSpinning = true;
	}




	void OnGUI()
	{
		if(!isSpinning)
		if(GUI.Button( new Rect ( Screen.width - Screen.width / 8 , Screen.height - Screen.height /8 , Screen.width /8 , Screen.height /8 ), "SPIN"))
		{
			
			isSpinning = true;
			StartSpin();
		}
	}
	public void StartSpin()
	{
		SlotDetection.Instance.EMPTYData();
		for(int i = 0 ; i <  3 ; i++)
		{
			SLOTSPINS[i].GetComponent<SlotSpin>()._gameState = SlotSpin.GameState.STARTSPIN;
			SLOTSPINS[i].GetComponent<SlotSpin>().SpinStrength = GameManager_ReelChef.Instance.SpinStrength * (i+1);
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
