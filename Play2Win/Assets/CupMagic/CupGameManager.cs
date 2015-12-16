using UnityEngine;
using System.Collections;

public class CupGameManager : MonoBehaviour {

	public enum GameState{
		IDLE,
		SHOWCUP,
		STARTSHUFFLE,
		STOPSHUFFLE,
		CHOOSECUP,
		PREPFORSHUFFLE,
		SELECTCUP
	}
	public GameState _gameState;
	
	[SerializeField]
	private GameObject[] CupButtons = new GameObject[3];

	private BoxCollider[] buttonCol = new BoxCollider[3];
	private tk2dSprite[] mySprite = new tk2dSprite[3];

	private int shuffleRound = 0;
	private float shuffleSpeed = 0;
	private int shuffleCup = 0;

	void Start(){
		_gameState = GameState.SHOWCUP;
		for (int i = 0; i < mySprite.Length; i++) {
			buttonCol[i] = CupButtons[i].GetComponent<BoxCollider>();
			mySprite[i] = CupButtons[i].GetComponent<tk2dSprite>();
		}
	}

	void ShowCup(){
		CupMovementManager.Instance.SetShowHideContent (3,true);
		DisableButton ();
	}

	void PrepForShuffle(){
		CupMovementManager.Instance.SetShowHideContent (3,false);

		_gameState = GameState.STARTSHUFFLE;
	}

	void DisableButton(){
		for(int i = 0 ; i < CupButtons.Length; i++){
			buttonCol[i].enabled = false;
			mySprite[i].color = new Color(0.5f,0.5f,0.5f);
		}
	}

	IEnumerator StartShuffle(){



		shuffleRound = Random.Range (4, 8);
		shuffleSpeed = Random.Range (0.3f, 0.5f);
		for (int i = 0; i < shuffleRound; i++) {
			shuffleCup = Random.Range(2,4);
			
		}
		
		yield return new WaitForSeconds (5.0f);
		_gameState = GameState.STOPSHUFFLE;
	}

	void EnableButtons(){
		for(int i = 0 ; i < CupButtons.Length; i++){
			buttonCol[i].enabled = true;
			mySprite[i].color = new Color(1.0f,1.0f,1.0f);
		}

		_gameState = GameState.SELECTCUP;
	}

	void StartGame(){
		_gameState = GameState.PREPFORSHUFFLE;
	}

	void ButtonSelected(){
		if(_gameState == GameState.SELECTCUP)
			_gameState = GameState.SHOWCUP;
	}

	void Update(){
		if (_gameState == GameState.SHOWCUP) {
			_gameState = GameState.IDLE;
			ShowCup ();
		} 
		else if (_gameState == GameState.PREPFORSHUFFLE) {
			_gameState = GameState.IDLE;
			PrepForShuffle ();
		} 
		else if (_gameState == GameState.STARTSHUFFLE) {
			_gameState = GameState.IDLE;
			StartCoroutine (StartShuffle ());
		} 

		else if (_gameState == GameState.STOPSHUFFLE) {
			_gameState = GameState.IDLE;
			EnableButtons();
		}
	}

}
