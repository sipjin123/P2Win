using UnityEngine;
using System.Collections;

public class JourneyScene : MonoBehaviour {

    [SerializeField]
    private tk2dTextMesh _levelText;

    [SerializeField]
    private GameObject _infoWindow;

    [SerializeField]
    private GameObject _destination1Info;

    [SerializeField]
    private GameObject _destination2Info;

    void Start() {
        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", ProfileUIType.GEM_SCENES);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);

        _levelText.text = PlayerDataManager.Instance.Level.ToString();

        CloseInfoWindow();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            ExitScene();
        }
    }

    public void ExitScene() {
        GameManager.Instance.LoadScene(GameState.MAIN_MENU);
    }

    public void ShowDestination1() {
        _infoWindow.SetActive(true);
        _destination1Info.SetActive(true);
        _destination2Info.SetActive(false);
    }

    public void ShowDestination2() {
        _infoWindow.SetActive(true);
        _destination1Info.SetActive(false);
        _destination2Info.SetActive(true);
    }

    public void CloseInfoWindow() {
        _infoWindow.SetActive(false);
    }

    public void OpenSlots() {
        GameManager.Instance.LoadScene(GameState.SLOTS);
    }

    public void OpenBarFrenzy() {
        GameManager.Instance.LoadScene(GameState.BAR_FRENZY);
    }
}
