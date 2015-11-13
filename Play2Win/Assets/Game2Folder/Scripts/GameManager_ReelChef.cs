using UnityEngine;
using System.Collections;

public class GameManager_ReelChef : MonoBehaviour {

	private static GameManager_ReelChef _instance;
	public static GameManager_ReelChef Instance { get { return _instance; } }


	public float SpinSpeed;
	public float SpinStrength;
	public tk2dTextMesh ScoreText;

	public float Score;
	public GameObject[] BonusHighlights;
	public GameObject[] Stars;
	public float BonusCounter;
	void Awake ()
	{
		_instance = this;
	}
	public void AddScore(float _score)
	{
		Score += _score;
		ScoreText.text = ""+Score;
	}
}
