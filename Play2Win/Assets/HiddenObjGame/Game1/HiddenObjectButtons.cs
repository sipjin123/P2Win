using UnityEngine;
using System.Collections;

public class HiddenObjectButtons : MonoBehaviour {

	public tk2dTextMesh _selectedName;
	public bool _isSet;
	public GameObject _selectedObject;
	public GameObject _clearedObject;


	public enum HIDDEN_OBJECT_BUTTONS{
		NONE,
		HINT
	}
	public HIDDEN_OBJECT_BUTTONS _hiddenObjButtons;

	void OnMouseDown()
	{
		if(_hiddenObjButtons == HIDDEN_OBJECT_BUTTONS.HINT)
		{
			HiddenObjectGameManager.Instance._gameItemsSelected[ Random.Range(0,6) ]._selectedObject.GetComponent<HiddenObjectClick>()._hintObj.SetActive(true);;
			gameObject.SetActive(false);
		}
	}
}
