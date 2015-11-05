using UnityEngine;
using System.Collections;

public class TriggerClick : MonoBehaviour {

	void OnMouseDown(){
		ClickEffect.Instance.ButtonClickEffect (this.gameObject.transform.position);
	}
}
