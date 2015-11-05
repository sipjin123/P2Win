using UnityEngine;
using System.Collections;

public class ClickEffect : MonoBehaviour {

	private tk2dSpriteAnimator myEffect;
	// Use this for initialization

	public static ClickEffect Instance { 
		get; 
		private set; 
	}

	void Awake () {
		ClickEffect.Instance = this;
		myEffect = this.GetComponent<tk2dSpriteAnimator>();
	}
	public void ButtonClickEffect(Vector3 p_pos){
		this.gameObject.transform.position = p_pos;
		if(myEffect != null)
			myEffect.Play("ClickParticle");
	}
}
