using UnityEngine;
using System.Collections;

public class BFRouletteManager : MonoBehaviour,IExtraRewardWindow {

	[SerializeField] tk2dTextMesh playerScore;
	[SerializeField] tk2dTextMesh enemyScore;

	[SerializeField] private tk2dTextMesh bonusChipsWon;
	[SerializeField] private tk2dTextMesh battleResultBoard;

	[SerializeField] BFHandScript enemyPin;
	[SerializeField] BFHandScript myPin;

	[SerializeField] BarFrenzyRoulette rouletteBody;
	[SerializeField] myCharacterManager myCharacter;
	[SerializeField]myCharacterManager myOpponent;

	[SerializeField] private tk2dSprite[] myDrinks;
	[SerializeField] private tk2dSprite[] opponentDrinks;
	[SerializeField] private GameObject[] pieGlow;
	[SerializeField] private GameObject myDrinkSparkle;
	[SerializeField] private GameObject opponentDrinkSparkle;

	private int myScore = 0;
	private int opponentScore = 0;
	private int myDrinkScore = 0;
	private int opponentDrinkScore = 0;
	private int chipsWon;
	private int battleResult;

	[SerializeField] private tk2dTextMesh PlayerGem;
	[SerializeField] private tk2dTextMesh PlayerChips;
	[SerializeField] private tk2dTextMesh PlayerLevel;
	[SerializeField] private tk2dSprite expBar;

	[SerializeField] private GameObject myCamera;
	[SerializeField] private GameObject gameBoard;

	[SerializeField]
	private GameState _backToLobby = GameState.MAIN_MENU;

	public void Show() {
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
					myDrinks [p_drinkScore + p_counter].spriteId = 50;
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

	void GetBattleResult (){
		if (myScore > opponentScore) {
			battleResult = 1;
			chipsWon = 1000 * (myScore - opponentScore);
			myCharacter.CharacterWin();
			myOpponent.CharacterLose("Opponent");

		} 
		else {
			battleResult = 2;
			chipsWon = 500;
			myCharacter.CharacterLose("Player");
			myOpponent.CharacterWin();
		}

	}

	void UpdateResultBoard(){
		battleResultBoard.text = battleResult == 1 ? "You Won!" : "You Lose!";
		bonusChipsWon.text = chipsWon.ToString ();

		gameBoard.SetActive (true);
	}

	IEnumerator WaitForAnim(){
		myPin.ActivateGlow ();
		yield return new WaitForSeconds (1.5f);
		myPin.DisableGlow ();
		myDrinkSparkle.SetActive (true);
		yield return  new WaitForSeconds (0.4f);
		myCharacter.StartMixing ("Player");
		yield return  new WaitForSeconds (0.4f);
		enemyPin.ActivateGlow ();
		yield return new WaitForSeconds (1.5f);
		enemyPin.DisableGlow ();
		opponentDrinkSparkle.SetActive (true);
		yield return new WaitForSeconds (0.4f);
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
