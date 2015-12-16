using UnityEngine;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour {

	[SerializeField]
	private GameObject coinAndGems;
	[SerializeField]
	private GameObject dice;
	private GameObject TransitionLogo;

	public void SetTransitionLogo(GameObject logo){
		TransitionLogo = logo;
	}

	void OnEnable(){
		StartCoroutine (StartTransition ());
	}

	IEnumerator StartTransition(){
		dice.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		TransitionLogo.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		coinAndGems.SetActive (true);
	}
}
