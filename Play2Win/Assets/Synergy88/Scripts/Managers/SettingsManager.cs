using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour, ISignalListener {
	
	[SerializeField]
    private Transform _sfxOnSprite;

    [SerializeField]
    private Transform _sfxOffSprite;

    [SerializeField]
    private Transform _bgmOnSprite;

    [SerializeField]
    private Transform _bgmOffSprite;

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
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.EXIT);
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

	public void RestartProfile() {
        PlayerPrefs.DeleteAll();
        Application.Quit();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
	}

	private void UpdateCheckBoxes() {
        _sfxOnSprite.gameObject.SetActive(PlayerDataManager.Instance.UseSFX);
        _sfxOffSprite.gameObject.SetActive(!PlayerDataManager.Instance.UseSFX);
        _bgmOnSprite.gameObject.SetActive(PlayerDataManager.Instance.UseBGM);
        _bgmOffSprite.gameObject.SetActive(!PlayerDataManager.Instance.UseBGM);
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
