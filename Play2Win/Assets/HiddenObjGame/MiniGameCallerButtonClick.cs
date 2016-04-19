using UnityEngine;
using System.Collections;

public class MiniGameCallerButtonClick : MonoBehaviour {
	
	public enum TYPEOFGAME
	{
		NONE,
		HIDDEN_OBJECTS,
		FIND_THE_TIGER
	}
	public TYPEOFGAME _typeofGame;

	void OnMouseDown()
	{
		switch(_typeofGame){

		case TYPEOFGAME.FIND_THE_TIGER:
			Application.LoadLevel("FIND_THE_TIGER");
			break;

		case TYPEOFGAME.HIDDEN_OBJECTS:
			Application.LoadLevel("HIDDEN_OBJECTS");
			break;

		}
	}
}
