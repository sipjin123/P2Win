using UnityEngine;
using System.Collections;

public class FreeSpinsWinManager : MonoBehaviour, IExtraRewardWindow {

	[SerializeField]
	private tk2dTextMesh _amountText;
	
	public void Show() {
		gameObject.SetActive(true);
	}
	
	public void Hide() {
		gameObject.SetActive(false);
	}
	
	public void Close() {
		SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
		Hide();
	}
	
	public void SetAmount(int amount) {
		_amountText.text = amount.ToString("#,#");
	}

	public void Share() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

}
