using UnityEngine;
using System.Collections;

public class SlideInObject : MonoBehaviour {

	private Animator _animator;

	void Start() {
		_animator = GetComponent<Animator>();
	}

	public void Show() {
		_animator.SetTrigger("Show");
	}

	public void Hide() {
		_animator.SetTrigger("Hide");
	}

}
