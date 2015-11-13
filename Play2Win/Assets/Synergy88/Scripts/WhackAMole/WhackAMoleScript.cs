using UnityEngine;
using System.Collections;

public class WhackAMoleScript : MonoBehaviour {

	private const string MOLE_STATE = "isHiding";
	private const string BG_GLOW = "BgGlow";
	private const string HIDE_BLUE = "BlueHide";
	private const string HIDE_RED = "RedHide";
	private const string HIDE_PURPLE = "PurpleHide";
	private const string HIDE_MONK = "MonkHide";
	private const string SHOW_RED = "RedPop";
	private const string SHOW_BLUE = "BluePop";
	private const string SHOW_PURPLE = "PurplePop";
	private const string SHOW_MONK = "MonkPop";
	private const string IDLE_RED = "RedIdle";
	private const string IDLE_BLUE = "BlueIdle";
	private const string IDLE_PURPLE = "PurpleIdle";
	private const string IDLE_MONK = "MonkIdle";
	private const string MONKEY_ATTACK_1 = "MonkeyAttack1";
	private const string MONKEY_ATTACK_2 = "MonkeyAttack2";
	private const string MONKEY_IDLE = "MonkeyIdle";
	private const string MONKEY_LOSE = "MonkeyLose";
	private const string MONKEY_WIN = "MonkeyWin";
	private const string SMOKE = "Smoke";
	private const string POW = "Pow";
	private const string SPEED_LINES = "SpeedLines";


	private int waitTime = 0;
	private int stateChanged = 0;
	private int moleIndex = 0;
	private int demonChosen = 0;

	private float stateDuration = 0.0f;
	private bool isHiding = true;
	private bool choose;

	[SerializeField]private tk2dSprite myMole;
	[SerializeField]private GameObject mole;
	[SerializeField] private GameObject effect;


	[SerializeField]private tk2dSpriteAnimator MoleState;
	[SerializeField]private tk2dSpriteAnimator smokeEffect;
	[SerializeField]private tk2dSpriteAnimator hitEffect;
	[SerializeField]private WhackAMoleManager myManager;

	IEnumerator CheckSpawnDemon(){
		mole.gameObject.SetActive(true);
		if (demonChosen == 0) {
			MoleState.Play (SHOW_BLUE);
			yield return new WaitForSeconds(0.3f);
			MoleState.Play(IDLE_BLUE);
		} 
		else if (demonChosen == 1) {
			MoleState.Play (SHOW_RED);
			yield return new WaitForSeconds(0.3f);
			MoleState.Play(IDLE_RED);
		} 
		else if (demonChosen == 2) {
			MoleState.Play (SHOW_MONK);
			yield return new WaitForSeconds(0.3f);
			MoleState.Play(IDLE_MONK);
		}
		else if (demonChosen == 3) {
			MoleState.Play (SHOW_PURPLE);
			yield return new WaitForSeconds(0.3f);
			MoleState.Play(IDLE_PURPLE);
		}
	}

	IEnumerator CheckHideDemon(){
		if (demonChosen == 0) {
			MoleState.Play (HIDE_BLUE);
			yield return new WaitForSeconds(0.3f);
			mole.gameObject.SetActive(false);
		} 
		else if (demonChosen == 1) {
			MoleState.Play (HIDE_RED);
			yield return new WaitForSeconds(0.3f);
			mole.gameObject.SetActive(false);
		} 
		else if (demonChosen == 2) {
			MoleState.Play (HIDE_MONK);
			yield return new WaitForSeconds(0.3f);
			mole.gameObject.SetActive(false);
		}
		else if (demonChosen == 3) {
			MoleState.Play (HIDE_PURPLE);
			yield return new WaitForSeconds(0.3f);
			mole.gameObject.SetActive(false);
		}
	}

	IEnumerator WhackTheMole(){
		choose = true;
		yield return new WaitForSeconds (waitTime);

		waitTime = 1;

		if (!myManager.gameover) {
			if(choose){
				stateDuration = Random.Range(1.5f, myManager.timer < 26.5f ? 3.5f : 30.0f - myManager.timer);
				stateChanged = Random.Range (0, 101);
				isHiding = stateChanged > 45 ? false : true;
				moleIndex = Random.Range (0, 100);
				choose = false;
			}
			CheckDemonToShow ();

			if (!isHiding)
				StartCoroutine(CheckSpawnDemon());

			yield return new WaitForSeconds (stateDuration);

			if (!isHiding){
				StartCoroutine(CheckHideDemon());
				yield return new WaitForSeconds(0.2f);
				isHiding = true;
			}


			StartCoroutine (WhackTheMole ());
		} 
		else 
			StartCoroutine(EndRound());
	}

	public void startWhack(){
		MoleState.Play (HIDE_BLUE);
		StartCoroutine (WhackTheMole());
	}

	IEnumerator EndRound(){
		MonkeyManagerScript.Instance.MonkeyWinEffect ();
		isHiding = true;
		CheckHideDemon ();
		yield return new WaitForSeconds (1.0f);
		CheckSpawnDemon ();
	}

	void CheckDemonToShow(){
		if (moleIndex >= 0 && moleIndex < 30) {
			//myMole.SetSprite(0);
			demonChosen = 0;
		} 
		else if (moleIndex >= 30 && moleIndex < 70) {
			//myMole.SetSprite(1);
			demonChosen = 1;
		}
		else if (moleIndex >= 70 && moleIndex < 90) {
			//myMole.SetSprite(2);
			demonChosen = 2;
		} 
		else if (moleIndex >= 90) {
			//myMole.SetSprite(3);
			demonChosen = 3;
		}
	}

	void CheckScoring(){
		int score = 0;
		if (demonChosen == 0)
			score = 50;
		else if (demonChosen == 1) 
			score = 100;
		else if (demonChosen == 2)
			score = -200;
		else if (demonChosen == 3) 
			score = 200;

		myManager.AddScore (score);
	}

	IEnumerator EnableEffect(){
		effect.gameObject.SetActive (true);
		smokeEffect.Play (SMOKE);
		hitEffect.Play (POW);
		yield return new WaitForSeconds (0.6f);
		effect.gameObject.SetActive (false);
	}

	void HideTheMole(){
		if (!isHiding) {
			if(demonChosen != 2){
				MonkeyManagerScript.Instance.MonkeyHitEffect();
				MonkeyManagerScript.Instance.MonkeyHit(true);
			}
			else if(demonChosen == 2){
				MonkeyManagerScript.Instance.MonkeyHit(false);
			}
			StartCoroutine(EnableEffect());
			isHiding = true;
			CheckScoring();
			StartCoroutine(CheckHideDemon());
		}
	}
}
