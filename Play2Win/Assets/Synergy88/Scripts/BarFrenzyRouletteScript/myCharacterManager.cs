using UnityEngine;
using System.Collections;

public class myCharacterManager : MonoBehaviour {

	[SerializeField] private GameObject[] myCharacterStates;
	[SerializeField] private tk2dSpriteAnimator[] myCharacterAnimator;

	IEnumerator characterMix(string p_char){
		myCharacterStates [3].SetActive (false);
		myCharacterStates [0].SetActive (true);
		myCharacterAnimator[0].Play(p_char == "Player" ? "GirlMix" : "BoyMix");
		yield return new WaitForSeconds (p_char == "Player" ? 1.5f : 1.4f);
		myCharacterStates [0].SetActive (false);
		myCharacterStates [1].SetActive (true);
		myCharacterAnimator[1].Play(p_char == "Player" ? "GirlPour" : "BoyPour");
		yield return new WaitForSeconds (p_char == "Player" ? 1.4f : 1.07f);
		myCharacterStates [1].SetActive (false);
		myCharacterStates [3].SetActive (true);
	}

	public void StartMixing(string p_sender){
		StartCoroutine (characterMix (p_sender));
	}
	public void CharacterWin(string p_sender){
		myCharacterStates [3].SetActive (false);
		myCharacterStates [4].SetActive (true);
	}
	public void CharacterLose(string p_sender){
		myCharacterStates [3].SetActive (false);
		myCharacterAnimator[2].Play(p_sender == "Player" ? "GirlMix" : "BoyMix");
	}
} 
