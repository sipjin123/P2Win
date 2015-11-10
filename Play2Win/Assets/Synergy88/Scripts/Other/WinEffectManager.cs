using UnityEngine;
using System.Collections;

public class WinEffectManager : MonoBehaviour {


	[SerializeField] private GameObject[] chipBurst;
	[SerializeField] private GameObject chipCollect;
	[SerializeField] private GameObject sparkleBurstCollector;
	[SerializeField] private GameObject hugeSparkleCollection;
	[SerializeField] private GameObject hugeWinCap;
	[SerializeField] private GameObject bonusWinCap;
	[SerializeField] private tk2dSpriteAnimator[] sparkleBurst;
	[SerializeField] private tk2dSpriteAnimator[] sparkleCollection;

	private Animator bonusWinCapAnimator;
	private Animator hugeWinCapAnimator;
 	

	public static WinEffectManager Instance { 
		get; 
		private set; 
	}
	
	void Awake () {
		WinEffectManager.Instance = this;
		bonusWinCapAnimator = bonusWinCap.GetComponent<Animator> ();
		hugeWinCapAnimator = hugeWinCap.GetComponent<Animator> ();
	}

	public void startHugeWin(){
		StartCoroutine (HugeWin ());
	}

	public void startCollectChip(){
		chipCollect.SetActive (true);
	}
	public void startBonusWin(){
		StartCoroutine (BonusWin ());
	}

	IEnumerator BonusWin(){
		bonusWinCap.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		chipBurst [0].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [1].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [2].SetActive (true);
		yield return new WaitForSeconds (0.3f);
		chipBurst [1].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [0].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [2].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [1].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [0].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		chipBurst [2].SetActive (true);
		yield return new WaitForSeconds (0.2f);
		bonusWinCapAnimator.SetBool ("Hide", true);
		
	}

	IEnumerator HugeWin(){
		hugeWinCap.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		sparkleBurstCollector.SetActive (true);
		for(int i = 0; i < sparkleBurst.Length; i++)
			sparkleBurst[i].Play("Sparkle");
		yield return new WaitForSeconds (1.0f);
		hugeSparkleCollection.SetActive (true);
		chipBurst [0].SetActive (true);
		chipBurst [2].SetActive (true);
		sparkleCollection[0].Play("Sparkle");
		yield return new WaitForSeconds (0.2f);
		sparkleCollection[1].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[2].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		chipBurst [1].SetActive (true);
		sparkleCollection[3].Play("Sparkle");
		yield return new WaitForSeconds (0.2f);
		sparkleCollection[4].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[5].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[6].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		chipBurst [0].SetActive (true);
		chipBurst [2].SetActive (true);
		sparkleCollection[7].Play("Sparkle");
		yield return new WaitForSeconds (0.2f);
		sparkleCollection[8].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[9].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		chipBurst [1].SetActive (true);
		sparkleCollection[10].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[11].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[12].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[13].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[14].Play("Sparkle");
		yield return new WaitForSeconds (0.1f);
		sparkleCollection[15].Play("Sparkle");
		hugeWinCapAnimator.SetBool ("Hide", true);
		yield return new WaitForSeconds (0.5f);
		hugeSparkleCollection.SetActive (false);
		sparkleBurstCollector.SetActive (false);

		
		

	}
}
