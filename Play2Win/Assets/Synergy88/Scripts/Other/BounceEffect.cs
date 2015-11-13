using UnityEngine;
using System.Collections;

public class BounceEffect : MonoBehaviour {

	private float defaultXPos = 0;
	private Vector3 originalPos1;
	private Vector3 rightPos;
	private Vector3 leftPos;
	private bool isRight;
	private bool isHold = false;

	[SerializeField] private BounceEffect content;
	[SerializeField] private GameObject[] characters;

	void Awake(){
		defaultXPos = this.transform.localPosition.x;
		originalPos1 = new Vector3(0.0f,0.4102091f,0.0f);

        if (characters.Length > 0) {
		    leftPos = characters [0].transform.localPosition + new Vector3 (-0.5f, 0.0f, 0.0f);
		    rightPos = characters [0].transform.localPosition + new Vector3 (0.5f, 0.0f, 0.0f);
        }
	}

	IEnumerator moveCharacter(){
		if (this.transform.localPosition.x > defaultXPos + 0.001f) {
			for(int i = 0; i < characters.Length; i++){
				characters [i].transform.localPosition = Vector3.Lerp (characters [i].transform.localPosition, leftPos, 0.1f);
			}
			yield return new WaitForSeconds (0.1f);
		} 

		else if (this.transform.localPosition.x < defaultXPos - 0.001f) {
			for(int i = 0; i < characters.Length; i++){
				characters [i].transform.localPosition = Vector3.Lerp (characters [i].transform.localPosition, rightPos, 0.1f);
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator ResetPosition(){
			for (int i = 0; i < characters.Length; i++) {
				characters [i].transform.localPosition = Vector3.Lerp (characters [i].transform.localPosition, originalPos1, 0.2f);
			}
			yield return new WaitForSeconds (0.2f); 
	}

	void Update(){
		if (this.gameObject.tag != "button") {
			if (isHold) {
				StartCoroutine (moveCharacter ());
			} 
			else
				StartCoroutine (ResetPosition ());
		}
	}

	public void setHold(bool p_hold){
		isHold = p_hold;
		defaultXPos = this.transform.localPosition.x;
	}

	void OnMouseDown(){
		if (this.gameObject.tag == "button")
			content.setHold (true);
	}

	void OnMouseUp(){
		if(this.gameObject.tag == "button")
			content.setHold (false);
	}

}
