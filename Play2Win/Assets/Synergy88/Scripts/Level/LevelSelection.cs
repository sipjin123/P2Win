using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct LevelRequirements {
	public string name;
	public int slotLevel;
	public int minimumPlayerLevel;
	public bool isIAP;
	public Image lockSprite;
	public Image downloadSprite;
	public LevelDownloadProgress downloadProgressText;
}

public class LevelSelection : MonoBehaviour {

	public RectTransform panel; // To hold the ScrollPanel
	public RectTransform[] pages;

	[SerializeField]
	private RectTransform center; // Center to compare the distance for each button

	[SerializeField]
	private RectTransform left;

	[SerializeField]
	private RectTransform right;

	[SerializeField]
	private LevelRequirements[] _levelRequirements;
	private Dictionary<int, LevelRequirements> _levelRequirementDictionary;

	void Start() {
		_levelRequirementDictionary = new Dictionary<int, LevelRequirements>();
		for (int i = 0; i < _levelRequirements.Length; i++) {
			_levelRequirementDictionary.Add(_levelRequirements[i].slotLevel, _levelRequirements[i]);
		}
	}

    public void Load5x3Slots() {
        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        LevelSpriteCollectionManager.Instance.ActiveLevel = 1;
        GameManager.Instance.LoadScene(GameState.SLOTS);
    }

    public void Load3x3Slots() {

    }

}
