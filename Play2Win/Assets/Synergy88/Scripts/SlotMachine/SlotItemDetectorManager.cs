using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct SlotItemPatternTable {

	public SlotItemType Type;

	public PatternRewardTable[] RewardTable;

	[System.Serializable]
	public struct PatternRewardTable {
		public int amount;
		public int reward;
	}
}

public class SlotItemDetectorManager : MonoBehaviour {

	[SerializeField]
	private SlotItemDetector[] _topItemDetectors;

	[SerializeField]
	private SlotItemDetector[] _midItemDetectors;

	[SerializeField]
	private SlotItemDetector[] _botItemDetectors;



	[SerializeField]
	private SlotItemPatternTable[] _regularItemRewards;

	[SerializeField]
	private SlotItemPatternTable _wildAnywhereReward;

	[SerializeField]
	private SlotItemPatternTable _bonusAnywhereReward;

	[SerializeField]
	private SlotItemPatternTable _scatterAnywhereReward;

	[SerializeField]
	private SlotItemPatternTable _boosterAnywhereReward;

	private Dictionary<SlotItemType, SlotItemPatternTable> _itemRewardTable;

	public SlotItemPatternTable WildRewardTable { get { return _wildAnywhereReward; } }
	public SlotItemPatternTable BonusRewardTable { get { return _bonusAnywhereReward; } }
	public SlotItemPatternTable ScatterRewardTable { get { return _scatterAnywhereReward; } }
	public SlotItemPatternTable BoosterRewardTable { get { return _boosterAnywhereReward; } }
	public Dictionary<SlotItemType, SlotItemPatternTable> ItemsRewardTable { get { return _itemRewardTable; } }


	void Start() {
		_itemRewardTable = new Dictionary<SlotItemType, SlotItemPatternTable>();
		for (int i = 0; i < _regularItemRewards.Length; i++) {
			_itemRewardTable.Add(_regularItemRewards[i].Type, _regularItemRewards[i]);
		}
	}

	public SlotItemDetector GetItem(int index, int position) {
		switch (position) {
		case 0:
			return _topItemDetectors[index];

		case 1:
			return _midItemDetectors[index];

		case 2:
			return _botItemDetectors[index];
		}
		return null;
	}

	public PatternReward GetBaseRewardFromPattern(SlotItemDetector[] detectors) {

		// Search for a pattern starting from the left ONLY. 
		// If the second item does not match, only a special item can be considered (ie: Wild)
		SlotItemType mainPatternType = SlotItemType.WILD;
        //SlotItemType previousType = SlotItemType.WILD;

		List<SlotItem> normalPatternItems = new List<SlotItem>();
		List<SlotItem> wildPatternItems = new List<SlotItem>();
		List<SlotItem> secondaryWildPatternItems = new List<SlotItem>(); // Only up to two wild pattern set can be formed from a 5-layered slot (ie: W-W-A-W-W)

		bool normalPatternEnded = false;
		bool wildPatternEnded = false;
		bool secondaryWildPatternEnded = false;

		for (int i = 0; i < detectors.Length; i++) {
			SlotItem item = detectors[i].GetItem();
			
			if (item == null) {
				Debug.LogWarning("Invalid Slot Item (null)", detectors[i].gameObject);
				continue;
			}

			if (mainPatternType == SlotItemType.WILD && !item.IsBonus) {
				mainPatternType = item.CurrentItemType;
			}

			// Update Wild pattern count
			if (item.CurrentItemType == SlotItemType.WILD && !item.IsBonus) {
				if (!wildPatternEnded) {
					wildPatternItems.Add(item);
				} 

				else if (!secondaryWildPatternEnded) {
					secondaryWildPatternItems.Add(item);
				} 

				else {
					if (!wildPatternEnded && wildPatternItems.Count > 0) {
						wildPatternEnded = true;
					} else if (!secondaryWildPatternEnded && secondaryWildPatternItems.Count > 0) {
						secondaryWildPatternEnded = true;
					}
				}
			} 
			else {
				if (!wildPatternEnded && wildPatternItems.Count > 0) {
					wildPatternEnded = true;
				} 

				else if (!secondaryWildPatternEnded && secondaryWildPatternItems.Count > 0) {
					secondaryWildPatternEnded = true;
				}
			}

			// Update normal pattern count
			if ((item.CurrentItemType == mainPatternType || item.CurrentItemType == SlotItemType.WILD)  && !item.IsBonus) {
				if (!normalPatternEnded) {
					normalPatternItems.Add(item);
				}
			} 
			else {
				normalPatternEnded = true;
			}
		}

		PatternReward patternReward = new PatternReward(false);

		// Get normal reward
		if (mainPatternType != SlotItemType.WILD) {
			for (int i = 0; i < _itemRewardTable[mainPatternType].RewardTable.Length; i++) {
				if (normalPatternItems.Count == _itemRewardTable[mainPatternType].RewardTable[i].amount) {
					patternReward.totalReward += _itemRewardTable[mainPatternType].RewardTable[i].reward;
					patternReward.items.AddRange(normalPatternItems);
					break;
				}
			}
		}

//		// Get wild and secondary wild reward
//		for (int i = 0; i < _wildAnywhereReward.RewardTable.Length; i++) {
//			if (wildPatternItems.Count == _wildAnywhereReward.RewardTable[i].amount) {
//				patternReward.totalReward += _wildAnywhereReward.RewardTable[i].reward;
//
//				foreach(SlotItem wildItem in wildPatternItems) {
//					if (!patternReward.items.Contains(wildItem)) {
//						patternReward.items.Add(wildItem);
//					}
//				}
//			}
//
//			if (secondaryWildPatternItems.Count == _wildAnywhereReward.RewardTable[i].amount) {
//				patternReward.totalReward += _wildAnywhereReward.RewardTable[i].reward;
//				
//				foreach(SlotItem wildItem in secondaryWildPatternItems) {
//					if (!patternReward.items.Contains(wildItem)) {
//						patternReward.items.Add(wildItem);
//					}
//				}
//			}
//		}

		return patternReward;


		// Old logic that searches regardless of order
//		// Organize the slot items
//		Dictionary<SlotItemType, List<SlotItem>> organizedItemList = new Dictionary<SlotItemType, List<SlotItem>>();
//		for (int i = 0; i < detectors.Length; i++) {
//			SlotItem item = detectors[i].GetItem();
//
//			if (item == null) {
//				Debug.LogWarning("Invalid Slot Item (null)", detectors[i].gameObject);
//				continue;
//			}
//
//			if (!organizedItemList.ContainsKey(item.CurrentItemType)) {
//				organizedItemList.Add(item.CurrentItemType, new List<SlotItem>());
//			}
//			organizedItemList[item.CurrentItemType].Add(item);
//		}
//
//		// Count Slot Items and check if this pattern won something
//		// -- Count wild cards
//		List<SlotItem> wildItems = new List<SlotItem>();
//		if (organizedItemList.ContainsKey(SlotItemType.WILD)) {
//			wildItems = organizedItemList[SlotItemType.WILD];
//		}
//
//		PatternReward patternReward = new PatternReward(false);
//
//		// -- Count regular item types only
//		foreach(SlotItemType type in organizedItemList.Keys) {
//
//			// Ignore global item types
//			if (type == SlotItemType.BONUS || 
//			    type == SlotItemType.BOOSTER || 
//			    type == SlotItemType.SCATTER || 
//			    type == SlotItemType.WILD) {
//				continue;
//			}
//
//			int totalCount = organizedItemList[type].Count + wildItems.Count;
//			for (int i = 0; i < _itemRewardTable[type].RewardTable.Length; i++) {
//				if (totalCount == _itemRewardTable[type].RewardTable[i].amount) {
//					patternReward.totalReward += _itemRewardTable[type].RewardTable[i].reward;
//					patternReward.items.AddRange(organizedItemList[type]);
//					break;
//				}
//			}
//		}
//
//		// -- Include the wild items last so they wont get added multiple times
//		if (patternReward.items.Count > 0) {
//			patternReward.items.AddRange(wildItems);
//		}
//		return patternReward;
	}

	/**
	 * For Wild Anywhere use only
	 **/
	public PatternReward CheckWildExtraReward() {
		PatternReward reward = new PatternReward(true);

		List<SlotItem> wildItems = GetAllAvailable(SlotItemType.WILD);
		int patternCount = wildItems.Count;
		if (patternCount > 5) { 
			patternCount = 5;
		}

		for (int i = 0; i < _wildAnywhereReward.RewardTable.Length; i++) {
			if (patternCount == _wildAnywhereReward.RewardTable[i].amount) {
				reward.totalReward = _wildAnywhereReward.RewardTable[i].reward;
				reward.items = wildItems;
				break;
			}
		}

		return reward;
	}

	public PatternReward CheckForSpinTheWheel() {
		PatternReward reward = new PatternReward(true);

		List<SlotItem> bonusItems = GetAllAvailable(SlotItemType.BONUS);
		int patternCount = bonusItems.Count;
		if (patternCount > 5) { 
			patternCount = 5;
		}

		for (int i = 0; i < _bonusAnywhereReward.RewardTable.Length; i++) {
			if (patternCount == _bonusAnywhereReward.RewardTable[i].amount) {
				reward.totalReward = _bonusAnywhereReward.RewardTable[i].reward;
				reward.items = bonusItems;
				break;
			}
		}
		
		return reward;
	}

	public PatternReward CheckScatterReward() {
		PatternReward reward = new PatternReward(true);
		
		List<SlotItem> scatterItems = GetAllAvailable(SlotItemType.SCATTER);
		int patternCount = scatterItems.Count;
		if (patternCount > 5) { 
			patternCount = 5;
		}

		for (int i = 0; i < _scatterAnywhereReward.RewardTable.Length; i++) {
			if (patternCount == _scatterAnywhereReward.RewardTable[i].amount) {
				reward.totalReward = _scatterAnywhereReward.RewardTable[i].reward;
				reward.items = scatterItems;
				break;
			}
		}
		
		return reward;
	}

	public PatternReward CheckBoosterReward() {
		PatternReward reward = new PatternReward(true);
		
		List<SlotItem> boosterItems = GetAllAvailable(SlotItemType.BOOSTER);
		int patternCount = boosterItems.Count;
		if (patternCount > 5) { 
			patternCount = 5;
		}

		for (int i = 0; i < _boosterAnywhereReward.RewardTable.Length; i++) {
			if (patternCount == _boosterAnywhereReward.RewardTable[i].amount) {
				reward.totalReward = _boosterAnywhereReward.RewardTable[i].reward;
				reward.items = boosterItems;
				break;
			}
		}
		
		return reward;
	}

	private List<SlotItem> GetAllAvailable(SlotItemType type) {
		List<SlotItem> retList = new List<SlotItem>();

		for (int i = 0; i < _topItemDetectors.Length; i++) {
			SlotItem item = _topItemDetectors[i].GetItem();
			if (item.CurrentItemType == type) {
				retList.Add(item);
			}
		}

		for (int i = 0; i < _midItemDetectors.Length; i++) {
			SlotItem item = _midItemDetectors[i].GetItem();
			if (item.CurrentItemType == type) {
				retList.Add(item);
			}
		}

		for (int i = 0; i < _botItemDetectors.Length; i++) {
			SlotItem item = _botItemDetectors[i].GetItem();
			if (item.CurrentItemType == type) {
				retList.Add(item);
			}
		}

		return retList;
	}

	/** Test script **/ 
//	void Update() {
//		if (Input.GetKeyDown(KeyCode.Y)) {
//			for (int i = 0; i < _topItemDetectors.Length; i++) {
//				_topItemDetectors[i].GetItem().Shake();
//			}
//		}
//
//		if (Input.GetKeyDown(KeyCode.U)) {
//			for (int i = 0; i < _midItemDetectors.Length; i++) {
//				_midItemDetectors[i].GetItem().Shake();
//			}
//		}
//
//		if (Input.GetKeyDown(KeyCode.I)) {
//			for (int i = 0; i < _botItemDetectors.Length; i++) {
//				_botItemDetectors[i].GetItem().Shake();
//			}
//		}
//
//		if (Input.GetKeyDown(KeyCode.H)) {
//			for (int i = 0; i < _topItemDetectors.Length; i++) {
//				_topItemDetectors[i].GetItem().Reset();
//			}
//		}
//		
//		if (Input.GetKeyDown(KeyCode.J)) {
//			for (int i = 0; i < _midItemDetectors.Length; i++) {
//				_midItemDetectors[i].GetItem().Reset();
//			}
//		}
//		
//		if (Input.GetKeyDown(KeyCode.K)) {
//			for (int i = 0; i < _botItemDetectors.Length; i++) {
//				_botItemDetectors[i].GetItem().Reset();
//			}
//		}
//	}
}
