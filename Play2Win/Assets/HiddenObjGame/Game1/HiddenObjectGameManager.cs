using UnityEngine;
using System.Collections;

public class HiddenObjectGameManager : MonoBehaviour {
	private static HiddenObjectGameManager _instance;
	public static HiddenObjectGameManager Instance { get { return _instance; } }

	public Transform _gameItemHolder;
	public Transform _gameItemSelectedHolder;

	public HiddenObjectClick[] _gameItems;
	public HiddenObjectButtons[] _gameItemsSelected;

	public int _childCount;

	public float _timer;
	public tk2dTextMesh _timerText;

	public bool _gameEnd;
	public GameObject _gameEndWindow;
	public GameObject _gameStartWindow;
	public int _points;
	public tk2dTextMesh _pointsText,_pointsText2 ;

	void Awake()
	{
		_instance = this;
	}
	void Start()
	{
		_childCount = _gameItemHolder.childCount;
		_gameItems = new HiddenObjectClick[ _childCount ];
		_gameItemsSelected = new HiddenObjectButtons[ 6 ];

		_timer = 30;
		_gameEnd = false;
		_gameEndWindow.SetActive(false);
		_gameStartWindow.SetActive(true);
		_points = 0;

		for(int i = 0 ; i < _childCount; i++)
		{
			_gameItems[i] = _gameItemHolder.GetChild(i).GetComponent<HiddenObjectClick>();
			_gameItems[i]._objName.text = _gameItems[i].gameObject.name;
			_gameItems[i]._objName.gameObject.SetActive(true);
		}
		for(int i = 0 ; i < 6 ; i++)
		{
			_gameItemsSelected[i] = _gameItemSelectedHolder.GetChild(i).GetComponent<HiddenObjectButtons>();
		}
		RandomizeSelectedObjects();
	}
	void RandomizeSelectedObjects()
	{

		for(int i = 0 ; i < 6 ; i++)
		{
			if(_gameItemsSelected[i]._isSet == false)
			{
				int randomizer = Random.Range(0, _childCount);
				if(_gameItems[ randomizer ]._isActive == false)
				{
					_gameItemsSelected[i]._selectedName.text = _gameItems[ randomizer ]._objName.text;
					_gameItemsSelected[i]._selectedObject = _gameItems[randomizer].gameObject;
					_gameItems[ randomizer ]._isActive = true;
					_gameItemsSelected[i]._isSet = true;
				}
				else
				{
					RandomizeSelectedObjects();
				}
			}
		}
	}
	public void IncrementPoints()
	{
		_points++;
		_pointsText.text = ""+_points;
		_pointsText2.text = ""+_points;
		if(_points >=6)
		{
			GameEnd();
		}
	}
	public void GameEnd()
	{
		StartCoroutine( GameEndWait() );
	}
	IEnumerator GameEndWait()
	{
		
		_gameEnd = true;
		_gameEndWindow.SetActive(true);
		_gameStartWindow.SetActive(false);
		yield return new WaitForSeconds(2);
		Application.LoadLevel("MainGameSample");
	}
	void Update()
	{
		if(_timer > 0)
		{
			_timer -= Time.deltaTime;
			_timerText.text = ""+ (int)_timer;
		}
		else
		{
			GameEnd();
		}
	}
}
