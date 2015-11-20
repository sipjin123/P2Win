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

	void ShowSetting(){
		if (isHide) {
			settingsAnim.SetBool ("Hide", false);
			isHide = false;
		}
		else {
			settingsAnim.SetBool ("Hide", true);
			isHide = true;
		}
	}

	void ShowSettingsWindow(){
		SignalManager.Instance.Call (SignalType.SHOW_SETTINGS);
	}

	void ShowRedeemWindow(){
		redeemWindow.SetActive (true);

	}
	void CloseRedeemWindow(){
		inputCode.text = "";
		redeemWindow.SetActive (false);
	}

	void CheckRedeemCode(){
		if (inputCode.text == "FREECHIPS") {
			PlayerDataManager.Instance.AddChips (1000000);
		} 
		else if (inputCode.text == "FREEGEMS") {
			PlayerDataManager.Instance.AddPoints (1000000);
		} 
		else if (inputCode.text == "LEVELUP!") {
            PlayerDataManager.Instance.AddExp(GameDataManager.Instance.LevelInfo.ExpToNextLevel);
        } 
        else if (inputCode.text == "RESTART") {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
	}

	void Validate(){
		CheckRedeemCode ();
		CloseRedeemWindow ();
	}
	
}
