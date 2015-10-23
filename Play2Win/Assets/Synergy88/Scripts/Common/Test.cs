using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			_animator.SetTrigger("Stop");
//			_animator.SetFloat("CurrentAngle", transform.eulerAngles.x);
//			Debug.Log(transform.eulerAngles.x.ToString());
		} else if (Input.GetKeyDown(KeyCode.S)) {
			_animator.SetTrigger("Start");
		} else if (Input.GetKeyDown(KeyCode.D)) {
			Debug.Log (_animator.GetCurrentAnimatorStateInfo(0).IsName("Ended").ToString());
		}
	}
}
