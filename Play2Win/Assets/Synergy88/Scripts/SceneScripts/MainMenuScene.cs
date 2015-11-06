using UnityEngine;
using System.Collections;

public class MainMenuScene : MonoBehaviour {

    [SerializeField]
    private GameState _games;

    [SerializeField]
    private GameState _rewards;

    [SerializeField]
    private GameState _journey;

    [SerializeField]
    private GameState _wallet;

    [SerializeField]
    private GameObject _boosterAvailableObject;

    [SerializeField]
    private tk2dTextMesh _timerText;

    private bool _bonusAvailable = false;

    void Start() {
        SignalManager.Instance.Call(SignalType.LOBBY_ENTERED);

        if (!GameDataManager.Instance.LobbyWindowsLoaded) {
            Application.LoadLevelAdditive("PlayerProfile");
            Application.LoadLevelAdditive("Settings");
            Application.LoadLevelAdditive("LevelupWindow");
            Application.LoadLevelAdditive("MessagePrompt");

            GameDataManager.Instance.SetLobbyWindowsLoaded();
        }

        PlayerDataManager.Instance.InitializeListener();

        UpdateTimerVisibility();

        AudioManager.Instance.SwitchBGM(AudioManager.GlobalAudioType.BGM_LOBBY);
        AudioManager.Instance.ResumeBGM();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Application.Quit();
        }

        if (_bonusAvailable != PlayerDataManager.Instance.BonusAvailable) {
            _bonusAvailable = PlayerDataManager.Instance.BonusAvailable;
            UpdateTimerVisibility();
        }

        if (!_bonusAvailable) {
            _timerText.text = PlayerDataManager.Instance.BonusTimeLeftString;
        }
    }

    private void UpdateTimerVisibility() {
        _boosterAvailableObject.gameObject.SetActive(_bonusAvailable);
        _timerText.gameObject.SetActive(!_bonusAvailable);
    }

    public void OpenSettings() {
        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        SignalManager.Instance.Call(SignalType.SHOW_SETTINGS);
    }

    public void CollectBonus() {
        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.COINS);
        PlayerDataManager.Instance.CollectBonus();
    }

    public void LoadGameMenu() {
        GameManager.Instance.LoadScene(_games);
    }

    public void LoadRewardsScene() {
        GameManager.Instance.LoadScene(_rewards);
    }

    public void LoadJourneyScene() {
        GameManager.Instance.LoadScene(_journey);
    }

    public void LoadWalletScene() {
        GameManager.Instance.LoadScene(_wallet);
    }
}
