using UnityEngine;
using System.Collections;

public class BounceEffect : MonoBehaviour {

	private float defaultXPos = 0;
	private Vector3 originalPos1;
	private Vector3 rightPos;
	private Vector3 leftPos;
	private bool isRight;

	[SerializeField] private GameObject[] characters;

	void Awake(){
		defaultXPos = this.transform.position.x;
		originalPos1 = characters[0].transform.localPosition;
		leftPos = characters [0].transform.localPosition + new Vector3 (-0.3f, 0.0f, 0.0f);
		rightPos = characters [0].transform.localPosition + new Vector3 (0.3f, 0.0f, 0.0f);
	}

	void Update(){
		if (this.transform.position.x > defaultXPos + 0.001f) {
			characters[0].transform.localPosition = Vector3.Lerp(characters[0].transform.localPosition,leftPos,0.1f);
			characters[1].transform.localPosition = Vector3.Lerp(characters[1].transform.localPosition,leftPos,0.1f);
			characters[2].transform.localPosition = Vector3.Lerp(characters[2].transform.localPosition,leftPos,0.1f);
			characters[3].transform.localPosition = Vector3.Lerp(characters[3].transform.localPosition,leftPos,0.1f);

		}
		else if (this.transform.position.x < defaultXPos - 0.001f) {
			characters[0].transform.localPosition = Vector3.Lerp(characters[0].transform.localPosition,rightPos,0.1f);
			characters[1].transform.localPosition = Vector3.Lerp(characters[1].transform.localPosition,rightPos,0.1f);
			characters[2].transform.localPosition = Vector3.Lerp(characters[2].transform.localPosition,rightPos,0.1f);
			characters[3].transform.localPosition = Vector3.Lerp(characters[3].transform.localPosition,rightPos,0.1f);
			
		}
		else if(this.transform.position.x < defaultXPos + 0.002f && this.transform.position.x > defaultXPos - 0.002f){
			characters[0].transform.localPosition = Vector3.Lerp(characters[0].transform.localPosition,originalPos1,0.3f);
			characters[1].transform.localPosition = Vector3.Lerp(characters[1].transform.localPosition,originalPos1,0.3f);
			characters[2].transform.localPosition = Vector3.Lerp(characters[2].transform.localPosition,originalPos1,0.3f);
			characters[3].transform.localPosition = Vector3.Lerp(characters[3].transform.localPosition,originalPos1,0.3f);
		}
	}

}
