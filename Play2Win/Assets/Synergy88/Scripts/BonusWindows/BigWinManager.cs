using UnityEngine;
using System.Collections;

public class BigWinManager : MonoBehaviour, IExtraRewardWindow {

	[SerializeField]
	private tk2dTextMesh _amountText;

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Close();
		}
	}

	public void Show() {
		gameObject.SetActive(true);
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WIN_3);
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

}
