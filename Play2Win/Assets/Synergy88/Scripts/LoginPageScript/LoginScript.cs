using UnityEngine;
using System.Collections;
using System;

public class LoginScript : MonoBehaviour {

	private string playerName = string.Empty;
	private DateTime playerBirthday = DateTime.Today;

	[SerializeField] private GameObject[] loginDisable;
	[SerializeField] private GameObject loginUser;
	[SerializeField] private GameObject loginNotif;
	[SerializeField] private GameObject scrollbar;
	[SerializeField] private tk2dUIScrollableArea scrollable;

	[SerializeField] private tk2dTextMesh pNameInputBox;
	[SerializeField] private tk2dTextMesh[] pBirthdayInputBox;
	[SerializeField] private tk2dTextMesh userName;
	[SerializeField] private tk2dTextMesh userNameForVerification;
	
	private int FBLoginBonus = 500;

	[SerializeField]
	private GameState _mainMenuScene = GameState.MAIN_MENU;


	void disableUserInput(){
		for (int count = 0; count < loginDisable.Length; count++) {
			loginDisable[count].SetActive(false);
		}	
	}
	IEnumerator LoginPlayer(string p_name, DateTime p_birthday){
		userName.text = p_name;
		loginUser.SetActive (true);

		yield return new WaitForSeconds (3.0f);

		loginUser.SetActive (false);
		userNameForVerification.text = userName.text;

		yield return new WaitForSeconds (0.1f);

		loginNotif.SetActive (true);  		
	}

	void CheckLoginBonus(){
		DateTime lastLogin = DateTime.Today;
		if (PlayerPrefs.HasKey ("PLAYER_LAST_LOGIN"))
			lastLogin = DateTime.Parse(PlayerPrefs.GetString("PLAYER_LAST_LOGIN"));

		if (DateTime.Compare (lastLogin.AddDays (1), DateTime.Today) == 0) {
			//2x
		}
		else {
			//1x;
		}

	}

	void Login(){
		playerName = pNameInputBox.text == string.Empty ? "Deafult Guest" : pNameInputBox.text;
		playerBirthday = DateTime.Parse (pBirthdayInputBox[0].text + "/" + pBirthdayInputBox[1].text + "/" + pBirthdayInputBox[2].text);
		disableUserInput();
		CheckLoginBonus ();
		StartCoroutine (LoginPlayer (playerName, playerBirthday));
		
	}

	void FBLogin(){
		playerName = "Default Guest";
		playerBirthday = DateTime.Today;
		disableUserInput();
		PlayerDataManager.Instance.AddChips (FBLoginBonus);.
		CheckLoginBonus ();
		StartCoroutine (LoginPlayer (playerName, playerBirthday));
	}

	void LoginContinue(){
		PlayerDataManager.Instance.lastLogin (DateTime.Today);
		GameManager.Instance.LoadScene(_mainMenuScene);
	}

	void EnableScroll(){
		if (!scrollbar.activeSelf) {
			this.scrollable.enabled = true;
			this.scrollbar.SetActive (true);
		} 
		else {
			this.scrollable.enabled = false;
			this.scrollbar.SetActive (false);
		}
	}
}
