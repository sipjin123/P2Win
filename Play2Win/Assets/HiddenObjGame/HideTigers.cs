using UnityEngine;
using System.Collections;

public class HideTigers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (destroyMeAfter());
	}
	
	IEnumerator destroyMeAfter()
	{
		while (GetComponent<tk2dSpriteAnimator> ().Playing) {
			yield return null;
		}
		Destroy (gameObject);
	}
}
