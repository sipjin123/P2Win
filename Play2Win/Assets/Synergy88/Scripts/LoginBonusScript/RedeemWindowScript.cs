using UnityEngine;
using System.Collections;

public class RedeemWindowScript : MonoBehaviour {

	[SerializeField] private GameObject[] spinResult;

	void Start () {
		StartCoroutine (ShowResult ());
	}

	IEnumerator ShowResult(){
		yield return new WaitForSeconds (0.3f);
		spinResult [0].SetActive (true);
		yield return new WaitForSeconds (0.3f);
		spinResult [1].SetActive (true);
		yield return new WaitForSeconds (0.3f);
		spinResult [2].SetActive (true);
	}
}
