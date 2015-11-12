using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollManager : MonoBehaviour {

	[SerializeField] private GameObject[] scrollItem;
	private tk2dUIScrollableArea scrollLength;

	private const float startPos = -7.4f;
	private const float startLength = 1.4f;
	private const float addLength = 2.95f;
	private const float addPos = 3.0f;
	private const float minItem = 4.0f;

	void Start(){
		scrollLength = this.GetComponent<tk2dUIScrollableArea> ();
		StartCoroutine (ArrangeScrollList());
	}

	IEnumerator ArrangeScrollList(){
		for (int i = 0; i < scrollItem.Length; i++) {
			scrollItem[i].transform.localPosition = new Vector3(startPos + (addPos * i),-3.13f,0.0f);
		}
		yield return new WaitForSeconds (1.0f);

		if (scrollItem.Length > 4) {
			scrollLength.ContentLength = (float)(startLength + (scrollItem.Length - minItem) * addLength);
		}
	}
}
