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
			this.transform.localScale = new Vector3 (minimumScale + (myCamera.aspect - 1.4f), this.transform.localScale.y, this.transform.localScale.z);
		} 
		else if (myCamera.aspect > 1.7f) {
			this.transform.localScale = new Vector3 (1.12f, this.transform.localScale.y, this.transform.localScale.z);
		}
		else {
			this.transform.localScale = new Vector3(minimumScale, this.transform.localScale.y,this.transform.localScale.z);
		}
		Debug.Log (myCamera.aspect);
	}
}
