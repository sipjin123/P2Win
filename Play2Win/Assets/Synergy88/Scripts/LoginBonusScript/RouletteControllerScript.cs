using UnityEngine;
using System.Collections;

public class RouletteControllerScript : MonoBehaviour {

	[SerializeField] private GameObject rouletteHand;
	[SerializeField] private GameObject claimRewardWindow;
	[SerializeField] private GameObject roulettePin;
	[SerializeField] private GameObject anotherSpin;
	[SerializeField] private GameObject[] priceValue;

	[SerializeField] private tk2dTextMesh prizeWon;
	[SerializeField] private tk2dSprite spinButton;
	[SerializeField] private tk2dTextMesh bonusCoin;
	[SerializeField] private tk2dTextMesh multiplier;
 	[SerializeField] private tk2dUITweenItem buttonAnim;
	
	private RouletteHandScript pinScript;
	private bool m_spin = true;	
	private int prizeMultiplier;
	private int totalPrize;

	void Start(){
		pinScript = roulettePin.GetComponent<RouletteHandScript> ();
		prizeMultiplier = 1;
	}

	IEnumerator WheelSpin(){
		yield return new WaitForSeconds (0.3f);
		iTween.RotateBy (gameObject, new Vector3 (0, 0, Random.Range(-30.0f,31.0f)), 6);
		Invoke ("Finished",6.5f);
		yield return new WaitForSeconds (1.0f);
		m_spin = false;
	}
	IEnumerator AnotherSpin(){
		anotherSpin.SetActive (true);

		yield return new WaitForSeconds (0.3f);

		for(int count = 0; count < priceValue.Length; count++){
			if(count == 0 || count == 1)
				priceValue[count].SetActive(false);
			else
				priceValue[count].SetActive(true);
		}

		yield return new WaitForSeconds (3.0f);

		anotherSpin.SetActive (false);

		spinButton.color = new Color (1.0f, 1.0f, 1.0f);
		buttonAnim.enabled = true;
		m_spin = true;
	}
	
	void spinTheWheel(){
		if (m_spin) {
			buttonAnim.enabled = false;
			spinButton.color = new Color(0.5f,0.5f,0.5f);
			StartCoroutine (WheelSpin ());
		}
	}

	void Reload(){
		Application.LoadLevel (0);
	}
	
	void Finished()
	{
		if (pinScript.rouletteTextPrice != "2X" && pinScript.rouletteTextPrice != "3X") {
			totalPrize = pinScript.roulettePrice * prizeMultiplier;
			bonusCoin.text = "$" + totalPrize.ToString();
			prizeWon.text = "$" + totalPrize.ToString();
			claimRewardWindow.SetActive (true);
		} 
		else {
			prizeMultiplier = pinScript.rouletteTextPrice == "2X" ? 2 : 3;
			multiplier.text = pinScript.rouletteTextPrice + " multiplier";
			StartCoroutine(AnotherSpin());
		}
	}

}
