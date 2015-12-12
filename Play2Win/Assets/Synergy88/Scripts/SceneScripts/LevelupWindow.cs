using UnityEngine;
using System.Collections;

public class LevelupWindow : MonoBehaviour, ISignalListener {

	[SerializeField]
	private tk2dTextMesh _previousLevelText;

	[SerializeField]
	private tk2dTextMesh _rewardText;

	[SerializeField]
	private tk2dTextMesh _maxBetText;


	void Start() {
		SignalManager.Instance.Register(this, SignalType.LEVELED_UP);
		Close();
	}
	
	void OnDestroy() {
		SignalManager.Instance.Remove(this, SignalType.LEVELED_UP);
	}

	public void Show() {
		int giftReward = 150 + (PlayerDataManager.Instance.Level * 50);

		AudioManager.Instance.PauseBGM();
		AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.LEVELUP);


		PlayerDataManager.Instance.AddChips(giftReward);

		_previousLevelText.text = PlayerDataManager.Instance.Level.ToString();
		_rewardText.text = giftReward.ToString("#,#");
		_maxBetText.text = GameDataManager.Instance.LevelInfo.MaxCoinBet.ToString("#,#");

		gameObject.SetActive(true);
	}

	public void Close() {
		gameObject.SetActive(false);
		SignalManager.Instance.Call(SignalType.LEVELUPWINDOW_CLOSED);
	}

	public void Execute(SignalType type, ISignalParameters param) {
		switch (type) {
		case SignalType.LEVELED_UP:
			Invoke("Show", 0.5f);
			break;
		}
	}
}

