using UnityEngine;
using System.Collections;

public class WheelPin : MonoBehaviour {

	[SerializeField]
	private int _currentScore;

	public int CurrentScore { get { return _currentScore; } }

	void OnTriggerEnter(Collider other) {
		WheelItem wheelItem = other.GetComponent<WheelItem>();
		if (wheelItem != null) {
			_currentScore = wheelItem.Amount;
		}
	}
}
