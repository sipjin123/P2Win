using UnityEngine;
using System.Collections;

public class BFRouletteManager : MonoBehaviour,IExtraRewardWindow {

	[SerializeField] tk2dTextMesh playerScore;
	[SerializeField] tk2dTextMesh enemyScore;

	[SerializeField] private tk2dTextMesh bonusChipsWon;
	[SerializeField] private GameObject youLose;
	[SerializeField] private GameObject youWin;

	[SerializeField] BFHandScript enemyPin;
	[SerializeField] BFHandScript myPin;

	[SerializeField] BarFrenzyRoulette rouletteBody;
	[SerializeField] myCharacterManager myCharacter;
	[SerializeField]myCharacterManager myOpponent;

	[SerializeField] private tk2dSprite[] myDrinks;
	[SerializeField] private tk2dSprite[] opponentDrinks;
	[SerializeField] private GameObject myDrinkSparkle;
	[SerializeField] private GameObject opponentDrinkSparkle;

	private int myScore = 0;
	private int opponentScore = 0;
	private int myDrinkScore = 0;
	private int opponentDrinkScore = 0;
	private int chipsWon;

	[SerializeField] private tk2dTextMesh PlayerGem;
	[SerializeField] private tk2dTextMesh PlayerChips;
	[SerializeField] private tk2dTextMesh PlayerLevel;
	[SerializeField] private tk2dSprite expBar;

	[SerializeField] private GameObject myCamera;
	[SerializeField] private GameObject gameBoard;
	[SerializeField] private tk2dSpriteAnimator[] winSparkle;

	[SerializeField]
	private GameState _backToLobby = GameState.MAIN_MENU;

	public void Show() {
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.BARFRENZY_MINIGAMEBGM);
		PlayerGem.text = PlayerDataManager.Instance.Points.ToString();
		PlayerChips.text = PlayerDataManager.Instance.Chips.ToString();
		PlayerLevel.text = PlayerDataManager.Instance.Level.ToString ();
		expBar.scale = new Vector3(PlayerDataManager.Instance.ExpRatio, 1f, 1f);
		myCharacter.ResetCharacter ();
		myOpponent.ResetCharacter ();
		myScore = 0;
		opponentScore = 0;
		myDrinkScore = 0;
		opponentDrinkScore = 0;
		UpdateScoreBoard ();
		gameBoard.SetActive (false);
		rouletteBody.setSpinCounter ();
		myCamera.SetActive(true);
		winSparkle [0].gameObject.SetActive (false);
		winSparkle [1].gameObject.SetActive (false);
		for (int i = 0; i < myDrinks.Length; i ++) {
			myDrinks[i].gameObject.SetActive(false);
			opponentDrinks[i].gameObject.SetActive(false);
		}
	}

	void BackToLobby(){
		GameManager.Instance.LoadScene (_backToLobby);
	}

	public void Hide() {
		myCamera.SetActive(false);
	}
	
	public void End() {
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SELECT);
		PlayerDataManager.Instance.AddChips (myScore);
		Hide();
		SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
	}

	void ChangeDrinks(int p_drinks,int p_counter,int p_drinkScore,bool isPlayer){
		if (isPlayer) {
			if (p_drinks == 1) {
				myDrinks [p_drinkScore + p_counter].spriteId = 17;
			} 
			else if (p_drinks == 2) {
				if (p_counter == 0)
					myDrinks [p_drinkScore + p_counter].spriteId = 51;
				else if (p_counter == 1)
					myDrinks [p_drinkScore + p_counter].spriteId = 50;
			} 
			else if (p_drinks == 3) {
				if (p_counter == 0)
					myDrinks [p_drinkScore + p_counter].spriteId = 51;
				else if (p_counter == 1)
					myDrinks [p_drinkScore + p_counter].spriteId = 15;
				else if (p_counter == 2)
					myDrinks [p_drinkScore + p_counter].spriteId = 50;
			} 
			else if (p_drinks == 4) {
				if (p_counter == 0)
					myDrinks [p_drinkScore + p_counter].spriteId = 51;
				else if (p_counter == 1)
					myDrinks [p_drinkScore + p_counter].spriteId = 4;
				else if (p_counter == 2)
					myDrinks [p_drinkScore + p_counter].spriteId = 23;
				else if (p_counter == 3)
					myDrinks [p_drinkScore + p_counter].spriteId = 67;
			}
		} 
		else {
			if (p_drinks == 1) {
				opponentDrinks [p_drinkScore + p_counter].spriteId = 17;
			} 
			else if (p_drinks == 2) {
				if (p_counter == 0)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 51;
				else if (p_counter == 1)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 15;
			} 
			else if (p_drinks == 3) {
				if (p_counter == 0)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 51;
				else if (p_counter == 1)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 15;
				else if (p_counter == 2)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 50;
			} 
			else if (p_drinks == 4) {
				if (p_counter == 0)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 51;
				else if (p_counter == 1)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 4;
				else if (p_counter == 2)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 23;
				else if (p_counter == 3)
					opponentDrinks [p_drinkScore + p_counter].spriteId = 50;
			}
		}
	}
	IEnumerator EnableEnemyDrinks(int p_drinks){
		for(int i = 0; i < p_drinks; i++){
			ChangeDrinks(p_drinks,i,opponentDrinkScore,false);
			opponentDrinks[opponentDrinkScore + i].gameObject.SetActive(true);
			yield return new WaitForSeconds(0.25f);
		}
		opponentDrinkScore += p_drinks;
	}
	IEnumerator EnableMyDrinks(int p_drinks){
		for(int i = 0; i < p_drinks; i++){
			ChangeDrinks(p_drinks,i,myDrinkScore,true);
			myDrinks[myDrinkScore + i].gameObject.SetActive(true);
			yield return new WaitForSeconds(0.25f);
		}
		myDrinkScore += p_drinks;
	}

	public void CalculateScore(){

		StartCoroutine (EnableEnemyDrinks (enemyPin.getroullettePrice()));
		StartCoroutine (EnableMyDrinks (myPin.getroullettePrice ()));

		opponentScore += enemyPin.getroullettePrice();
		myScore += myPin.getroullettePrice();

		UpdateScoreBoard();
	}

	void UpdateScoreBoard(){
		playerScore.text = "MyScore: " + myScore.ToString();
		enemyScore.text = opponentScore.ToString() + " :Opponent";

	}

	IEnumerator ShowSparkle(int p_sender){
		for (int i = 0; i < 5; i++) {
			winSparkle [p_sender].gameObject.SetActive (true);
			winSparkle[p_sender].Play("WinSparkle");
			yield return new WaitForSeconds (0.75f);
			winSparkle [p_sender].gameObject.SetActive (false);
			yield return new WaitForSeconds(1.25f);
		}
	}

	void GetBattleResult (){
		if (myScore > opponentScore) {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_WINANIMATION);
			chipsWon = 1000 * (myScore - opponentScore);
			myCharacter.CharacterWin();
			myOpponent.CharacterLose("Opponent");
			youLose.SetActive(false);
			youWin.SetActive(true);
		} 
		else {
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_MINIGAMELOSE);
			youLose.SetActive(true);
			youWin.SetActive(false);
			chipsWon = 500;
			myCharacter.CharacterLose("Player");
			myOpponent.CharacterWin();
		}
		StartCoroutine (ShowSparkle (myScore > opponentScore ? 0 : 1));
	}

	void UpdateResultBoard(){
		bonusChipsWon.text = chipsWon.ToString ();
		AudioManager.Instance.StopGlobalAudio (AudioManager.GlobalAudioType.BARFRENZY_WINANIMATION);
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_CONGRATULATIONPOP);
		gameBoard.SetActive (true);
	}

	IEnumerator WaitForAnim(){
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.BARFRENZY_DRINKSELECT);
		myPin.ActivateGlow ();
		yield return new WaitForSeconds (1.5f);
		myPin.DisableGlow ();
		myDrinkSparkle.SetActive (true);
		yield return  new WaitForSeconds (0.4f);
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.BARFRENZY_POURDRINK);
		myCharacter.StartMixing ("Player");
		yield return  new WaitForSeconds (0.4f);
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.BARFRENZY_DRINKSELECT);
		enemyPin.ActivateGlow ();
		yield return new WaitForSeconds (1.5f);
		enemyPin.DisableGlow ();
		opponentDrinkSparkle.SetActive (true);
		yield return new WaitForSeconds (0.4f);
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.BARFRENZY_POURDRINK);
		myOpponent.StartMixing("Opponent");
	}

	public void AnimateCharacter(){
		StartCoroutine (WaitForAnim ());
	}

	IEnumerator GameOver(){
		AnimateCharacter ();
		yield return new WaitForSeconds (7.0f);
		CalculateScore ();
		GetBattleResult ();
		yield return new WaitForSeconds (2.0f);
		UpdateResultBoard ();
	}
	public void FinishMiniGame(){
		StartCoroutine (GameOver ());

	}

}
