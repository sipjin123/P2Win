using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PatternSpot : MonoBehaviour {

	[SerializeField]
	private SlotItem _currentItem;

	public SlotItem CurrentItem { get { return _currentItem; } }
	
	void OnTriggerEnter(Collider other) {
		SlotItem item = other.GetComponent<SlotItem>();
		if (item != null) {
			_currentItem = item;
		}
	}
}
