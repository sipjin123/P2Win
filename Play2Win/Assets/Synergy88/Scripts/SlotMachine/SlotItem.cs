using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SlotItemType {
	ITEM_1,
	ITEM_2,
	ITEM_3,
	ITEM_4,
	ITEM_5,
	ITEM_6,
	ITEM_7,
	ITEM_8,
	BONUS,
	SCATTER,
	WILD,
	BOOSTER
}

[RequireComponent(typeof(tk2dSprite))]
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class SlotItem : MonoBehaviour {

	public static SlotItemType ITEM_CHEAT = SlotItemType.ITEM_1; // Normal items (1-8) are ignored

	private const string ITEM_NAME_PREFFIX = "item_0";
	private const int SLOT_ITEMS_MAX = 8;
	private const float BONUS_CHANCE = 0.05f;
	private const float SCATTER_CHANCE = 0.05f;
	private const float WILD_CHANCE = 0.05f;
	 
	[SerializeField]
	private Collider _randomizer;

//	[SerializeField]
//	private int _currentItemNumber;

	[SerializeField]
	private SlotItemType _currentItemType;

	private tk2dSprite _sprite;
	[SerializeField] private GameObject borderGlow;

	private Dictionary<SlotItemType, float> _itemChances;
	private float _maxRNGRange = 67.5f;

//	public int CurrentItemNumber { get { return _currentItemNumber; } }
	public SlotItemType CurrentItemType { get { return _currentItemType; } }
	public bool IsBonus { get { return _currentItemType == SlotItemType.BONUS || _currentItemType == SlotItemType.SCATTER || _currentItemType == SlotItemType.BOOSTER; } }


	private tk2dSpriteAnimator _animator;

	void Start() {
		_sprite = GetComponent<tk2dSprite>();
		_animator = borderGlow.GetComponent<tk2dSpriteAnimator> ();

		_itemChances = new Dictionary<SlotItemType, float>();
		_itemChances.Add(SlotItemType.ITEM_1, 0f);
		_itemChances.Add(SlotItemType.ITEM_2, 10f);
		_itemChances.Add(SlotItemType.ITEM_3, 20f);
		_itemChances.Add(SlotItemType.ITEM_4, 30f);
		_itemChances.Add(SlotItemType.ITEM_5, 40f);
		_itemChances.Add(SlotItemType.ITEM_6, 45f);
		_itemChances.Add(SlotItemType.ITEM_7, 50f);
		_itemChances.Add(SlotItemType.ITEM_8, 55f);
		_itemChances.Add(SlotItemType.BONUS, 60f);
		_itemChances.Add(SlotItemType.SCATTER, 62.5f);
		_itemChances.Add(SlotItemType.WILD, 65f);
        //_itemChances.Add(SlotItemType.BOOSTER, 67.5f);
	}

	public void Randomize() {
		_currentItemType = GetRandomItemType();
		_sprite.SetSprite(SlotItemName(_currentItemType));
	}

    public void Randomize_Editor(SlotItemType newType) {
        _currentItemType = newType;
        tk2dSprite sprite = GetComponent<tk2dSprite>();
        sprite.SetSprite(SlotItemName(_currentItemType));
    }

	void OnTriggerEnter(Collider other) {
		if (other == _randomizer) {
			Randomize();
		}
	}

	public void Shake() {
		borderGlow.SetActive (true);
		_animator.Play("BorderGlow");
	}

	public void Reset() {
		borderGlow.SetActive (false);
		_animator.Stop ();
	}

	private SlotItemType GetRandomItemType() {
		float rand = Random.Range(1f, _maxRNGRange);
		SlotItemType ret = SlotItemType.ITEM_1;
		foreach(SlotItemType type in _itemChances.Keys) {
			if (rand >= _itemChances[type] && _itemChances[type] > _itemChances[ret]) {
				ret = type;
			}
		}

		return ret;
	}

	public static string SlotItemName(SlotItemType type) {
		switch (type) {
		    case SlotItemType.ITEM_1: 
			    return "slot_item1";

		    case SlotItemType.ITEM_2:
                return "slot_item2";

		    case SlotItemType.ITEM_3:
                return "slot_item3";

		    case SlotItemType.ITEM_4:
                return "slot_item4";

		    case SlotItemType.ITEM_5:
                return "slot_item5";

		    case SlotItemType.ITEM_6:
                return "slot_item6";

		    case SlotItemType.ITEM_7:
                return "slot_item7";

		    case SlotItemType.ITEM_8:
                return "slot_item8";

		    case SlotItemType.BONUS:
                return "slot_bonus";

		    case SlotItemType.SCATTER:
                return "slot_scatter";

		    case SlotItemType.WILD:
                return "slot_wild";

		    case SlotItemType.BOOSTER: 
			    return "slot_booster";
		}

		return "";
	}

	public static SlotItemType GetTypeFromName(string name) {
		switch (name) {
            case "slot_item1": 
		        return SlotItemType.ITEM_1;

            case "slot_item2": 
			    return SlotItemType.ITEM_2;

            case "slot_item3": 
			    return SlotItemType.ITEM_3;

            case "slot_item4": 
			    return SlotItemType.ITEM_4;

            case "slot_item5": 
			    return SlotItemType.ITEM_5;

            case "slot_item6": 
			    return SlotItemType.ITEM_6;

            case "slot_item7": 
			    return SlotItemType.ITEM_7;

            case "slot_item8": 
			    return SlotItemType.ITEM_8;
			
		    case "slot_bonus":
			    return SlotItemType.BONUS;
			
		    case "slot_scatter": 
			    return SlotItemType.SCATTER;
			
		    case "slot_wild": 
			    return SlotItemType.WILD;

		    case "slot_booster":
			    return SlotItemType.BOOSTER;
		}
		
		return SlotItemType.WILD;
	}
}
