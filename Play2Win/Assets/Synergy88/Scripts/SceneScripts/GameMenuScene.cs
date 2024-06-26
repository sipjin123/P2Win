﻿using UnityEngine;
using System.Collections;
using System.IO;

public class GameMenuScene : AnimatedScene, ISignalListener {

    [SerializeField]
    private tk2dSpriteAnimator[] myAnimator;

    [SerializeField]
    private tk2dUIItem[] myUIItem;

    [SerializeField]
    private Animator contentAnim;

    [SerializeField]
    private GameObject _level2LockedSprite;
    [SerializeField]
    private GameObject _level2AvailableSprite;

	[SerializeField]
	private GameObject _level3LockedSprite;
	[SerializeField]
	private GameObject _level3AvailableSprite;

	[SerializeField]
	private GameObject _JTWLogo;
	[SerializeField]
	private GameObject _BFLogo;
	[SerializeField]
	private GameObject _TSLogo;
	[SerializeField]
	private GameObject _transitionScene;

    void Start() {
        Initialize();

        ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
        stateParam.AddParameter(SlotMachineScene.PARAM_LOCK, false);
        SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);

        SignalManager.Instance.Register(this, SignalType.LEVELED_UP);

		SignalManager.Instance.Call (SignalType.REVERT_SETTINGSBTN_SPRITE);

        UpdateGame2Availability();
		UpdateGame3Availability();
    }

    void OnDestroy() {
        SignalManager.Instance.Remove(this, SignalType.LEVELED_UP);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Exit();
        }
    }

    private void UpdateGame2Availability() {
        _level2AvailableSprite.SetActive(PlayerDataManager.Instance.Level >= 2);
        _level2LockedSprite.SetActive(PlayerDataManager.Instance.Level < 2);
    }

	private void UpdateGame3Availability() {
		_level3AvailableSprite.SetActive(PlayerDataManager.Instance.Level >= 3);
		_level3LockedSprite.SetActive(PlayerDataManager.Instance.Level < 3);
	}

	public void LoadGame1() {
		AudioManager.Instance.PauseBGM();
        myUIItem[0].enabled = false;
        myAnimator[0].Play("BtnShine");

        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        LevelSpriteCollectionManager.Instance.ActiveLevel = 1;

		_transitionScene.GetComponent<SceneTransitionManager> ().SetTransitionLogo (_JTWLogo);
		_transitionScene.SetActive (true);

        LoadScene(GameState.SLOTS);
    }

    public void LoadGame2() {
		AudioManager.Instance.PauseBGM();
        if (PlayerDataManager.Instance.Level < 2) {
            return;
        }
		myUIItem[1].enabled = false;
		myAnimator[1].Play("BtnShine");

		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		LevelSpriteCollectionManager.Instance.ActiveLevel = 2;

		_transitionScene.GetComponent<SceneTransitionManager> ().SetTransitionLogo (_BFLogo);
		_transitionScene.SetActive (true);

		LoadScene (GameState.BAR_FRENZY);
        // Load Game 2 here
    }

	public void LoadGame3() {
		AudioManager.Instance.PauseBGM();
		if (PlayerDataManager.Instance.Level < 3) {
			return;
		}

		myUIItem[2].enabled = false;
		myAnimator[2].Play("BtnShine");
		
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
		LevelSpriteCollectionManager.Instance.ActiveLevel = 3;
		
		_transitionScene.GetComponent<SceneTransitionManager> ().SetTransitionLogo (_TSLogo);
		_transitionScene.SetActive (true);

		LoadScene (GameState.TIGER_SLOTS);
	}

    public void Execute(SignalType type, ISignalParameters param) {
        UpdateGame2Availability();
    }
}
