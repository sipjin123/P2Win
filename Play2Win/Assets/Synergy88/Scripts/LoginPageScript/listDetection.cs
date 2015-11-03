using UnityEngine;
using System.Collections;

public class listDetection : MonoBehaviour {

	[SerializeField] private GameObject scrollbar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.transform.FindChild ("DropDownItem") != null && this.gameObject.transform.FindChild ("DropDownItem").gameObject.activeSelf) {
			scrollbar.SetActive (true);
		} 
		else {
			scrollbar.SetActive(false);
		}
	}
}
