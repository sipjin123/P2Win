using UnityEngine;
using System.Collections;

public class Itemscript : MonoBehaviour {

	public GameObject HighlightObject;
	public GameObject RewardsObject;
	public GameObject PointsObject;
	public GameObject SlotIcon;
	void Awake()
	{
		SlotIcon = transform.FindChild("SlotIcon").gameObject;
	}
	void Start () {
		
		gameObject.name = SlotIcon.GetComponent<tk2dSprite>().CurrentSprite.name;
	}

	// Update is called once per frame
	void Update () {

	}
}
