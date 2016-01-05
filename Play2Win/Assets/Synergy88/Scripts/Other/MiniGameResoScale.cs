using UnityEngine;
using System.Collections;

public class MiniGameResoScale : MonoBehaviour {

	[SerializeField]
	private Camera myCamera;



	void Awake(){
		if (myCamera.aspect == 1.5f) {
			this.transform.localScale = new Vector3 (0.95f, this.transform.localScale.y, this.transform.localScale.z);
		}else if (myCamera.aspect >= 1.7f && myCamera.aspect < 1.75f) {
			this.transform.localScale = new Vector3 (1.1f, this.transform.localScale.y, this.transform.localScale.z);
		} else if (myCamera.aspect >= 1.65f && myCamera.aspect < 1.7f) {
			this.transform.localScale = new Vector3 (1.05f, this.transform.localScale.y, this.transform.localScale.z);
		} else if (myCamera.aspect == 1.333333f) {
			this.transform.localScale = new Vector3 (0.85f, this.transform.localScale.y, this.transform.localScale.z);
		} else if(myCamera.aspect == 1.77758f){
			this.transform.localScale = new Vector3 (0.85f, this.transform.localScale.y, this.transform.localScale.z);
		}else if(myCamera.aspect > 1.778f && myCamera.aspect < 1.7783f){
			this.transform.localScale = new Vector3 (1.12f, this.transform.localScale.y, this.transform.localScale.z);
		}
		else {
			this.transform.localScale = new Vector3 (0.85f, this.transform.localScale.y, this.transform.localScale.z);
		}
		Debug.Log (myCamera.aspect);
	}
	
}
