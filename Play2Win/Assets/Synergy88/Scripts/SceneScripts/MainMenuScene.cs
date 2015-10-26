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
