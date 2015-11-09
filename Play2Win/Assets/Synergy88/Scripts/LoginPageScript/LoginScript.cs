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
	[SerializeField]
	private GameState _BonusSpin = GameState.SPINBONUS;


	void disableUserInput(){
		for (int count = 0; count < loginDisable.Length; count++) {
			loginDisable[count].SetActive(false);
		}	
	}
	IEnumerator LoginPlayer(string p_name, DateTime p_birthday){
		userName.text = p_name;
		loginUser.SetActive (true);

		yield return new WaitForSeconds (1.5f);

		loginUser.SetActive (false);
		userNameForVerification.text = userName.text;

		yield return new WaitForSeconds (0.1f);

		loginNotif.SetActive (true);  		
	}

	void CheckLoginBonus(){
		DateTime lastLogin = PlayerDataManager.Instance.LastUserLogin;

		if (DateTime.Compare (lastLogin.AddDays (1), DateTime.Today) == 0) {
			PlayerDataManager.Instance.curLoginBonus(PlayerDataManager.Instance.LogInBonus + 1);
			PlayerDataManager.Instance.lastLogin(DateTime.Today);
		} 
		else if (DateTime.Compare (lastLogin.AddDays (1), DateTime.Today) > 0) {
			//NothingHappens
		}
		else {
			PlayerDataManager.Instance.curLoginBonus(1);
			PlayerDataManager.Instance.lastLogin(DateTime.Today);
		}

	}

	void Login(){
		playerName = pNameInputBox.text == string.Empty ? "Default Guest" : pNameInputBox.text;
		playerBirthday = DateTime.Parse (pBirthdayInputBox[0].text + "/" + pBirthdayInputBox[1].text + "/" + pBirthdayInputBox[2].text);
		disableUserInput();
		CheckLoginBonus ();
		StartCoroutine (LoginPlayer (playerName, playerBirthday));
		
	}

	void FBLogin(){
		playerName = "Darren Diaz";
		playerBirthday = DateTime.Parse("12/12/1980");	
		disableUserInput();
		PlayerDataManager.Instance.AddChips (FBLoginBonus);
		CheckLoginBonus ();
		StartCoroutine (LoginPlayer (playerName, playerBirthday));
	}

	void LoginContinue(){
		PlayerDataManager.Instance.lastLogin (DateTime.Today);


		if (DateTime.Compare (PlayerDataManager.Instance.SpinBonusTimeLeft.AddDays(1), DateTime.Now) >= 0) {
			PlayerDataManager.Instance.lastBonusSpin(DateTime.Now);
			GameManager.Instance.LoadScene (_BonusSpin);
		}
		else {
			GameManager.Instance.LoadScene (_mainMenuScene);
		}

	}
}
