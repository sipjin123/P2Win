using UnityEngine;
using System.Collections;

public class BFHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public int roulettePrice;
	[SerializeField] private tk2dTextMesh rouletteScoreBoard;

	public int getroullettePrice(){
		return roulettePrice;
	}

	void OnTriggerEnter(Collider col){
		//AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SPIN_TICK);
		rouletteTextPrice = col.gameObject.name; 
		roulettePrice = int.Parse (rouletteTextPrice);
		rouletteScoreBoard.text = this.name == "Player" ? rouletteTextPrice + " :Player" : "Opponent: " + rouletteTextPrice;		
	}
}
