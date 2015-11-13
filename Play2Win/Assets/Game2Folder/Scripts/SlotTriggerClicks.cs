using UnityEngine;
using System.Collections;

public class SlotTriggerClicks : MonoBehaviour {

	
	void OnMouseDown(){
		SlotManager.Instance.StartSpin();
	}
}
