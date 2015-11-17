using UnityEngine;
using System.Collections;

public class SlotTriggerClicks : MonoBehaviour {

	
	void OnMouseDown(){
		if(gameObject.name == "SpinButton")
			SlotManager.Instance.StartSpin();

		if(gameObject.name == "UpBetButton")
			GameManager_ReelChef.Instance.AddBet(true);
		if(gameObject.name == "DownBetButton")
			GameManager_ReelChef.Instance.AddBet(false);

		if(gameObject.name == "RevealAutoSpinItemsButton")
			GameManager_ReelChef.Instance.ShowAutoSpinItems();
		if(gameObject.name == "UpAutoSpinButton5")
			GameManager_ReelChef.Instance.AutoSpinSet(5);
		if(gameObject.name == "UpAutoSpinButton10")
			GameManager_ReelChef.Instance.AutoSpinSet(10);
		if(gameObject.name == "UpAutoSpinButton25")
			GameManager_ReelChef.Instance.AutoSpinSet(25);
	}
}
