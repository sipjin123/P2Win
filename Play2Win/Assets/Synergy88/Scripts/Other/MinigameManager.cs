using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour {

	[SerializeField]
	private GameObject WhackAMole;
	[SerializeField]
	private GameObject spinWheel;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString ("BonusLoaded") == "WhackAMole") {
			WhackAMole.SetActive(true);	
		} 
		else if (PlayerPrefs.GetString ("BonusLoaded") == "SpinWheel") {
			spinWheel.SetActive(true);
		}
	}
}
