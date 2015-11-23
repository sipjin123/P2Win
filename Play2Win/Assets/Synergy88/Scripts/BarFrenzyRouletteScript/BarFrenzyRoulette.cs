using UnityEngine;
using System.Collections;

public class BarFrenzyRoulette : MonoBehaviour {

	[SerializeField] private tk2dSprite spinButton;
	[SerializeField] private tk2dUITweenItem buttonAnim;
	[SerializeField] private tk2dTextMesh spinLeft;

	[SerializeField] BFRouletteManager myManager; 

	private bool m_spin = true;
	private int spinCounter = 5;

	public void setSpinCounter(){
		spinCounter = 5;
		m_spin = true;
		spinButton.color = new Color (1.0f, 1.0f, 1.0f);
	}

	IEnumerator WheelSpin(){
		yield return new WaitForSeconds (0.3f);
		iTween.RotateBy (gameObject, new Vector3 (0, 0, Random.Range(-30.0f,31.0f)), 6);
		Invoke ("Finished",6.5f);
		yield return new WaitForSeconds (1.0f);
	}

	void spinTheWheel(){
		if (m_spin) {
			m_spin = false;
			//AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.STOP_REELS);
			spinCounter -= 1;
			spinLeft.text = spinCounter.ToString();
			buttonAnim.enabled = false;
			spinButton.color = new Color(0.5f,0.5f,0.5f);
			StartCoroutine (WheelSpin ());
		}
	}

	IEnumerator WaitForAnim(){
		myManager.AnimateCharacter ();
		yield return new WaitForSeconds (6.0f);
		myManager.CalculateScore();
		m_spin = true;
		buttonAnim.enabled = true;
		spinButton.color = new Color (1.0f, 1.0f, 1.0f);
	}


	void Finished(){
		if (spinCounter > 0) {
			StartCoroutine(WaitForAnim());
		} 
		else {
			myManager.FinishMiniGame();
		}
	}
}
