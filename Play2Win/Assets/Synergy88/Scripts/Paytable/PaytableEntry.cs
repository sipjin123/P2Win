using UnityEngine;
using System.Collections;

public class PaytableEntry : MonoBehaviour {

	[SerializeField]
	private SlotItemType _type;

	[SerializeField]
	private tk2dTextMesh _value2;

	[SerializeField]
	private tk2dTextMesh _value3;

	[SerializeField]
	private tk2dTextMesh _value4;

	[SerializeField]
	private tk2dTextMesh _value5;

	[SerializeField]
	private tk2dTextMesh _label2;
	
	[SerializeField]
	private tk2dTextMesh _label3;
	
	[SerializeField]
	private tk2dTextMesh _label4;
	
	[SerializeField]
	private tk2dTextMesh _label5;

	public SlotItemType Type { get { return _type; } }

	public void UpdateValues(SlotItemPatternTable patternTable) {

		bool hasEntry2 = false;
		bool hasEntry3 = false;
		bool hasEntry4 = false;
		bool hasEntry5 = false;

		for (int i = 0; i < patternTable.RewardTable.Length; i++) {

			// Currently only supports values 2-5 because of layout limitations
			switch (patternTable.RewardTable[i].amount) {
			case 2:
				SetTextValue(_value2, patternTable.RewardTable[i].reward);
				hasEntry2 = true;
				break;
			case 3:
				SetTextValue(_value3, patternTable.RewardTable[i].reward);
				hasEntry3 = true;
				break;
			case 4:
				SetTextValue(_value4, patternTable.RewardTable[i].reward);
				hasEntry4 = true;
				break;
			case 5:
				SetTextValue(_value5, patternTable.RewardTable[i].reward);
				hasEntry5 = true;
				break;
			}
		}

		if (_label2 != null && _value2 != null) {
			_label2.gameObject.SetActive(hasEntry2);
			_value2.gameObject.SetActive(hasEntry2);
		}

		if (_label3 != null && _value3 != null) {
			_label3.gameObject.SetActive(hasEntry3);
			_value3.gameObject.SetActive(hasEntry3);
		}

		if (_label4 != null && _value4 != null) {
			_label4.gameObject.SetActive(hasEntry4);
			_value4.gameObject.SetActive(hasEntry4);
		}

		if (_label5 != null && _value5 != null) {
			_label5.gameObject.SetActive(hasEntry5);
			_value5.gameObject.SetActive(hasEntry5);
		}
	}

	private void SetTextValue(tk2dTextMesh textMesh, int value) {
		if (textMesh == null)
			return;

		textMesh.text = value.ToString();

		if (_type == SlotItemType.BONUS) {
			if (value == 1) {
				textMesh.text = "WIN";
			} else if (value > 1) {
				textMesh.text = "WIN x " + value.ToString();
			}
		}
	}

	public void AutoSetSerializedValues() {
		foreach(tk2dTextMesh text in GetComponentsInChildren<tk2dTextMesh>()) {
			switch (text.gameObject.name) {
			case "2_Value":
				_value2 = text;
				break;

			case "3_Value":
				_value3 = text;
				break;

			case "4_Value":
				_value4 = text;
				break;

			case "5_Value":
				_value5 = text;
				break;

			case "2_Amount":
				_label2 = text;
				break;

			case "3_Amount":
				_label3 = text;
				break;

			case "4_Amount":
				_label4 = text;
				break;

			case "5_Amount":
				_label5 = text;
				break;
			}
		}
	}

}
