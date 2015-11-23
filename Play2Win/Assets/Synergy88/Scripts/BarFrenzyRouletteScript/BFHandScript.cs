using UnityEngine;
using System.Collections;

public class BFHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public int roulettePrice;
	private GameObject currentPie;

	public int getroullettePrice(){
		return roulettePrice;
	}
	public void ActivateGlow(){
		currentPie.transform.GetChild (0).gameObject.SetActive (true);
	}
	public void DisableGlow(){
		currentPie.transform.GetChild (0).gameObject.SetActive (false);
	}
	void OnTriggerEnter(Collider col){
		currentPie = col.gameObject;
		//AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SPIN_TICK);
		rouletteTextPrice = col.gameObject.name; 
		roulettePrice = int.Parse (rouletteTextPrice);
	}
}
