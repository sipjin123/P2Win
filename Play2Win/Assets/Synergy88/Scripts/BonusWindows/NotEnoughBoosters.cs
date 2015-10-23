using UnityEngine;
using System.Collections;

public class NotEnoughBoosters : MonoBehaviour {

	[SerializeField]
	GameObject SlotMachine;

	public void Show() {
		gameObject.SetActive(true);
	}
	
	public void Hide() {
		gameObject.SetActive(false);
	}
	
	public void Close() {
		SlotMachine.GetComponent<SlotMachineScene> ().Close ();
		Hide();
	}
	
	public void InApp() {
		SignalManager.Instance.Call(SignalType.BUTTON_MORE_BOOSTERS);
	}
}
