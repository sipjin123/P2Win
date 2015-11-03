using UnityEngine;
using System.Collections;

public class WhackAMoleScript : MonoBehaviour {

	private const string MOLE_STATE = "isHiding";

	private int waitTime = 0;
	private int stateChanged = 0;
	private int moleIndex = 0;
	private int moleMultiplier = 1;

	private float stateDuration = 0.0f;
	private bool isHiding = true;
	private bool choose;

	[SerializeField]private tk2dSprite myMole;

	[SerializeField]private Animator MoleState;
	[SerializeField]private WhackAMoleManager myManager;

	void Start(){

	}

	IEnumerator WhackTheMole(){
		choose = true;
		yield return new WaitForSeconds (waitTime);

		waitTime = 2;

		if (!myManager.gameover) {
			if(choose){
				stateDuration = Random.Range(1.0f, myManager.timer < 27.0f ? 3.1f : 30.0f - myManager.timer);
				stateChanged = Random.Range (0, 101);
				isHiding = stateChanged > 45 ? false : true;
				moleIndex = Random.Range (0, 100);
				choose = false;
			}
			CheckMoleToShow ();

			if (!isHiding)
				MoleState.SetBool (MOLE_STATE, isHiding);

			yield return new WaitForSeconds (stateDuration);

			if (!isHiding) {
				isHiding = true;
				MoleState.SetBool (MOLE_STATE, isHiding);
			}
			StartCoroutine (WhackTheMole ());
		} 
		else 
			StartCoroutine(EndRound());
	}

	public void startWhack(){
		MoleState.SetBool (MOLE_STATE, isHiding);		
		StartCoroutine (WhackTheMole());
	}

	IEnumerator EndRound(){
		isHiding = true;
		MoleState.SetBool (MOLE_STATE, true);
		yield return new WaitForSeconds (1.0f);
		MoleState.SetBool (MOLE_STATE, false);
	}

	void CheckMoleToShow(){
		if (moleIndex >= 0 && moleIndex < 30) {
			myMole.SetSprite(0);
		} 
		else if (moleIndex >= 30 && moleIndex < 60) {
			myMole.SetSprite(1);
		}
		else if (moleIndex >= 60 && moleIndex < 90) {
			myMole.SetSprite(2);
		} 
		else if (moleIndex >= 90) {
			myMole.SetSprite(3);
		}
	}

	void CheckScoring(){
		if (moleIndex >= 0 && moleIndex < 30)
			myManager.AddScore (100);
		else if (moleIndex >= 30 && moleIndex < 60) 
			myManager.AddScore (200);
		else if (moleIndex >= 60 && moleIndex < 90)
			myManager.AddScore (-200);
		else if (moleIndex >= 90) {
			myManager.AddMultiplier(Random.Range(1,6));
		}
	}

	void HideTheMole(){
		if (!isHiding) {
			isHiding = true;
			CheckScoring();
			MoleState.SetBool (MOLE_STATE, isHiding);
		}
	}
}
