using UnityEngine;
using System.Collections;

public class BarFrenzyRoulette : MonoBehaviour {

	[SerializeField] private tk2dSprite spinButton;
	[SerializeField] private tk2dUITweenItem buttonAnim;
	[SerializeField] private tk2dTextMesh spinLeft;

	[SerializeField] BFRouletteManager myManager; 

	private bool m_spin = true;
	private int spinCounter = 5;

	public void setCounter(){
		spinCounter = 0;
	}

	IEnumerator WheelSpin(){
		yield return new WaitForSeconds (0.3f);
		iTween.RotateBy (gameObject, new Vector3 (0, 0, Random.Range(-30.0f,31.0f)), 6);
		Invoke ("Finished",6.5f);
		yield return new WaitForSeconds (1.0f);
		m_spin = false;
	}

	void spinTheWheel(){
		if (m_spin & spinCounter > 0) {
			spinCounter -= 1;
			spinLeft.text = "Spin Left: " + spinCounter.ToString();
			//AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.STOP_REELS);
			buttonAnim.enabled = false;
			spinButton.color = new Color(0.5f,0.5f,0.5f);
			StartCoroutine (WheelSpin ());
		}
	}

	void Finished(){
		if (spinCounter > 0) {
			myManager.CalculateScore();
			m_spin = true;
			buttonAnim.enabled = true;
			spinButton.color = new Color (1.0f, 1.0f, 1.0f);
		} 
		else {
			myManager.FinishMiniGame();
		}
	}
}
