using UnityEngine;
using System.Collections;

public class BFRouletteManager : MonoBehaviour,IExtraRewardWindow {

	[SerializeField] tk2dTextMesh playerScore;
	[SerializeField] tk2dTextMesh enemyScore;

	[SerializeField] private tk2dTextMesh myScoreBoard;
	[SerializeField] private tk2dTextMesh opponentScoreBoard;
	[SerializeField] private tk2dTextMesh bonusChipsWon;
	[SerializeField] private tk2dTextMesh battleResultBoard;

	[SerializeField] BFHandScript enemyPin;
	[SerializeField] BFHandScript myPin;

	[SerializeField] BarFrenzyRoulette rouletteBody;

	private int myScore = 0;
	private int opponentScore = 0;
	private int chipsWon;
	private int battleResult;

	[SerializeField] private GameObject myCamera;
	[SerializeField] private GameObject gameBoard;

	public void Show() {
		myScore = 0;
		opponentScore = 0;
		myCamera.SetActive(true);
	}
	
	public void Hide() {
		myCamera.SetActive(false);
	}
	
	public void End() {
		PlayerDataManager.Instance.AddChips (myScore);
		Hide();
		SignalManager.Instance.Call(SignalType.EXTRA_REWARD_CLOSED);
	}

	public void CalculateScore(){
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
			chipsWon = 500;
		} 
		else {
			battleResult = 2;
			chipsWon = 250;
		}

	}

	void UpdateResultBoard(){
		opponentScoreBoard.text = opponentScore.ToString ();
		myScoreBoard.text = myScore.ToString();
		battleResultBoard.text = battleResult == 1 ? "You Won" : "You Lose";
		bonusChipsWon.text = chipsWon.ToString ();

		gameBoard.SetActive (true);
	}

	public void FinishMiniGame(){
		CalculateScore ();
		GetBattleResult ();
		UpdateResultBoard ();
	}

}
