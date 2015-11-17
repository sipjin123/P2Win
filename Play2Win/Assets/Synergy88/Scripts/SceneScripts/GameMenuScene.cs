using UnityEngine;
using System.Collections;
using System.IO;

public class GameMenuScene : AnimatedScene {

    [SerializeField]
    private tk2dSpriteAnimator[] myAnimator;

    [SerializeField]
    private tk2dUIItem[] myUIItem;

    [SerializeField]
    private Animator contentAnim;

    void Start() {
        Initialize();

        ConcreteSignalParameters stateParam = new ConcreteSignalParameters();
        stateParam.AddParameter(SlotMachineScene.PARAM_LOCK, false);
        SignalManager.Instance.CallWithParam(SignalType.SLOT_MACHINE_STATE_CHANGED, stateParam);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Exit();
        }
    }

    public void LoadGame1() {
        Debug.LogWarning("Loading Game 1");
        myUIItem[0].enabled = false;
        myAnimator[0].Play("BtnShine");

        AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BUTTON_GENERIC);
        LevelSpriteCollectionManager.Instance.ActiveLevel = 1;

        LoadScene(GameState.SLOTS);
    }

    public void LoadGame2() {

    }

}
