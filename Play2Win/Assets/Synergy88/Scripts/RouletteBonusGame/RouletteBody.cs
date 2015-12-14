using UnityEngine;
using System.Collections;

public class RouletteBody : MonoBehaviour {

	[SerializeField]
	private GameObject rouletteManager;

	IEnumerator WheelSpin(){
		yield return new WaitForSeconds (0.3f);
		iTween.RotateBy (gameObject, new Vector3 (0, 0, Random.Range(-180.0f,181.0f)), 6);
		Invoke ("Finished",6.5f);
//		yield return new WaitForSeconds (1.0f);
//		m_spin = false;
	}

	public void spinTheWheel(){
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.STOP_REELS);
//			buttonAnim.enabled = false;
//			spinButton.color = new Color(0.5f,0.5f,0.5f);
			StartCoroutine (WheelSpin ());
	}
	void Finished(){
		rouletteManager.GetComponent<BonusRouletteManager>().RotateFinished();
	}
}
