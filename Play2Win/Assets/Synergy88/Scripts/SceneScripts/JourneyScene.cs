﻿using UnityEngine;
using System.Collections;

public class JourneyScene : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            ExitScene();
        }
    }

    public void ExitScene() {
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
    }
}
