using UnityEngine;
using System.Collections;

public class BonusRouletteHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public string itemSelected;
	private int roulettePrice;
	private GameObject currentlySelected;

	[SerializeField] private tk2dTextMesh rouletteScoreBoard;
	
	void OnTriggerEnter(Collider col){
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SPIN_TICK);
		rouletteTextPrice = col.gameObject.name; 
		rouletteScoreBoard.text = rouletteTextPrice;
		itemSelected = col.gameObject.tag;
		currentlySelected = col.gameObject;
		if(rouletteTextPrice != "2X" && rouletteTextPrice != "3X")
			roulettePrice = int.Parse (rouletteTextPrice);		
	}

	public void setSelectedObject(){
		currentlySelected.gameObject.tag = "Selected";
	}

	public string getItemSelected(){
		return itemSelected;
	}

	public int getRoulettePrice(){
		return roulettePrice;
	}
}
