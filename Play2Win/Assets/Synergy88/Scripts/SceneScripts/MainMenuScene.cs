using UnityEngine;
using System.Collections;

public class MainMenuScene : MonoBehaviour {

	[SerializeField] private tk2dSpriteAnimator[] myAnimator;
	[SerializeField] private tk2dUIItem[] myUIItem;
	[SerializeField] private Animator contentAnim;

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

        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", ProfileUIType.LOBBY);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);

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
		StartCoroutine (waitBeforeLoad (_games));
		myUIItem [0].enabled = false;
    }

    // TEMP: Disable game menu
    private void Load5x3Slots() {
        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        LevelSpriteCollectionManager.Instance.ActiveLevel = 1;
        GameManager.Instance.LoadScene(GameState.SLOTS);

        AudioManager.Instance.PauseBGM();
    }

    public void LoadRewardsScene() {
        StartCoroutine(waitBeforeLoad(_rewards));
        myUIItem[1].enabled = false;
    }

    public void LoadJourneyScene() {
        StartCoroutine(waitBeforeLoad(_journey));
        myUIItem[2].enabled = false;
    }

    public void LoadWalletScene() {
        StartCoroutine(waitBeforeLoad(_wallet));
        myUIItem[3].enabled = false;
    }

	IEnumerator waitBeforeLoad(GameState loadScene){
		if (loadScene == _games) {
			myAnimator[0].Play("BtnShine");
		}
		else if (loadScene == _rewards) {
			myAnimator[1].Play("BtnShine");
		}
		else if (loadScene == _journey) {
			myAnimator[2].Play("BtnShine");
		}
		else if (loadScene == _wallet) {
			myAnimator[3].Play("BtnShine");
		}

		yield return new WaitForSeconds (0.7f);

		contentAnim.SetBool("Hide",true);

		yield return new WaitForSeconds (0.7f);
        
        // TEMP: Disable game menu
        if (loadScene == _games) {
            Load5x3Slots();
        } else {
            GameManager.Instance.LoadScene(loadScene);
        }
	}
}
