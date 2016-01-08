using UnityEngine;
using System.Collections;

public class BonusRouletteHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public string itemSelected;
	private int roulettePrice;
	private GameObject currentlySelected;
	private string blockerGroup;

	[SerializeField] private tk2dTextMesh rouletteScoreBoard;
	
	void OnTriggerEnter(Collider col){
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SPIN_TICK);
		rouletteTextPrice = col.gameObject.name; 
		rouletteScoreBoard.text = rouletteTextPrice;
		itemSelected = col.gameObject.tag;
		currentlySelected = col.gameObject;
		blockerGroup = col.gameObject.transform.GetChild (0).name;
		if(rouletteTextPrice != "2X" && rouletteTextPrice != "3X")
			roulettePrice = int.Parse (rouletteTextPrice);

	}
	public void setSelectedObject(){
		currentlySelected.gameObject.tag = "Selected";
	}

	public string getItemSelected(){
		return itemSelected;
	}

	public string getBlockerName(){
		return blockerGroup;
	}

	public int getRoulettePrice(){
		return roulettePrice;
	}
}
