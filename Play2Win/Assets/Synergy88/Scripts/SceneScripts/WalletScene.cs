using UnityEngine;
using System.Collections;

public class WalletScene : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            ExitScene();
        }
    }

    public void ExitScene() {
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
    }
}
