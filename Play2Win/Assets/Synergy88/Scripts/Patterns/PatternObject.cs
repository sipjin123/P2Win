using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternObject : MonoBehaviour {

	private enum State {
		HIDDEN,
		SHOWN,
		FADING_OUT,
		PRESENTING
	}

	private const float SHOW_DURATION = 3f;
	private const float FADE_DURATION = 0.5f;

	[SerializeField]
	private SlotItemDetectorManager _slotItemDetectorManager;

	[SerializeField]
	private SlotItemDetector[] _slotItemDetectors;

	[SerializeField]
	private tk2dSprite _sprite;

	private State _currentState = State.HIDDEN;
	private float _timer = 0f;
	private Color _fadeColorTemp;

	private PatternReward _winningObjects;

	void Update() {
		switch(_currentState) {
		case State.SHOWN:
			_timer -= Time.deltaTime;
			if (_timer <= 0f) {
				SetState(State.FADING_OUT);
			}
			break;

		case State.FADING_OUT:
			_timer -= Time.deltaTime;
			if (_timer <= 0f) {
				SetState(State.HIDDEN);
			} else {
				UpdateAlpha(_timer / FADE_DURATION);
			}
			break;
		}
	}

	void OnDrawGizmosSelected() {
		for (int i = 0; i < _slotItemDetectors.Length; i++) {
			if (_slotItemDetectors[i] != null) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(_slotItemDetectors[i].transform.position, 0.2f);
			}
		}
	}

	public void SetActive(bool visible) {
		if (visible) {
			SetState(State.SHOWN);
		} else {
			SetState(State.HIDDEN);
		}
	}

	public void ShowWinningPattern() {
		SetState(State.PRESENTING);
	}

	private void UpdateAlpha(float scale) {
		_fadeColorTemp.a = scale;
		_sprite.color = _fadeColorTemp;
	}

	private void SetState(State nextState) {
		State previousState = _currentState;

		_currentState = nextState;
		switch (_currentState) {
		case State.SHOWN:
			_sprite.gameObject.SetActive(true);
			_sprite.color = Color.white;
			_timer = SHOW_DURATION;
			break;

		case State.FADING_OUT:
			_timer = FADE_DURATION;
			_fadeColorTemp = Color.white;
			break;

		case State.HIDDEN:
			_sprite.gameObject.SetActive(false);
			_sprite.color = Color.white;
			_timer = 0;
			break;

		case State.PRESENTING:
			_sprite.gameObject.SetActive(true);
			_sprite.color = Color.white;
			SignalManager.Instance.Call(SignalType.HIDE_SLOT_ITEM);
			AnimateWinningObjects();
			break;
		}

		switch (previousState) {
		case State.PRESENTING:
			SignalManager.Instance.Call(SignalType.RETURN_SLOT_ITEM);
			StopAllObjectAnimations();
			break;
		}
	}

	private void AnimateWinningObjects() {
		foreach(SlotItem item in _winningObjects.items) {
			item.Shake();
		}
	}

	private void StopAllObjectAnimations() {
		foreach(SlotItem item in _winningObjects.items) {
			item.Reset();
		}
	}

	public bool RegisterPatterns() {
		_winningObjects = _slotItemDetectorManager.GetBaseRewardFromPattern(_slotItemDetectors);
		return _winningObjects.items.Count > 0;

		/** Old pattern counting **/
//		_winningObjects.Clear();
//
////		Debug.Log("Registering patterns for Pattern : " + gameObject.name);
//
//		Dictionary<SlotItemType, List<SlotItem>> patternCount = new Dictionary<SlotItemType, List<SlotItem>>();
//		for (int x = 0; x < _slotItemDetectors.Length; x++) {
//			SlotItemType type = _slotItemDetectors[x].GetItem().CurrentItemType;
//			if (!patternCount.ContainsKey(type)) {
//				patternCount.Add(type, new List<SlotItem>());
//			}
//
//			patternCount[type].Add(_slotItemDetectors[x].GetItem());
//		}
//
//		foreach(SlotItemType key in patternCount.Keys) {
//			if (patternCount[key].Count > 1) {
////				Debug.Log ("Adding pattern objects with number : " + key);
////				foreach(PatternSpot spot in patternCount[key]) {
////					Debug.Log ("--- " + spot.gameObject.name);
////				}
//				_winningObjects.AddRange(patternCount[key]);
//			}
//		}
//
//		return (_winningObjects.Count > 0);
	}

	public int GetTotalWinnings() {
		return _winningObjects.totalReward;
	}

	/** EDITOR FUNCTIONS **/

	public void SetSlotItemDetector(int index, int position) {
		_slotItemDetectors[index] = _slotItemDetectorManager.GetItem(index, position);
	}
}
