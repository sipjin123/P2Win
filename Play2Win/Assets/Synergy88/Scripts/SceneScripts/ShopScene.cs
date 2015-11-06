using UnityEngine;
using System.Collections;

public class ShopScene : MonoBehaviour {

    void Start() {
        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", ProfileUIType.GEM_SCENES);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            ExitScene();
        }
    }

    public void ExitScene() {
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
    }

}
