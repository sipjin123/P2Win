using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CupMovementManager : MonoBehaviour {


	private GameObject[] RefPos;

	private List<GameObject> selectedCup = new List<GameObject>();

	[SerializeField]
	private CupData[] cups;
	[SerializeField]
	private CupGameManager _gameState;

	private int shuffleRound = 0;
	private float shuffleSpeed = 0;
	private int shuffleCup = 0;
	private int cupIndex = 0;
	private int cupsToMove;

	private List<int> CupShuffle;

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



	public void ShuffleContent(){
		StartCoroutine (StartShuffle ());
	}

	IEnumerator StartShuffle(){
		
		shuffleRound = Random.Range (4, 8);
		shuffleSpeed = Random.Range (0.3f, 0.5f);
		for (int i = 0; i < shuffleRound; i++) {
			shuffleCup = Random.Range(2,4);
			CupShuffle = new List<int>();
			
			for(int cupCount = 0; i < shuffleCup; i++){
				cupIndex = Random.Range(0,3);
				if(!CupShuffle.Contains(cupIndex))
					CupShuffle.Add(cupIndex);
				else {
					i--;
				}
			}
			
		}
		
		yield return new WaitForSeconds (5.0f);
		_gameState.stopShuffle();
	}

}
