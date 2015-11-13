using UnityEngine;
using System.Collections;

public class Itemscript : MonoBehaviour {

	public GameObject HighlightObject;
	public GameObject RewardsObject;
	void Start () {
		
		gameObject.name =  GetComponent<tk2dSprite>().CurrentSprite.name;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
