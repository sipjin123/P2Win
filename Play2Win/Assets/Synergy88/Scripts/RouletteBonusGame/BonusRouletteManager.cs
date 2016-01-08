using UnityEngine;
using System.Collections;

public class BonusRouletteManager : MonoBehaviour {

	[SerializeField]
	private GameObject[] rouletteItems;
	[SerializeField]
	private GameObject myCamera;
	[SerializeField]
	private GameObject ResultBoard;
	[SerializeField]
	private tk2dTextMesh result;
	[SerializeField]
	private GameObject[] valueBlocker;
	
	private bool m_spin;

	private int totalPrice;

	private int spinCounter = 0;

	[SerializeField]
	private RouletteBody rouletteBody;
	[SerializeField]
	private BonusRouletteHandScript roulettePin;

	[SerializeField]
	private tk2dTextMesh totalPriceText;

	private GameState _slots = GameState.SLOTS;

	public void OnEnable(){
		for (int i = 0; i < rouletteItems.Length; i++) {
			rouletteItems[i].gameObject.tag = "UnSelected";
		}
		totalPrice = 0;
		m_spin = true;
		myCamera.SetActive (true);

	}

	public void Hide() {
//		ResultBoard.SetActive (false);
//		myCamera.SetActive(false);
		GameManager.Instance.LoadScene (_slots);
	}
	
	public void End() {
		spinCounter = 0;
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		PlayerDataManager.Instance.AddChips (totalPrice);
		Hide();
		//SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
	}

	void StartSpin(){
		if (m_spin) {
			m_spin = false;
			rouletteBody.spinTheWheel();
		}
	}

	void ShowFinalResult(){
		result.text = totalPrice.ToString ("#,#");
		ResultBoard.SetActive (true);

	}

	void RewardAccepted(){
		End ();
	}

	IEnumerator ActivateBlocker(string _blockerName){
		yield return new WaitForSeconds(1.0f);

		for (int i = 0; i < valueBlocker.Length; i++) {
			if(valueBlocker[i].name == _blockerName){
				valueBlocker[i].SetActive(true);
				valueBlocker[i].transform.parent.tag = "Selected";
			}
		}
		if(spinCounter == 4) {
			ShowFinalResult();
		}
	}

	public void RotateFinished(){
		string itemStatus = roulettePin.getItemSelected ();
		int itemObtained = roulettePin.getRoulettePrice ();
		string blocker = roulettePin.getBlockerName ();

		if (itemStatus == "UnSelected") {
			m_spin = true;
			spinCounter += 1;
			totalPrice += itemObtained;
			totalPriceText.text = totalPrice.ToString("#,#");
			roulettePin.setSelectedObject();
			StartCoroutine(ActivateBlocker(blocker));
		} 
		else if (itemStatus == "Selected") {
			m_spin = false;
			ShowFinalResult();
		}
	}
}
