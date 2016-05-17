using UnityEngine;
using System.Collections;

public class FindTigerGameManager : MonoBehaviour {
	private static FindTigerGameManager _instance;
	public static FindTigerGameManager Instance { get { return _instance; } }

	public GameObject[] _tigers;
	public bool[] _tigerSpawned;
	public FindTigerClicked[] _hideSpots;

	
	public Transform _tigersParent;
	public Transform _hideSpotsParent;

	public bool _gameEnd = false;
	public GameObject _gameEndWindow;
	public GameObject _gameStartWindow;
	public int _totalScore;

	public tk2dTextMesh _scoreText, _endScore;
	public tk2dTextMesh _timerText;

	private int _timer = 30;
	public int guessAttemps = 0;

	void Awake()
	{
		_instance = this;
	}
	void Start () 
	{
		_tigers = new GameObject[ _tigersParent.childCount ];
		_tigerSpawned = new bool[ _tigersParent.childCount ];
		_hideSpots = new FindTigerClicked[ _hideSpotsParent.childCount ];

		for(int i = 0 ; i < _hideSpotsParent.childCount ; i++)
		{
			_hideSpots[i] = _hideSpotsParent.GetChild(i).GetComponent<FindTigerClicked>();
		}
		for(int i = 0 ; i < _tigersParent.childCount ; i++)
		{
			_tigers[i] = _tigersParent.GetChild(i).gameObject;
			_tigerSpawned[i] = false;
		}

		_gameEndWindow.SetActive(false);
		_gameStartWindow.SetActive(true);
		_totalScore = 0;

		StartCoroutine( DelayStart() );
	}
	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(3);
		StartCoroutine (startTimer ());
		SetTigersHiddingSpot();
	}

	IEnumerator startTimer()
	{
		while (_timer >= 0) {
			yield return new WaitForSeconds (1);
			_timerText.text = ""+_timer;
			_timer--;
		}
		GameEnd ();
	}

	public void SetTigersHiddingSpot()
	{
		for(int i = 0 ; i < _tigers.Length ; i++)
		{
			if(_tigerSpawned[i] == false)
			{
				int _randomizer = Random.Range(0, 5);
				if(_hideSpots[ _randomizer ]._whichTiger == null)
				{
					_hideSpots[_randomizer]._whichTiger = _tigers[i];
					_tigers[i].transform.position = _hideSpots[_randomizer].transform.position;
					_tigers[i].transform.position = new Vector3( _tigers[i].transform.position.x, _tigers[i].transform.position.y, -1 );
					_tigerSpawned[i] = true;
				}
				else
				{
					SetTigersHiddingSpot();
				}
			}
		}
	}
	public void guess()
	{
		guessAttemps++;

		if (guessAttemps >= 3)
			GameEnd ();
	}

	public void AddScore()
	{
		_totalScore ++;
		_scoreText.text = ""+_totalScore;
		_endScore.text = ""+_totalScore;

		if(_totalScore >= _tigersParent.childCount)
		{
			GameEnd();
		}

	}
	public void GameEnd()
	{
		PlayerDataManager.Instance.TSFreeSpins = _totalScore;
		_gameEnd = true;
		StartCoroutine(GameEndDelay());
	}
	public IEnumerator GameEndDelay()
	{
		yield return new WaitForSeconds(3);
		_gameEndWindow.SetActive(true);
		_gameStartWindow.SetActive(false);
		yield return new WaitForSeconds(2);
		Application.LoadLevel("TigerSlots");
		SignalManager.Instance.Call(SignalType.UPDATE_SETTINGSBTN_SPRITE);
	}

}
