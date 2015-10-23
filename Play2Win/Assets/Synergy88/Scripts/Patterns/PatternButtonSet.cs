using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternButtonSet : MonoBehaviour, ISignalListener {

	[SerializeField]
	private PatternButton[] _patternButtons;

	private Dictionary<int, List<PatternButton>> _sortedList;

	private bool _active = false;

	void Start() {
		SignalManager.Instance.Register(this, SignalType.PATTERN_UPDATED);

		_sortedList = new Dictionary<int, List<PatternButton>>();
		foreach (PatternButton button in _patternButtons) {
			if (!_sortedList.ContainsKey(button.LineNumber)) {
				_sortedList.Add(button.LineNumber, new List<PatternButton>());
			}
			_sortedList[button.LineNumber].Add(button);
		}
	}

	public void Activate() {
		gameObject.SetActive(true);
		_active = true;
	}

	public void Deactivate() {
		gameObject.SetActive(false);
		_active = false;
	}

	public void Execute(SignalType type, ISignalParameters param) {
		if (!_active)
			return;

		switch (type) {
		case SignalType.PATTERN_UPDATED:
			int patternsUsed = (int)param.GetParameter(SignalManager.PARAM_PATTERNCOUNT);

			foreach(int key in _sortedList.Keys) {
				if (key <= patternsUsed) {
					foreach(PatternButton button in _sortedList[key]) {
						button.SetSpriteActive();
					}
				} else {
					foreach(PatternButton button in _sortedList[key]) {
						button.SetSpriteInactive();
					}
				}
			}

			break;
		}
	}
}
