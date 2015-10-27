using UnityEngine;
using System.Collections;

public class LoadingIconScript : MonoBehaviour {
		
	void Update () {
		this.transform.Rotate (Vector3.back * 700.0f * Time.deltaTime);
	}
}
