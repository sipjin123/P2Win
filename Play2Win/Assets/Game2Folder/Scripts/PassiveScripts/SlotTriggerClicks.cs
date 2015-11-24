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
		if(gameObject.name == "LowerBarButton")
			GameManager_ReelChef.Instance.ShowLowerBar();

		if(gameObject.name == "PlayTableButtonOn")
			GameManager_ReelChef.Instance.ShowPlayTable(true);
		if(gameObject.name == "PlayTableButtonOff")
			GameManager_ReelChef.Instance.ShowPlayTable(false);
		if(gameObject.name == "BetMaxButton")
			GameManager_ReelChef.Instance.BetMaxButton();
		if(gameObject.name == "TriStar")
			GameManager_ReelChef.Instance.ShowInfoWindow(true);
		if(gameObject.name == "CloseInfo")
			GameManager_ReelChef.Instance.ShowInfoWindow(false);

		
		if(gameObject.name == "LowerBarButton")
			GameManager_ReelChef.Instance.ShowLowerBar();

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
