using UnityEngine;
using System.Collections;

public class SpinTheWheelManager : MonoBehaviour, IExtraRewardWindow {

	[SerializeField]
	private Wheel _wheel;

	[SerializeField]
	private WheelPin _pin;

	[SerializeField]
	private GameObject _totalBonusOverlay;

	[SerializeField]
	private tk2dTextMesh _amountText;

	private bool _spinCalled;
	private bool _stopCalled;
	
	private int _multiplier;
	private int _coins;

	void Start() {
		_wheel.SetStopDelegate(OnWheelStopped);
	}
	
	public void SetMultiplier(int amount) {
		_multiplier = amount;
	}
	
	public void SetCoins(float amount) {
		_coins = (int)amount;
	}

	public void Show() {
		gameObject.SetActive(true);
		_totalBonusOverlay.SetActive(false);
		_spinCalled = false;
		_stopCalled = false;
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WHEEL_INTRO);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void End() {
		Hide();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
	}

	public void FBConnect() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	public void OnWheelStopped() {
		Debug.Log(_pin.CurrentScore);

		int value = _pin.CurrentScore * _multiplier * _coins;
		PlayerDataManager.Instance.AddCoins(value);
		_amountText.text = (value).ToString("#,#");
		int textCount = _amountText.NumTotalCharacters() - 10;
		if (textCount > 0) {
			float textScale = 1 - textCount * 0.06f;
			_amountText.transform.localScale = new Vector2(textScale, textScale);
		}
		_totalBonusOverlay.SetActive(true);

		AudioManager.Instance.StopGlobalAudio(AudioManager.GlobalAudioType.WHEEL_BGM);
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WHEEL_WIN);
	}

	public void OnStartSpinning() {
		if (!_spinCalled) {
			_spinCalled = true;
			_wheel.Spin();
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WHEEL_BGM);
		}
	}

	public void OnStopSpinning() {
		if (!_stopCalled && _spinCalled) {
			_stopCalled = true;
			_wheel.Stop();
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.WHEEL_STOP);
		}
	}

}
