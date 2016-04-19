using UnityEngine;
using System.Collections;

public class HiddenObjectClick : MonoBehaviour {

	public bool _isActive;

	public tk2dTextMesh _objName;

	public GameObject _hintObj;

	void OnMouseDown()
	{
		if(_isActive)
		{
			for(int i = 0 ; i < 6 ; i++)
			{
				if(HiddenObjectGameManager.Instance._gameItemsSelected[i]._selectedName.text == _objName.text)
				{
					HiddenObjectGameManager.Instance._gameItemsSelected[i]._selectedName.text = "";
					HiddenObjectGameManager.Instance._gameItemsSelected[i]._clearedObject.SetActive(true);
					gameObject.SetActive(false);
					HiddenObjectGameManager.Instance.IncrementPoints();
					break;
				}
			}
		}
	}
}
