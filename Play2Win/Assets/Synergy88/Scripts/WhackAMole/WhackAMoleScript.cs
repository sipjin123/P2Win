using UnityEngine;
using System.Collections;

public class WhackAMoleScript : MonoBehaviour {

	[SerializeField] private GameObject Mole;

	private const string MOLE_STATE = "isHiding";

	private const int waitTime = 2;
	private int stateChanged = 0;

	private float stateDuration = 0.0f;
	private bool isHiding = true;

	private Animator MoleState;

	void Start(){
		MoleState = Mole.GetComponent<Animator> ();
		StartCoroutine (WhackTheMole());
		MoleState.SetBool (MOLE_STATE, isHiding);
	}

	IEnumerator WhackTheMole(){
		yield return new WaitForSeconds (waitTime);

		stateDuration = Random.Range (0.0f, 5.1f);
		stateChanged = Random.Range (0, 101);
		isHiding = stateChanged > 45 ? false : true;

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

	void HideTheMole(){
		if (!isHiding) {
			isHiding = true;
			MoleState.SetBool (MOLE_STATE, isHiding);
		}
	}
}
