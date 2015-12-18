using UnityEngine;
using System.Collections;

public class CupMovementManager : MonoBehaviour {

	private int cupsToMove;
	private GameObject[] RefPos;
	private GameObject[] selectedCup;
	[SerializeField]
	private CupData[] cups;


	public static CupMovementManager Instance { 
		get; 
		private set; 
	}

	void Start(){
		CupMovementManager.Instance = this;
	}

	public void SetShowHideContent(int cupsToMove,bool isShow){
		selectedCup = new GameObject[cupsToMove];
		for (int i = 0; i < cupsToMove; i++) {
			selectedCup[i] =  cups[i].gameObject;
		}
		ShowHideContent (isShow,cupsToMove);
	}

	private void ShowHideContent(bool isShow,int _cupsToMove){
		for (int i = 0; i < _cupsToMove; i++) {
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
