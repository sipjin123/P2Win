using UnityEngine;
using System.Collections;

public class WhackAMoleScript : MonoBehaviour {

	[SerializeField] private GameObject Mole;

	private const string MOLE_STATE = "isHiding";

	private const int waitTime = 2;
	private int stateChanged = 0;
	private int moleIndex = 0;
	private int moleMultiplier = 1;

	private float stateDuration = 0.0f;
	private bool isHiding = true;

	[SerializeField]private tk2dSprite myMole;

	private Animator MoleState;
	[SerializeField]private WhackAMoleManager myManager;

	void Start(){
		MoleState = Mole.GetComponent<Animator>();
		MoleState.SetBool (MOLE_STATE, isHiding);

		StartCoroutine (WhackTheMole());
	}

	IEnumerator WhackTheMole(){
		yield return new WaitForSeconds (waitTime);

		stateDuration = Random.Range (0.0f, 5.1f);
		stateChanged = Random.Range (0, 101);
		isHiding = stateChanged > 45 ? false : true;
		moleIndex = Random.Range (0,100);

		CheckMoleToShow ();

		if (!isHiding) {
			MoleState.SetBool (MOLE_STATE, isHiding);
		}
		
		yield return new WaitForSeconds (stateDuration);

		if (!isHiding) {
			isHiding = true;
			MoleState.SetBool (MOLE_STATE, isHiding);
		}
		StartCoroutine (WhackTheMole());

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
