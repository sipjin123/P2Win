using UnityEngine;
using System.Collections;
using System.IO;

public class LobbyScene : MonoBehaviour {
	
	void Start() {

        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", ProfileUIType.LOBBY);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);

		UpdateTimerVisibility();

		ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
		stateParam.AddParameter(SlotMachineScene.PARAM_LOCK, false);
		SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);

        AudioManager.Instance.SwitchBGM(AudioManager.GlobalAudioType.BGM_LOBBY);
        AudioManager.Instance.ResumeBGM();
	}

	void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            ExitScene();
        }

        //if (_bonusAvailable != PlayerDataManager.Instance.BonusAvailable) {
        //    _bonusAvailable = PlayerDataManager.Instance.BonusAvailable;
        //    UpdateTimerVisibility();
        //}

        //if (!_bonusAvailable) {
        //    _timerText.text = PlayerDataManager.Instance.BonusTimeLeftString;
        //}
	}

	private void UpdateTimerVisibility() {
        //_boosterAvailableObject.gameObject.SetActive(_bonusAvailable);
        //_timerText.gameObject.SetActive(!_bonusAvailable);
	}

    public void ExitScene() {
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
    }

	public void CollectBonus() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.COINS);
		PlayerDataManager.Instance.CollectBonus();
	}

    public void Load5x3Slots() {
        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        LevelSpriteCollectionManager.Instance.ActiveLevel = 1;
        GameManager.Instance.LoadScene(GameState.SLOTS);

        AudioManager.Instance.PauseBGM();
    }

    public void LoadGame2() {

    }

}
