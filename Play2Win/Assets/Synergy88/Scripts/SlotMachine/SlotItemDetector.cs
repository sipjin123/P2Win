using UnityEngine;
using System.Collections;

public class SlotItemDetector : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(transform.position, 0.2f);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.position, 0.2f);
	}


	public SlotItem GetItem() {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast(transform.position, fwd, out hit, 10f)) {
			SlotItem slotItem = (SlotItem)hit.collider.gameObject.GetComponent<SlotItem>();
			if (slotItem != null) {
				return slotItem;
			}
		}

		Debug.LogWarning("SlotItemDetector " + gameObject.name + " does not detect any slot item!", this.gameObject);
		return null;
	}
}
