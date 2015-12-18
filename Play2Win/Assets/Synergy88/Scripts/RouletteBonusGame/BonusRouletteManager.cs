﻿using UnityEngine;
using System.Collections;

public class BonusRouletteManager : MonoBehaviour, IExtraRewardWindow  {

	[SerializeField]
	private GameObject[] rouletteItems;
	[SerializeField]
	private GameObject myCamera;
	[SerializeField]
	private GameObject ResultBoard;
	[SerializeField]
	private tk2dTextMesh result;
	
	private bool m_spin;

	private int totalPrice;

	[SerializeField]
	private RouletteBody rouletteBody;
	[SerializeField]
	private BonusRouletteHandScript roulettePin;

	public void Show(){
		for (int i = 0; i < rouletteItems.Length; i++) {
			rouletteItems[i].gameObject.tag = "UnSelected";
		}
		totalPrice = 0;
		m_spin = true;
		myCamera.SetActive (true);

	}

	public void Hide() {
		ResultBoard.SetActive (false);
		myCamera.SetActive(false);
	}
	
	public void End() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		PlayerDataManager.Instance.AddChips (totalPrice);
		Hide();
		SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
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


	public void RotateFinished(){
		string itemStatus = roulettePin.getItemSelected ();
		int itemObtained = roulettePin.getRoulettePrice ();

		if (itemStatus == "UnSelected") {
			m_spin = true;
			totalPrice += itemObtained;
			roulettePin.setSelectedObject();
		} 
		else if (itemStatus == "Selected") {
			m_spin = false;
			ShowFinalResult();
		}
	}
}
