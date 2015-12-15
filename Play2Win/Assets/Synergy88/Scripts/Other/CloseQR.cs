using UnityEngine;
using System.Collections;

public class CloseQR : MonoBehaviour {

	[SerializeField]
	private tk2dSpriteAnimator qrcode;
	[SerializeField]
	private GameObject backDrop;
	[SerializeField]
	private GameObject myCamera;

	void CloseOpenQRCOde(){
		qrcode.gameObject.SetActive (false);
		this.gameObject.SetActive (false);
		backDrop.SetActive (false);
	}

	IEnumerator OpenQRCode(){
		myCamera.SetActive (true);
		backDrop.SetActive (true);
		
		yield return new WaitForSeconds(0.1f);
		
		qrcode.gameObject.SetActive (true);
		qrcode.Play("QRFlip");
	}

	public void OpenCode(){
		StartCoroutine (OpenQRCode ());
	}
}
