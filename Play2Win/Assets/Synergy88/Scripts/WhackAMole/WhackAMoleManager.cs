using UnityEngine;
using System.Collections;

public class WhackAMoleManager : MonoBehaviour {

	[SerializeField] private tk2dTextMesh scoreBoard;
	[SerializeField] private tk2dTextMesh multiplierBoard;
	[SerializeField] private tk2dTextMesh timerText;

	[SerializeField] private tk2dTextMesh scoreEarned;
	[SerializeField] private tk2dTextMesh multiplierEarned;
	[SerializeField] private tk2dTextMesh totalScore;
	[SerializeField] private tk2dTextMesh bonusEarned;
 
	[SerializeField] private GameObject[] moles;

	[SerializeField] private Animator whackBoard;

	private const string HIDE_SHOW_BOARD = "startgame";

	private int myScore;
	private int myMultiplier;

	public float timer;
	private bool gamestart;

	public bool gameover = false;

	void Update(){
		if (gamestart) {
			timer += 1.0f * Time.deltaTime;
			timerText.text = "Timer: " + Mathf.RoundToInt (timer).ToString ();
		}
	}

	IEnumerator startWhack(){
		yield return new WaitForSeconds (1.0f);
			for (int i = 0; i < moles.Length; i++) {
				moles[i].GetComponent<WhackAMoleScript>().startWhack();
			}
			gamestart = true;
		yield return new WaitForSeconds(30.0f);
			gameover = true;
			gamestart = false;
		yield return new WaitForSeconds (1.0f);
			scoreEarned.text = myScore.ToString();
			multiplierEarned.text = myMultiplier.ToString() + "X";
			totalScore.text = (myScore * myMultiplier).ToString ();
			bonusEarned.text = "0000";
		yield return new WaitForSeconds (1.0f);
			whackBoard.SetBool (HIDE_SHOW_BOARD, false);

	}

	public void AddScore(int p_points){
		myScore += p_points;
		if (myScore < 0) {
			myScore = 0;
		}
		UpdateScoreBoard (myScore);
	}
	public void AddMultiplier(int p_multiplier){
		UpdateMultiplier (p_multiplier);
	}

	private void UpdateScoreBoard(int p_score){
		scoreBoard.text = "Score: " + p_score.ToString();
	}
	private void UpdateMultiplier(int p_multiplier){
		myMultiplier = p_multiplier;
		multiplierBoard.text = "Multiplier: " + p_multiplier + "X";
	}

	void ShowHideBoard(){
		whackBoard.SetBool (HIDE_SHOW_BOARD, true);
		StartCoroutine (startWhack ());
	}

	void AcceptBonus(){
		Debug.Log ("Accepted");
		//Application.LoadLevel(1);
	}
}
  