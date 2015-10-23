using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorChanger : MonoBehaviour {

	[SerializeField]
	private Color[] _colors;

	[SerializeField]
	private bool _isUnityUI = false;

	void Start() {
		SetColor(LevelSpriteCollectionManager.Instance.ActiveLevel);
	}

	private void SetColor(int level) {
		if (_isUnityUI) {
			Image sprite = GetComponent<Image>();
			sprite.color = _colors[level - 1];
		} else {
			SpriteRenderer sprite = GetComponent<SpriteRenderer>();
			sprite.color = _colors[level - 1];
		}
	}

	public void SetColorAlpha_Editor() {
		for (int i = 0; i < _colors.Length; i++) {
			_colors[i].a = 1f;
		}
	}
}
