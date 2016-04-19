using UnityEngine;
using System.Collections;

public class FindTigerClicked : MonoBehaviour {

	public GameObject _foulBeast;

	public GameObject _whichTiger;

	void OnEnable()
	{
		GetComponent<BoxCollider>().enabled = true;
	}

	void OnMouseDown()
	{
		if(_whichTiger != null)
		{
			_whichTiger.transform.position = new Vector3( _whichTiger.transform.position.x, _whichTiger.transform.position.y, -20);
			FindTigerGameManager.Instance.AddScore();
			GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			_foulBeast.transform.position = new Vector3( _foulBeast.transform.position.x, _foulBeast.transform.position.y, -20);
			FindTigerGameManager.Instance.GameEnd();
		}
	}
}
