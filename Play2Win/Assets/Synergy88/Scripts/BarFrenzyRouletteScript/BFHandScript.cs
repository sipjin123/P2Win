using UnityEngine;
using System.Collections;

public class BFHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public int roulettePrice;

	public int getroullettePrice(){
		return roulettePrice;
	}

	void OnTriggerEnter(Collider col){
		//AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SPIN_TICK);
		rouletteTextPrice = col.gameObject.name; 
		roulettePrice = int.Parse (rouletteTextPrice);
	}
}
