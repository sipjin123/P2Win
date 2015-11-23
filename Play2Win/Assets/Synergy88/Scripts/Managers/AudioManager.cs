using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	public enum GlobalAudioType {
		BUTTON_GENERIC,
		BUTTON_ADD,
		BUTTON_DECREASE,
		SLOT_SEGMENT_STOP,
		WIN_1,
		WIN_2,
		WIN_3,
		BONUS,
		COINS,
		WHEEL_INTRO,
		WHEEL_BGM,
		WHEEL_STOP,
		WHEEL_WIN,
		LEVELUP,
        BGM_LOBBY,
		EXIT,
		PURCHASE,
		SPINNING_REELS,
		STOP_REELS,
		SELECT,
		SPIN_TICK,
		DAILY_BONUS,
		MONSTER_HIT,
		PRINCESS_HIT,
		WHACK_BGM,
		REGULAR_WIN,
		BONUS_WIN,
		BIG_WIN,
		JTW_INTRO,
		WHACK_WIN
	}

	[System.Serializable]
	public struct GlobalAudioData {
		public GlobalAudioType type;
		public AudioSource clip;
	}

	private static AudioManager _instance;
	public static AudioManager Instance { 
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<AudioManager>();
			}
			return _instance;
		}
	}

	[SerializeField]
	private GlobalAudioData[] globalAudios;

	private Dictionary<GlobalAudioType, AudioSource> _globalAudioList;

	private GlobalAudioType _currentBGM = GlobalAudioType.BGM_LOBBY;

    private bool _bgmPlaying = false;

	void Start() {
		_instance = this;

		_globalAudioList = new Dictionary<GlobalAudioType, AudioSource>();

		for (int i = 0; i < globalAudios.Length; i++) {
			_globalAudioList.Add(globalAudios[i].type, globalAudios[i].clip);
		}
	}

	public void PlayGlobalAudio(GlobalAudioType type) {
//		Debug.Log(type.ToString());

		if (IsPlayable(type)) {
			_globalAudioList[type].Play();
		}
	}

	public void SwitchBGM(GlobalAudioType newType) {
        if (_currentBGM == newType && _bgmPlaying) {
            return;
        }

		_globalAudioList[_currentBGM].Stop();
		_currentBGM = newType;
		_globalAudioList[_currentBGM].Play();

        if (!_bgmPlaying) {
            _globalAudioList[_currentBGM].Pause();
        }
	}

	public void StopGlobalAudio(GlobalAudioType type) {
		_globalAudioList[type].Stop();
	}

	public void PauseBGM() {
		_globalAudioList[_currentBGM].Pause();

        _bgmPlaying = false;
	}

	public void ResumeBGM() {
		if (PlayerDataManager.Instance.UseBGM) {
			_globalAudioList[_currentBGM].Play();
            _bgmPlaying = true;
		}
	}

	private bool IsPlayable(GlobalAudioType type)  {
		switch (type) {
		case GlobalAudioType.BGM_LOBBY:
        case GlobalAudioType.WHACK_BGM:
        case GlobalAudioType.WHEEL_BGM:
			return PlayerDataManager.Instance.UseBGM;

		default:
			return PlayerDataManager.Instance.UseSFX;
		}
	}

}

