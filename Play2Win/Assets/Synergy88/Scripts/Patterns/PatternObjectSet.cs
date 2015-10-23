using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternObjectSet : MonoBehaviour {

	private const float SHOW_WIN_DURATION = 3f;

	[SerializeField]
	private PatternObject[] _patternObjects;

	private int _activeLines;
	public int ActiveLines { get { return _activeLines; } }

	private List<PatternObject> _winningPatterns = new List<PatternObject>();

	private bool _showingWinningSegment = false;

	public int WinningPatternCount { get { return _winningPatterns.Count; } }

	public void Activate() {
		gameObject.SetActive(true);
	}
	
	public void Deactivate() {
		gameObject.SetActive(false);
	}

	public void SetActiveLines(int lineCount) {
		_activeLines = lineCount;

		for(int i = 0; i < _patternObjects.Length; i++) {
			_patternObjects[i].SetActive(i + 1 <= lineCount);
		}
	}

	public void RegisterWinningPatterns() {
		_winningPatterns.Clear();

		for (int i = 0; i < _activeLines; i++) {
			if (_patternObjects[i].RegisterPatterns()) {
				_winningPatterns.Add(_patternObjects[i]);
			}
		}
	}

	public void ShowWinningPatterns() {
		_showingWinningSegment = true;

		StartCoroutine(RotateWinningPatterns());
	}

	public void ShowAllWinningPatterns() {
		_showingWinningSegment = true;
		foreach(PatternObject pattern in _winningPatterns) {
			pattern.ShowWinningPattern();
		}
	}

	public void StopShowingWinningPatterns() {
		_showingWinningSegment = false;

		foreach(PatternObject pattern in _winningPatterns) {
			pattern.SetActive(false);
		}
	}

	public int GetTotalWinnings(int coinsBet) {
		int totalWinnings = 0;

		foreach(PatternObject pattern in _winningPatterns) {
			totalWinnings += coinsBet * pattern.GetTotalWinnings();
		}
		
		return totalWinnings;
	}


	private IEnumerator RotateWinningPatterns() {
		if (_winningPatterns.Count == 0) {
			yield break;
		}

		if (_winningPatterns.Count == 1) {
			_winningPatterns[0].ShowWinningPattern();
			yield break;
		}

		int i = 0;
		while (_showingWinningSegment) {

//			if (i == 0) {
//				for (int x = 0; x < _winningPatterns.Count; x++) {
//					_winningPatterns[x].ShowWinningPattern();
//				}
//
//				yield return new WaitForSeconds(SHOW_WIN_DURATION);
//
//				for (int x = 0; x < _winningPatterns.Count; x++) {
//					_winningPatterns[x].SetActive(false);
//				}
//
//				yield return new WaitForSeconds(0.5f);
//			}

			if (_winningPatterns.Count == 0 || i >= _winningPatterns.Count) {
				_showingWinningSegment = false;
				yield break; // Catcher in case array changes during loop
			}

			_winningPatterns[i].ShowWinningPattern();

			yield return new WaitForSeconds(SHOW_WIN_DURATION);

			if (_winningPatterns.Count == 0 || i >= _winningPatterns.Count) {
				_showingWinningSegment = false;
				yield break; // Catcher in case array changes during loop
			}

			_winningPatterns[i].SetActive(false);

			yield return new WaitForSeconds(0.5f);

			i++;
			if (i >= _winningPatterns.Count) {
				i = 0;
			}
		}
	}

}
