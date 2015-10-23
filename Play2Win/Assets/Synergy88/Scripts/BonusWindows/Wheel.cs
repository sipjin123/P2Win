using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {

	private enum WheelState {
		SPINNING,
		STOPPING,
		STOPPED
	}

	private const float STOP_VELOCITY = 0.01f;

	private Rigidbody2D _rigidbody;
	private WheelState _currentState;

	public delegate void OnStopDelegate();
	private OnStopDelegate _onStopDelegate;

	void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_currentState = WheelState.STOPPED;
	}

	void Update() {

		switch(_currentState) {
		case WheelState.STOPPING:

			if ( Mathf.Abs(_rigidbody.angularVelocity) <= STOP_VELOCITY) {
				_currentState = WheelState.STOPPED;
				if (_onStopDelegate != null) {
					_onStopDelegate();
				}
			}

			break;
		}

//		if (Input.GetKeyDown(KeyCode.A)) {
//			Spin();
//		}
//		if (Input.GetKeyDown(KeyCode.S)) {
//			Stop();
//		}


	}

	public void SetStopDelegate(OnStopDelegate callback) {
		_onStopDelegate = callback;
	}

	public void Spin() {
		_rigidbody.AddTorque(400f);
		_rigidbody.angularDrag = 0;
		_currentState = WheelState.SPINNING;
	}

	public void Stop() {
		_rigidbody.AddTorque(_rigidbody.angularVelocity * -0.1f);
		_rigidbody.angularDrag = 0.5f;
		_currentState = WheelState.STOPPING;
	}
}
