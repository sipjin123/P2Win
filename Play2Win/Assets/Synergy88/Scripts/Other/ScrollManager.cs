using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollManager : MonoBehaviour {

	[SerializeField] private GameObject[] scrollItem;

    private List<GameObject> scrollItemList;

	private tk2dUIScrollableArea scrollLength;

	private const float startPos = -7.4f;
	private const float startLength = 1.4f;
	private const float addLength = 2.95f;
	private const float addPos = 3.0f;
	private const float minItem = 4.0f;

    void Awake() {
        scrollItemList = new List<GameObject>();
        scrollLength = this.GetComponent<tk2dUIScrollableArea>();
    }

    public void AddObject(GameObject newObject) {
        scrollItemList.Add(newObject);
    }

    public void ActivateScrollList() {
        StartCoroutine(ArrangeScrollList());
    }

	IEnumerator ArrangeScrollList(){
        for (int i = 0; i < scrollItemList.Count; i++) {
            scrollItemList[i].transform.localPosition = new Vector3(startPos + (addPos * i), -3.13f, 0.0f);
		}
		yield return new WaitForSeconds (1.0f);

        if (scrollItemList.Count > 4) {
            scrollLength.ContentLength = (float)(startLength + (scrollItemList.Count- minItem) * addLength);
		}
	}
}
