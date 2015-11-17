using UnityEngine;
using System.Collections;

public class GameManager_ReelChef : MonoBehaviour {

	private static GameManager_ReelChef _instance;
	public static GameManager_ReelChef Instance { get { return _instance; } }


	public float SpinSpeed;
	public float SpinStrength;
	public tk2dTextMesh ScoreText;
	public tk2dTextMesh BetText;
	public tk2dTextMesh AutoSpinText;

	public float Score;
	public float BetCounter;
	public float BonusCounter;
	public float AutoSpinCounter;

	public GameObject AutoSpinItems;
	public GameObject[] BonusHighlights;
	public GameObject[] Stars;
	void Awake ()
	{
		_instance = this;
	}
	public void AddScore(float _score)
	{
		Score += _score;
		ScoreText.text = ""+Score;
	}
	public void AddBet(bool _ifADD)
	{
		if(_ifADD)
		{
			BetCounter ++;
		}
		else
		{
			BetCounter --;
		}
		BetCounter = Mathf.Clamp(BetCounter , 1 , 20);
		BetText.text = ""+BetCounter;
	}
	public void ShowAutoSpinItems()
	{
		if(AutoSpinItems.activeSelf == false)
			AutoSpinItems.SetActive(true);
		else
			AutoSpinItems.SetActive(false);
	}
	public void AutoSpinSet(int _counter)
	{
		AutoSpinCounter = _counter;
		AutoSpinText.text = ""+AutoSpinCounter;
		ShowAutoSpinItems();
	}
}
