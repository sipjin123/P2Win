using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour, ISignalListener {
	
	[SerializeField]
	private Transform _sfxCheckSprite;

	[SerializeField]
	private Transform _bgmCheckSprite; 

	void Start() {
		SignalManager.Instance.Register(this, SignalType.SETTINGS_CHANGED);
		SignalManager.Instance.Register(this, SignalType.SHOW_SETTINGS);
		Close();
	}

	void OnDestroy() {
		SignalManager.Instance.ClearRegisterType(SignalType.SETTINGS_CHANGED);
		SignalManager.Instance.ClearRegisterType(SignalType.SHOW_SETTINGS);
	}

	public void Show() {
		gameObject.SetActive(true);
		UpdateCheckBoxes();
	}

	public void ButtonClose() {
		Close();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	public void Close() {
		gameObject.SetActive(false);
		SignalManager.Instance.Call(SignalType.SETTINGS_CLOSED);
	}

	public void SwitchSFX() {
		PlayerDataManager.Instance.ToggleSFX();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	public void SwitchBGM() {
		PlayerDataManager.Instance.ToggleBGM();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	public void FreeCoins() {
		Debug.Log("[FREE COINS not yet integrated]");
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	public void MoreGames() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);

		Debug.LogWarning("[More Games not yet fully integrated]");
	}

	public void Restore() {
		Close ();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	private void UpdateCheckBoxes() {
		_sfxCheckSprite.gameObject.SetActive(PlayerDataManager.Instance.UseSFX);
		_bgmCheckSprite.gameObject.SetActive(PlayerDataManager.Instance.UseBGM);
	}

	public void Execute(SignalType type, ISignalParameters param) {
		switch (type) {
		case SignalType.SETTINGS_CHANGED:
			UpdateCheckBoxes();
			break;
		case SignalType.SHOW_SETTINGS:
			Show();
			break;
		}
	}

}
