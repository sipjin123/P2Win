using UnityEngine;
using System.Collections;
using System.IO;
//using System.Runtime.InteropServices;

public class LobbyScene : MonoBehaviour {
	
    //[DllImport ("__Internal")]
    //private static extern void showScreen();

    //[SerializeField]
    //private tk2dTextMesh _timerText;

    //[SerializeField]
    //private Transform _boosterAvailableObject;

    //private bool _bonusAvailable = false;

	void Start() {
		SignalManager.Instance.Call(SignalType.LOBBY_ENTERED);
		InitializeSingletonDependentManagers();

		Application.LoadLevelAdditive ("LevelSelection");

		if (!GameDataManager.Instance.LobbyWindowsLoaded) {
			Application.LoadLevelAdditive("PlayerProfile");
			GameDataManager.Instance.SetLobbyWindowsLoaded();
			StartCoroutine(DelayedInitializer());
		}

		BundleLoader.Instance.ClearTextureReferences();

		UpdateTimerVisibility();

		ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
		stateParam.AddParameter(SlotMachineScene.PARAM_LOCK, false);
		SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);
	}

	//--- this function is to decrease the loading upon startup
	IEnumerator DelayedInitializer(){
		yield return new WaitForSeconds (0.5f);
        //Application.LoadLevelAdditive("UniBill");
        //Application.LoadLevelAdditive("FBPARSE");
		Application.LoadLevelAdditive("Settings");
		Application.LoadLevelAdditive("LevelupWindow");
		Application.LoadLevelAdditive("MessagePrompt");
	}

	void Update() {
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

	private void InitializeSingletonDependentManagers() {
		PlayerDataManager.Instance.InitializeListener();
	}

	public void CollectBonus() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.COINS);
		PlayerDataManager.Instance.CollectBonus();
	}

	public void FreeGames() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		Debug.LogWarning("[Free Games not yet integrated]");
	}

	public void MoreGames() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		Debug.LogWarning("[More Games not yet fully integrated]");
	}

    //public void ShowMoreGames(){
    //    showScreen();
    //}
}
