using UnityEngine;
using System.Collections;

public class CupMovementManager : MonoBehaviour {

	private int cupsToMove;
	private GameObject[] RefPos;
	private GameObject[] selectedCup;


	public static CupMovementManager Instance { 
		get; 
		private set; 
	}

	void Start(){
		CupMovementManager.Instance = this;
	}

	public void SetShowHideContent(int cupsToMove){
		selectedCup = new GameObject[cupsToMove];
		for (int i = 0; i < cupsToMove; i++) {
			selectedCup[i] =  CupData.Instance.playCup();
		}
		ShowHideContent (true);
	}

	private void ShowHideContent(bool isShow){
		for (int i = 0; i < cupsToMove; i++) {
			if(isShow){
				iTween.MoveTo (selectedCup [i], iTween.Hash (
					"x", selectedCup[i].transform.position.x,
					"y", selectedCup[i].transform.transform.position.y + 2.0f,
					"z", selectedCup[i].transform.transform.position.z,
					"time", 0.5f
					));
			}
			else{
				iTween.MoveTo (selectedCup [i], iTween.Hash (
					"x", selectedCup[i].transform.position.x,
					"y", selectedCup[i].transform.transform.position.y - 2.0f,
					"z", selectedCup[i].transform.transform.position.z,
					"time", 0.5f
					));
			}
		}
	}

	public void ShuffleContent(){

	}

}
