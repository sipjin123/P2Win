using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour {

	[SerializeField]
	private GameObject[] _splashImages;

	[SerializeField]
	private float _fadeInDuration = 1f;

	[SerializeField]
	private float _stayDuration = 1f;

	[SerializeField]
	private float _fadeOutDuration = 1f;

	private int _currentSplash = 0;
	private Color _tempColor;

	private float _currentAlpha;
	private float _timer;
	private SplashState _currentState;

	private bool _end = false;

	private enum SplashState {
		HIDDEN,
		FADE_IN,
		STAY,
		FADE_OUT
	}

	void Start() {
		for (int i = 0; i < _splashImages.Length; i++) {
			_splashImages[i].gameObject.SetActive(false);

			_tempColor = _splashImages[i].GetComponent<Image>().color;
			_tempColor.a = 0f;
			_splashImages[i].GetComponent<Image>().color = _tempColor;
		}

		StartFade(0);
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (_end)
			return;

		_timer += Time.deltaTime;

		switch (_currentState) {
		case SplashState.HIDDEN:
			if (_currentSplash >= _splashImages.Length - 1) {
				_end = true;
                GameManager.Instance.LoadScene(GameState.LOG_IN);
				Destroy(gameObject);
			} 
			else {
				StartFade(_currentSplash + 1);
			}

			break;

		case SplashState.FADE_IN:
			if (_timer >= _fadeInDuration) {
				SetAlpha(1f);
				SetState(SplashState.STAY);
			} 
			else {
				SetAlpha(_timer/_fadeInDuration);
			}

			break;

		case SplashState.STAY:
			if (_timer >= _stayDuration) {
				SetState(SplashState.FADE_OUT);
			}

			break;

		case SplashState.FADE_OUT:
			if (_timer >= _fadeOutDuration) {
                SetAlpha(0f);
				SetState(SplashState.HIDDEN);
			} 
			else {
                SetAlpha(1f - (_timer / _fadeOutDuration));
			}
			break;
		}

		if (Input.GetMouseButtonDown(0)) {
			SetAlpha(0f);
			SetState(SplashState.HIDDEN);
		}
	}

	private void StartFade(int index) {
		_splashImages[index].gameObject.SetActive(true);
		_tempColor = _splashImages[index].GetComponent<Image>().color;
		_currentSplash = index;
		SetState(SplashState.FADE_IN);
	}

	private void SetAlpha(float a) {
		_tempColor.a = a;
		_splashImages[_currentSplash].GetComponent<Image>().color = _tempColor;
	}

	private void SetState(SplashState state) {
		_currentState = state;
		_timer = 0f;
	}
}
