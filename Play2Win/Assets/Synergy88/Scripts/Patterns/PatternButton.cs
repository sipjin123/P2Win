using UnityEngine;
using System.Collections;

public class PatternButton : MonoBehaviour {

	private const string SPRITE_PREFIX = "line_";
	private const string ACTIVE_SUFFIX = "_active";
	private const string INACTIVE_SUFFIX = "_inactive";

	[SerializeField]
	private int _lineNumber;

	[SerializeField]
	private tk2dSprite _buttonSprite;


	public int LineNumber { get { return _lineNumber; } }


	public void SetSpriteInactive() {
		_buttonSprite.SetSprite(SPRITE_PREFIX + _lineNumber + INACTIVE_SUFFIX);
	}

	public void SetSpriteActive() {
		_buttonSprite.SetSprite(SPRITE_PREFIX + _lineNumber + ACTIVE_SUFFIX);
	}
}
