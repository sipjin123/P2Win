using UnityEngine;
using System.Collections;

public class GameManager_ReelChef : MonoBehaviour {

	private static GameManager_ReelChef _instance;
	public static GameManager_ReelChef Instance { get { return _instance; } }


	public float SpinSpeed;
	public float SpinStrength;
	public tk2dTextMesh ScoreText;

	public float Score;

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
