using UnityEngine;
using System.Collections;

public class LogInScene : MonoBehaviour {

    [SerializeField]
    private GameState _mainMenuScene = GameState.MAIN_MENU;

    public void LoadMainMenu() {
        GameManager.Instance.LoadScene(_mainMenuScene);
    }
}
