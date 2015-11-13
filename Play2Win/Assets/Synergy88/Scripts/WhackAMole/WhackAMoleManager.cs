using UnityEngine;
using System.Collections;

public class WhackAMoleManager : MonoBehaviour, IExtraRewardWindow  {

	[SerializeField] private GameObject myCamera;
	[SerializeField] private tk2dTextMesh scoreBoard;
	[SerializeField] private tk2dTextMesh multiplierBoard;
	[SerializeField] private tk2dTextMesh timerText;

	[SerializeField] private tk2dTextMesh scoreEarned;
	[SerializeField] private tk2dTextMesh multiplierEarned;
	[SerializeField] private tk2dTextMesh totalScore;
 
	[SerializeField] private GameObject[] moles;

	[SerializeField] private Animator whackBoard;

	private const string HIDE_SHOW_BOARD = "startgame";

	private int myScore;
	private int myMultiplier = 1;
	private int coin = 0;

	public float timer;
	private bool gamestart;

	public bool gameover = false;

	void Update(){
		if (gamestart) {
			timer += 1.0f * Time.deltaTime;
			timerText.text = Mathf.RoundToInt (timer).ToString ();
		}
	}

	public void SetMultiplier(int p_multiplier){
		myMultiplier = p_multiplier;
		multiplierBoard.text = myMultiplier.ToString();
	}

	public void SetCoins(float p_coin){
		coin = (int)p_coin;
	}

	public void Show() {
		gamestart = false;
		timer = 0.0f;
		myScore = 0;
		scoreBoard.text = "0";
		gameover = false;
		whackBoard.SetBool ("Reset", true);
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
			totalScore.text = (myScore * myMultiplier * coin).ToString ();
			myScore = myScore * myMultiplier * coin;
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

	private void UpdateScoreBoard(int p_score){
		scoreBoard.text = p_score.ToString();
	}

	void ShowHideBoard(){
		whackBoard.SetBool (HIDE_SHOW_BOARD, true);
		whackBoard.SetBool ("Reset", false);
		StartCoroutine (startWhack ());
	}
}
  