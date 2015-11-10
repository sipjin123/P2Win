using UnityEngine;
using System.Collections;

public class EffectDisable : MonoBehaviour {

	public void disable(){
		this.gameObject.SetActive (false);
	}
	public void reset(){
		this.GetComponent<Animator> ().SetBool ("Hide", false);
	}
}
