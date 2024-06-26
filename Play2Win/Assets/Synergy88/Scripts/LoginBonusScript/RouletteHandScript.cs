﻿using UnityEngine;
using System.Collections;

public class RouletteHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public int roulettePrice;
	[SerializeField] private tk2dTextMesh rouletteScoreBoard;

	void OnTriggerEnter(Collider col){
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SPIN_TICK);
		rouletteTextPrice = col.gameObject.name; 
		rouletteScoreBoard.text = rouletteTextPrice;
		if(rouletteTextPrice != "2X" && rouletteTextPrice != "3X")
			roulettePrice = int.Parse (rouletteTextPrice);
			
	}
}
