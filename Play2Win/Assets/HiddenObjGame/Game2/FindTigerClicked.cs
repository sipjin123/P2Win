using UnityEngine;
using System.Collections;

public class FindTigerClicked : MonoBehaviour {

	public GameObject _foulBeast;

	public GameObject _whichTiger;

	private tk2dSpriteAnimator _animator;
	public GameObject _x1;
	public GameObject _x2;

	private bool _onTop = false;
	private bool _alreadyWon = false;

	void OnEnable()
	{
		GetComponent<BoxCollider>().enabled = true;
		_animator = gameObject.GetComponent<tk2dSpriteAnimator> ();
	}

	void OnMouseUp()
	{
		if (FindTigerGameManager.Instance._gameEnd || !_onTop)
			return;

		FindTigerGameManager.Instance.guess ();

		if(_whichTiger != null)
		{
			_whichTiger.transform.position = new Vector3( _whichTiger.transform.position.x, _whichTiger.transform.position.y, -20);
			FindTigerGameManager.Instance.AddScore();
			GetComponent<BoxCollider>().enabled = false;
			ShowTiger();
		}
		else
		{
			if (!_x1.activeSelf)
				_x1.SetActive(true);
			else if (!_x2.activeSelf)
				_x2.SetActive(true);
		}
	}

	void ShowTiger()
	{
		_alreadyWon = true;

		switch (gameObject.name) {
		case "Bamboo":
			_animator.Play ("BambooWin");
			break;

		case "Stone":
			_animator.Play ("StoneWin");
			break;

		case "Log":
			_animator.Play ("LogWin");
			break;

		case "Left":
			_animator.Play ("BushLeftWin");
			break;

		case "Right":
			_animator.Play ("BushRightWin");
			break;

		default:
			break;
		}
	}

	void OnMouseExit()
	{
		_onTop = false;

		if (_alreadyWon)
			return;

		switch (gameObject.name) {
		case "Bamboo":
			_animator.Play ("BambooIdle");
			break;
			
		case "Stone":
			_animator.Play ("StoneIdle");
			break;
			
		case "Log":
			_animator.Play ("LogIdle");
			break;
			
		case "Left":
			_animator.Play ("BushLeftIdle");
			break;
			
		case "Right":
			_animator.Play ("BushRightIdle");
			break;
			
		default:
			break;
		}
	}

	void OnMouseEnter()
	{
		_onTop = true;

		if (FindTigerGameManager.Instance._gameEnd || _alreadyWon)
			return;

		switch (gameObject.name) {
		case "Bamboo":
			_animator.Play ("BambooGlow");
			break;
			
		case "Stone":
			_animator.Play ("StoneGlow");
			break;
			
		case "Log":
			_animator.Play ("LogGlow");
			break;
			
		case "Left":
			_animator.Play ("BushLeftGlow");
			break;
			
		case "Right":
			_animator.Play ("BushRightGlow");
			break;
			
		default:
			break;
		}
	}

	void OnMouseDown()
	{
		_onTop = true;
		
		if (FindTigerGameManager.Instance._gameEnd || _alreadyWon)
			return;
		
		switch (gameObject.name) {
		case "Bamboo":
			_animator.Play ("BambooGlow");
			break;
			
		case "Stone":
			_animator.Play ("StoneGlow");
			break;
			
		case "Log":
			_animator.Play ("LogGlow");
			break;
			
		case "Left":
			_animator.Play ("BushLeftGlow");
			break;
			
		case "Right":
			_animator.Play ("BushRightGlow");
			break;
			
		default:
			break;
		}
	}
}
