using UnityEngine;
using System.Collections;

public class CircularLoading : MonoBehaviour {
	private void Update() {
		this.transform.Rotate(Vector3.back, Time.deltaTime * 500);
	}
}
