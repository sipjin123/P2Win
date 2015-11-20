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

	[SerializeField] private GameObject[] myDrinks;
	[SerializeField] private GameObject[] opponentDrinks;
	[SerializeField] private GameObject[] pieGlow;
	[SerializeField] private GameObject myDrinkSparkle;
	[SerializeField] private GameObject opponentDrinkSparkle;

	private int myScore = 0;
	private int opponentScore = 0;
	private int myDrinkScore = 0;
	private int opponentDrinkScore = 0;
	private int chipsWon;
	private int battleResult;

	[SerializeField] private GameObject myCamera;
	[SerializeField] private GameObject gameBoard;

	public void Show() {
		myScore = 0;
		opponentScore = 0;
		myDrinkScore = 0;
		opponentDrinkScore = 0;
		myCamera.SetActive(true);
		for (int i = 0; i < myDrinks.Length; i ++) {
			myDrinks[i].SetActive(false);
			opponentDrinks[i].SetActive(false);
		}
	}
	
	public void Hide() {
		myCamera.SetActive(false);
	}
	
	public void End() {
		PlayerDataManager.Instance.AddChips (myScore);
		Hide();
		SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
	}

	IEnumerator EnableEnemyDrinks(int p_drinks){
		for(int i = 0; i < p_drinks; i++){
			opponentDrinks[opponentDrinkScore + i].SetActive(true);
			yield return new WaitForSeconds(0.25f);
		}
		opponentDrinkScore += p_drinks;
	}
	IEnumerator EnableMyDrinks(int p_drinks){
		for(int i = 0; i < p_drinks; i++){
			myDrinks[myDrinkScore + i].SetActive(true);
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
		} 
		else {
			battleResult = 2;
			chipsWon = 500;
		}

	}

	void UpdateResultBoard(){
		battleResultBoard.text = battleResult == 1 ? "You Won!" : "You Lose!";
		bonusChipsWon.text = chipsWon.ToString ();

		gameBoard.SetActive (true);
	}

	IEnumerator WaitForAnim(){
		myDrinkSparkle.SetActive (true);
		yield return  new WaitForSeconds (0.5f);
		myCharacter.StartMixing ("Player");
		yield return  new WaitForSeconds (0.5f);
		opponentDrinkSparkle.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		myOpponent.StartMixing("Opponent");
	}

	public void AnimateCharacter(){
		StartCoroutine (WaitForAnim ());
	}

	IEnumerator GameOver(){
		AnimateCharacter ();
		yield return new WaitForSeconds (3.5f);
		CalculateScore ();
		GetBattleResult ();
		UpdateResultBoard ();
	}
	public void FinishMiniGame(){
		StartCoroutine (GameOver ());

	}

}
