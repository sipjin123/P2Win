using UnityEngine;
using System.Collections;

public class MainMenuScene : MonoBehaviour {

    [SerializeField]
    private GameState _games;

    [SerializeField]
    private GameState _rewards;

    [SerializeField]
    private GameState _journey;

    [SerializeField]
    private GameState _wallet;

    void Start() {
        if (!GameDataManager.Instance.LobbyWindowsLoaded) {
            Application.LoadLevelAdditive("PlayerProfile");
            Application.LoadLevelAdditive("Settings");
            Application.LoadLevelAdditive("LevelupWindow");
            Application.LoadLevelAdditive("MessagePrompt");

            GameDataManager.Instance.SetLobbyWindowsLoaded();
        }

        PlayerDataManager.Instance.InitializeListener();
    }

    public void LoadGameMenu() {
        GameManager.Instance.LoadScene(_games);
    }

    public void LoadRewardsScene() {
        GameManager.Instance.LoadScene(_rewards);
    }

    public void LoadJourneyScene() {
        GameManager.Instance.LoadScene(_journey);
    }

    public void LoadWalletScene() {
        GameManager.Instance.LoadScene(_wallet);
    }
}
