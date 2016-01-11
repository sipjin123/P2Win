using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
	INIT,
	SPLASH,
    LOG_IN,
    MAIN_MENU,
	GAME_MENU,
	SLOTS,
    SHOP,
    ACHIEVEMENTS,
    WALLET,
	SPINBONUS,
	BAR_FRENZY,
	MINIGAME,
	BF_MINIGAME,
	JTW_MINIGAME2
}

public class GameManager : MonoBehaviour {

	[SerializeField]
	private GameState startingState = GameState.SPLASH;

	[SerializeField]
	private string SingletonScene = "Singletons";

	[SerializeField]
	private SceneState[] sceneList;

	[System.Serializable]
	public class SceneState {
		public string scene;
		public GameState state;
	}

//	private GameState _currentState;
	private Dictionary<GameState, string> _scenes;

	private bool _gamePaused;
	public bool GamePaused { get { return _gamePaused; } }

	private static GameManager _instance;
	public static GameManager Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GameManager>();
			}
			return _instance;
		}
	}

	void Start() {
		_instance = this;
//		_currentState = GameState.INIT;

		LoadSceneStates();
		InitializeManagers();
		LoadScene(startingState);

        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	

	public void TogglePause() {
		if (GamePaused) {
			Resume();
		} else {
			Pause();
		}
	}

	public void Pause() {
		_gamePaused = true;
		AudioManager.Instance.PauseBGM();
		Time.timeScale = 0f;
	}

	public void Resume() {
		_gamePaused = false;
		AudioManager.Instance.ResumeBGM();
		Time.timeScale = 1f;
	}

	private void LoadSceneStates() {
		if (_scenes == null) {
			_scenes = new Dictionary<GameState, string>();
		} else {
			_scenes.Clear();
		}

		for(int i = 0; i < sceneList.Length; i++) {
			if (_scenes.ContainsKey(sceneList[i].state)) {
				_scenes[sceneList[i].state] = sceneList[i].scene;
			} else {
				_scenes.Add(sceneList[i].state, sceneList[i].scene);
			}
		}
	}

	private void InitializeManagers() {
		LoadSingletons();
	}

	private void LoadSingletons() {
		Application.LoadLevelAdditive(SingletonScene);
	}

	public void LoadScene(GameState state) {
//		Debug.Log("Game State: " + state);
		Application.LoadLevel(_scenes[state]);
//		_currentState = state;
	}

	public AsyncOperation LoadLevelAsync(GameState state) {
		return Application.LoadLevelAsync(_scenes[state]);
	}

	public void LoadAdditiveScene(GameState state) {
		Application.LoadLevelAdditiveAsync(_scenes[state]);
//		_currentState = state;
	}


	public AsyncOperation LoadAdditiveAsync(GameState state) {
		return Application.LoadLevelAdditiveAsync(_scenes[state]);
	}
}
