using UnityEngine;
using System.Collections;

public class SpriteChanger : MonoBehaviour, ISignalListener
{
	[SerializeField]
	private GameObject GreenSettingsBTN;
	[SerializeField]
	private GameObject BlueSettingsBTN;

	void Start(){
		SignalManager.Instance.Register (this, SignalType.UPDATE_SETTINGSBTN_SPRITE);
		SignalManager.Instance.Register (this, SignalType.REVERT_SETTINGSBTN_SPRITE);

	}

	public void Execute(SignalType type, ISignalParameters param)
	{
		switch (type) {
		case SignalType.UPDATE_SETTINGSBTN_SPRITE:
				GreenSettingsBTN.SetActive(false);
				BlueSettingsBTN.SetActive(true);
			break;

		case SignalType.REVERT_SETTINGSBTN_SPRITE:
			GreenSettingsBTN.SetActive(true);
			BlueSettingsBTN.SetActive(false);
			break;

		default:
				GreenSettingsBTN.SetActive(true);
				BlueSettingsBTN.SetActive(false);
			break;
		}
	}

	void OnDestroy() {
		SignalManager.Instance.Remove (this, SignalType.UPDATE_SETTINGSBTN_SPRITE);
		SignalManager.Instance.Remove (this, SignalType.REVERT_SETTINGSBTN_SPRITE);
	}
}


