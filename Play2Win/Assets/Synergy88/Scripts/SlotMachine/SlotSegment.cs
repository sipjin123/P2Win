using UnityEngine;
using System.Collections;

public class SlotSegment : MonoBehaviour,ISignalListener {

//	private const float ROTATION_SPEED_MIN = -360f;
//	private const float ROTATION_SPEED_MAX = -270f;

	private const float ROTATION_DURATION_MIN = 0.1f;
	private const float ROTATION_DURATION_MAX = 1.4f;

	public delegate void RotationEndDelegate();
	private RotationEndDelegate _onRotationEnd;

	[SerializeField]
	private SlotItem[] _slotItems;

	private bool rotating;
	private bool stopping;
	private bool stopped;
//	private float currentRotationSpeed;
	private float currentRotationDuration;

	private Animator _animator;

	void Start() {
		SignalManager.Instance.Register (this, SignalType.HIDE_SLOT_ITEM);
		SignalManager.Instance.Register (this, SignalType.RETURN_SLOT_ITEM);
		_animator = GetComponent<Animator>();
	}

	void OnDestroy(){
		SignalManager.Instance.Remove (this, SignalType.HIDE_SLOT_ITEM);
		SignalManager.Instance.Remove (this, SignalType.RETURN_SLOT_ITEM);
	}

	void Update() {
		if (rotating) {
			UpdateRotation();
		}
	}

	public void Execute(SignalType type, ISignalParameters param) {
		switch (type) {
			case SignalType.HIDE_SLOT_ITEM:
				for(int i = 0; i < _slotItems.Length; i++){
					_slotItems[i].changeColor(1);
				}
				break;
			case SignalType.RETURN_SLOT_ITEM:
				for(int i = 0; i < _slotItems.Length; i++){
					_slotItems[i].changeColor(0);
				}
				break;
		}


	}

	public void SetRotationEnd(RotationEndDelegate OnRotationEnd) {
		_onRotationEnd = OnRotationEnd;
	}

	public void Rotate(bool useLongWait) {
		if (rotating) {
			return;
		}

//		currentRotationSpeed = Random.Range(ROTATION_SPEED_MIN, ROTATION_SPEED_MAX);
		currentRotationDuration = (useLongWait ? ROTATION_DURATION_MAX : ROTATION_DURATION_MIN); //Random.Range(ROTATION_DURATION_MIN, ROTATION_DURATION_MAX);

		rotating = true;

		_animator.SetTrigger("Start");
	}

	public void StartStopping() {
		stopping = true;
	}

	public void GetResult() {

	}


	private void UpdateRotation() {
//		transform.Rotate(currentRotationSpeed * Time.deltaTime, 0, 0, Space.World);
		if (stopping) {
			currentRotationDuration -= Time.deltaTime;
			if (currentRotationDuration <= 0f) {
				StopRotation();
			}
		} else if (stopped) {
			if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Ended")) {
				FullStopAchieved();
			}
		}
	}

	private void StopRotation() {
		_animator.SetTrigger("Stop");
	
		stopping = false;
		stopped = true;
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SLOT_SEGMENT_STOP);

//		Vector3 newRotation = transform.eulerAngles;
//		newRotation.x -= /*30f - */ (transform.eulerAngles.x % 30f);
//		transform.eulerAngles = newRotation;

//		currentRotationSpeed = 0f;
		currentRotationDuration = 0f;
	}

	private void FullStopAchieved() {
		stopped = false;
		rotating = false;

		if (_onRotationEnd != null) {
			_onRotationEnd();
			_onRotationEnd = null;
		}
	}
	
}
