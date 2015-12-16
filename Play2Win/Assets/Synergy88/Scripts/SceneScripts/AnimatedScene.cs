using UnityEngine;
using System.Collections;

public class AnimatedScene : MonoBehaviour {

    private const string SHOW_ANIMATION = "Show";
    private const string HIDE_ANIMATION = "Hide";

    [SerializeField]
    private ProfileUIType HUDType;

    [SerializeField]
    private AudioManager.GlobalAudioType SceneBGM;

    [SerializeField]
    private GameState PreviousScene;

    [SerializeField]
    private Animator SceneAnimator;

	protected void Initialize() {
        ConcreteSignalParameters updateHudParam = new ConcreteSignalParameters();
        updateHudParam.AddParameter("ProfileUIType", HUDType);
        SignalManager.Instance.CallWithParam(SignalType.UPDATE_PROFILE_HUD, updateHudParam);

        AudioManager.Instance.SwitchBGM(SceneBGM);

        SceneAnimator.SetTrigger(SHOW_ANIMATION);
	}

    protected void Exit() {
        if (PreviousScene == GameState.INIT) {
            Application.Quit();
        } else {
            LoadScene(PreviousScene);
        }
    }

    protected void LoadScene(GameState scene) {
        StartCoroutine(AnimatedLoading(scene));
    }

    private IEnumerator AnimatedLoading(GameState scene) {
        yield return new WaitForSeconds(0.3f);
        SceneAnimator.SetTrigger(HIDE_ANIMATION);

        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.LoadScene(scene);
    }
}
