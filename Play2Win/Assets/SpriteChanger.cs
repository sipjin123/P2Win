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

	}

	public void Execute(SignalType type, ISignalParameters param)
	{
		switch (type) {
		case SignalType.UPDATE_SETTINGSBTN_SPRITE:
			GreenSettingsBTN.SetActive(!GreenSettingsBTN.activeSelf);
			BlueSettingsBTN.SetActive(!BlueSettingsBTN.activeSelf);
			break;

		default:
			if (Application.loadedLevelName != "TigerSlots") {
				GreenSettingsBTN.SetActive(true);
				BlueSettingsBTN.SetActive(false);
			}
			break;
		}
	}

	void OnDestroy() {
		SignalManager.Instance.Remove (this, SignalType.UPDATE_SETTINGSBTN_SPRITE);
	}
}


