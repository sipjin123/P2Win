using UnityEngine;
using System.Collections;

public class NotEnoughCoins : MonoBehaviour {
	
	public void Show() {
		gameObject.SetActive(true);
	}
	
	public void Hide() {
		gameObject.SetActive(false);
	}
	
	public void Close() {
		Hide();
	}
	
	public void InApp() {
		SignalManager.Instance.Call(SignalType.BUTTON_MORE_COINS);
	}
}