using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CupMovementManager : MonoBehaviour {

	private int cupsToMove;
	private GameObject[] RefPos;
	private List<GameObject> selectedCup = new List<GameObject>();
	[SerializeField]
	private CupData[] cups;


	public static CupMovementManager Instance { 
		get; 
		private set; 
	}

	void Start(){
		CupMovementManager.Instance = this;
	}

	public void SetShowHideContent(bool isShow){
		for (int i = 0; i < cups.Length; i++) {
			selectedCup.Add(cups[i].gameObject);
		}
		ShowHideContent (isShow);
	}

	private void ShowHideContent(bool isShow){
		for (int i = 0; i < selectedCup.Count; i++) {
			if(isShow){
				iTween.MoveTo (selectedCup[i], iTween.Hash (
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
		selectedCup.RemoveRange(0,3);

	}



	public void ShuffleContent(int cupIndex){

	}

}
