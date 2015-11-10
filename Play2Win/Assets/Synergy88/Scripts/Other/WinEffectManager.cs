using UnityEngine;
using System.Collections;

public class WinEffectManager : MonoBehaviour {


	[SerializeField] private GameObject[] chipBurst;
	[SerializeField] private tk2dSpriteAnimator[] sparkleBurst;
	[SerializeField] private GameObject chipCollect;
	[SerializeField] private GameObject sparkleBurstCollector;


	public static WinEffectManager Instance { 
		get; 
		private set; 
	}
	
	void Awake () {
		WinEffectManager.Instance = this;
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
		yield return new WaitForSeconds (1.0f);
	}

	IEnumerator HugeWin(){
		sparkleBurstCollector.SetActive (true);
		for(int i = 0; i < sparkleBurst.Length; i++)
			sparkleBurst[i].Play("Sparkle");
		yield return new WaitForSeconds (1.0f);
	}
}
