using UnityEngine;
using System.Collections;

public class CameraOrtho : MonoBehaviour {

	private const float myReso = 480.0f;
	private const float minimumScale = 0.9f;
	private float addScale = 0;

	[SerializeField]
	private Camera myCamera;

	// Use this for initialization
	void Awake () {
		if (myCamera.aspect > 1.4f && myCamera.aspect < 1.7f) {
			if(myCamera.aspect > 1.499f && myCamera.aspect < 1.5f)
				this.transform.localScale = new Vector3 (1.12f, this.transform.localScale.y, this.transform.localScale.z);
			else 
				this.transform.localScale = new Vector3 (minimumScale + (myCamera.aspect - 1.4f), this.transform.localScale.y, this.transform.localScale.z);
		}
		else if(myCamera.aspect > 1.33f && myCamera.aspect < 1.34f){
			if(myCamera.aspect > 1.3335f)
				this.transform.localScale = new Vector3 (1.13f, this.transform.localScale.y, this.transform.localScale.z);
			else
				this.transform.localScale = new Vector3 (0.9f, this.transform.localScale.y, this.transform.localScale.z);
		}
		else if (myCamera.aspect > 1.7f && myCamera.aspect < 1.75f) {
				this.transform.localScale = new Vector3 (1.15f, this.transform.localScale.y, this.transform.localScale.z);

		} else if(myCamera.aspect > 1.778f && myCamera.aspect < 1.779f ){
			this.transform.localScale = new Vector3 (1.12f, this.transform.localScale.y, this.transform.localScale.z);
		} else if (myCamera.aspect > 1.779f) {
			this.transform.localScale = new Vector3 (1.21f, this.transform.localScale.y, this.transform.localScale.z);
		}
		else {
			this.transform.localScale = new Vector3(minimumScale, this.transform.localScale.y,this.transform.localScale.z);
		}
		Debug.Log (myCamera.aspect);
	}
}
