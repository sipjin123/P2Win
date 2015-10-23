using UnityEngine;
using System.Collections;

public class AnimationTrigger : MonoBehaviour {
	
	private Animator _animator;	
	private tk2dSpriteAnimator _tk2dSpriteAnimator;

	void Awake() {
		_animator = GetComponent<Animator>();
		_tk2dSpriteAnimator = GetComponent<tk2dSpriteAnimator>();
	}

	void OnEnable() {
		StartCoroutine(Play());
	}

	private IEnumerator Play() {
		_tk2dSpriteAnimator.PlayFromFrame(Random.Range(1, 8));
		float waitTime = Random.Range(0.2f, 2.8f);
		yield return new WaitForSeconds(waitTime);
		_animator.SetTrigger("Play");
	}
	
}
