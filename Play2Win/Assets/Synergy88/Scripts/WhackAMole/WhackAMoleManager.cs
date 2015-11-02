using UnityEngine;
using System.Collections;

public class WhackAMoleManager : MonoBehaviour {

	[SerializeField] private tk2dTextMesh scoreBoard;
	[SerializeField] private tk2dTextMesh multiplierBoard;
	[SerializeField] private tk2dTextMesh timerText;
	private int myScore;
	private float timer;

	public bool gameover = false;

	void Update(){
		if (!gameover) {
			timer += 1.0f * Time.deltaTime;
			timerText.text = "Timer: " + Mathf.RoundToInt (timer).ToString ();
		}
		if (timer > 30) {
			gameover = true;
		}
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
		multiplierBoard.text = "Multiplier: " + p_multiplier + "X";
	}
}
  