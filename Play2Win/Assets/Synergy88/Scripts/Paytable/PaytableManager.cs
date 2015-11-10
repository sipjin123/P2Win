using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaytableManager : MonoBehaviour {

	[SerializeField]
	private SlotItemDetectorManager _detectorManager;

	[SerializeField]
	private PaytableEntry[] _entries;

	private Dictionary<SlotItemType, PaytableEntry> _paytableItems;

	public void Load() {
		_paytableItems = new Dictionary<SlotItemType, PaytableEntry>();
		for (int i = 0; i < _entries.Length; i++) {
			_paytableItems.Add(_entries[i].Type, _entries[i]);
		}
	}

	public void Show() {
		gameObject.SetActive(true);

		foreach(SlotItemType key in _paytableItems.Keys) {
			switch (key) {
			case SlotItemType.BONUS:
				_paytableItems[key].UpdateValues(_detectorManager.BonusRewardTable);
				break;
			case SlotItemType.WILD:
				_paytableItems[key].UpdateValues(_detectorManager.WildRewardTable);
				break;
			case SlotItemType.SCATTER:
				_paytableItems[key].UpdateValues(_detectorManager.ScatterRewardTable);
				break;
			case SlotItemType.BOOSTER:
				// Skip this since it's not included in the pay table
				break;
			default:
				_paytableItems[key].UpdateValues(_detectorManager.ItemsRewardTable[key]);
				break;
			}
		}
	}

	public void Hide() {
		AudioManager.Instance.PlayGlobalAudio (AudioManager.GlobalAudioType.EXIT);
		gameObject.SetActive(false);
	}
}
