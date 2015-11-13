using UnityEngine;
using System.Collections;

public class MonkeyManagerScript : MonoBehaviour {

	[SerializeField]private GameObject MonkeyIdle;
	[SerializeField]private GameObject MonkeyAttack;
	[SerializeField]private GameObject MonkeyLose;
	[SerializeField]private GameObject MonkeyWin;
	[SerializeField]private GameObject BgGlow;
	[SerializeField]private GameObject SpeedLine;
	[SerializeField]private GameObject[] hitEffect;

	[SerializeField]private tk2dSpriteAnimator[] hitAnim;
	[SerializeField] private tk2dSpriteAnimator glow;
	[SerializeField]private tk2dSpriteAnimator lineSpeed;
	[SerializeField] private tk2dSpriteAnimator idle_monkey;
	[SerializeField] private tk2dSpriteAnimator attack_monkey;
	[SerializeField] private tk2dSpriteAnimator lose_monkey;

	private const string POW = "Pow";
	private const string SPEED_LINES = "SpeedLines";
	private const string BG_GLOW = "BgGlow";
	private const string MONKEY_ATTACK_1 = "MonkeyAttack1";
	private const string MONKEY_ATTACK_2 = "MonkeyAttack2";
	private const string MONKEY_LOSE = "MonkeyLose";

	private int hitCount = 0;

	public static MonkeyManagerScript Instance { 
		get; 
		private set; 
	}

	void Awake(){
		MonkeyManagerScript.Instance = this;
	}

	IEnumerator checkAction(bool p_attack){
		yield return new WaitForSeconds (0.5f);
		if (p_attack) {
			MonkeyAttack.SetActive (false);
		} 
		else {
			MonkeyLose.SetActive(false);
		}
		if (!MonkeyAttack.activeSelf && !MonkeyLose.activeSelf)
			MonkeyIdle.SetActive (true);
			
	}

	public void MonkeyHit(bool p_attack){
		if (MonkeyIdle.activeSelf) {
			MonkeyIdle.SetActive (false);
			if (p_attack) {
				MonkeyAttack.SetActive (true);
				if (hitCount == 2) {
					attack_monkey.Play (MONKEY_ATTACK_1);
					hitCount = 1;
				} else {
					attack_monkey.Play (MONKEY_ATTACK_2);
					hitCount = 0;
				}
			} else {
				MonkeyLose.SetActive (true);
				lose_monkey.Play (MONKEY_LOSE);
			}
			StartCoroutine (checkAction (p_attack));
		}
	}

	public void MonkeyWinEffect(){
		MonkeyIdle.SetActive (false);
		if (!MonkeyWin.activeSelf) {
			MonkeyAttack.SetActive (false);
			MonkeyLose.SetActive (false);
			MonkeyWin.SetActive (true);
		}
	}

	IEnumerator BgGlowEffect(){
		BgGlow.SetActive (true);
		glow.Play (BG_GLOW);
		yield return new WaitForSeconds (0.6f);
		BgGlow.SetActive (false);
	}
	IEnumerator Hit(){
		hitEffect [0].SetActive (true);
		hitAnim [0].Play (POW);
		yield return new WaitForSeconds (0.1f);
		hitEffect [1].SetActive (true);
		hitAnim [1].Play (POW);
		yield return new WaitForSeconds (0.1f);
		hitEffect [2].SetActive (true);
		hitAnim [2].Play (POW);
		yield return new WaitForSeconds (0.3f);
		for (int i = 0; i < hitEffect.Length; i++)
			hitEffect [i].SetActive (false);
	}
	IEnumerator SpeedLineEffect(){
		SpeedLine.SetActive (true);
		lineSpeed.Play (SPEED_LINES);
		yield return new WaitForSeconds (0.5f);
		SpeedLine.SetActive (false);
	}


	public void MonkeyHitEffect(){
		if (!BgGlow.activeSelf) {
			StartCoroutine(BgGlowEffect());
		}
		if (!SpeedLine.activeSelf) {
			StartCoroutine(SpeedLineEffect());
		}
		if (!hitEffect[2].activeSelf) {
			StartCoroutine(Hit());
		}
	}
}
