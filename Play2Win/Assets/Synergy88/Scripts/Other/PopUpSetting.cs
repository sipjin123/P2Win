using UnityEngine;
using System.Collections;

public class PopUpSetting : MonoBehaviour {

	private Animator settingsAnim;
	private bool isHide = true;
	[SerializeField] private GameObject redeemWindow;
	[SerializeField] private tk2dTextMesh inputCode;

	void Start(){
		settingsAnim = this.GetComponent<Animator> ();
	}

	void CheckHit(){
		RaycastHit hit = new RaycastHit ();
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);					
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject != this.gameObject) {
				settingsAnim.SetBool ("Hide", true);
				isHide = true;
			}
		}
	}
	void Update(){

#if UNITY_ANDROID
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				CheckHit ();
			}
		}
#endif
#if UNITY_EDITOR
		if (Input.GetMouseButton (0)) {
			CheckHit ();
		}

#endif
	}
	void ShowSetting(){
		if (isHide) {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
			settingsAnim.SetBool ("Hide", false);
			isHide = false;
		}
		else {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.EXIT);
			settingsAnim.SetBool ("Hide", true);
			isHide = true;
		}
	}

	void ShowSettingsWindow(){
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		SignalManager.Instance.Call (SignalType.SHOW_SETTINGS);
		settingsAnim.SetBool ("Hide", true);
		isHide = true;
	}

	void ShowRedeemWindow(){
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		redeemWindow.SetActive (true);
	}
	void CloseRedeemWindow(){
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.EXIT);
		inputCode.text = "";
		redeemWindow.SetActive (false);
		settingsAnim.SetBool ("Hide", true);
		isHide = true;
	}

	void CheckRedeemCode(){
		if (inputCode.text == "FREECHIPS") {
			PlayerDataManager.Instance.AddChips (1000000);
		} else if (inputCode.text == "FREEGEMS") {
			PlayerDataManager.Instance.AddPoints (1000000);
		} else if (inputCode.text == "LEVELUP!") {
			PlayerDataManager.Instance.AddExp (GameDataManager.Instance.LevelInfo.ExpToNextLevel);
		} 
		else if (inputCode.text == "RESETALL") {
			PlayerPrefs.DeleteAll();
		}
        else if (inputCode.text == "RESTART") {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
	}

	void Validate(){
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.SELECT);
		CheckRedeemCode ();
		inputCode.text = string.Empty;
		redeemWindow.SetActive (false);
		settingsAnim.SetBool ("Hide", true);
		isHide = true;
	}
	
}
