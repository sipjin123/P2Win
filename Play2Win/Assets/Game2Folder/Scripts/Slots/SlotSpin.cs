using UnityEngine;
using System.Collections;

public class SlotSpin : MonoBehaviour {

	public enum GameState{
		IDLE,
		STARTSPIN,
		STOPSPIN
	}
	public GameState _gameState;

	public float SpinSpeed;
	public float SpinStrength;


	[SerializeField] GameObject SlotChild;
	[SerializeField] GameObject[] SlotItems;

	void Start () {
		SpinSpeed = GameManager_ReelChef.Instance.SpinSpeed;
		_gameState = GameState.IDLE;

		int i = 0;
		foreach (Transform child in SlotChild.transform)
		{
			SlotItems[i] = child.gameObject;
			i++;
		}
	}


	void Update () {


		if(_gameState == GameState.STARTSPIN)
		{
			StartSpinning();
		}
		else if(_gameState == GameState.STOPSPIN)
		{
			StopSpinning();
		}
	}

	public void SpinButton()
	{
		_gameState = GameState.STARTSPIN;
	}

	
	void StartSpinning()
	{
		transform.position -= transform.forward * SpinSpeed;
		SpinStrength --;
		if(SpinStrength <= 0)
		{
			ResetSlotIconEffects();
			_gameState = GameState.STOPSPIN;
			AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_STOP);
			SlotManager.Instance.SpinChecker();
		}
	}
	void ResetSlotIconEffects()
	{
		foreach(Transform child in SlotChild.transform)
		{
			int SpriteID = child.GetComponent<Itemscript>().SlotIcon.GetComponent<tk2dSprite>().spriteId;
			if(SpriteID > 7)
			{
				child.GetComponent<Itemscript>().SlotIcon.GetComponent<tk2dSprite>().SetSprite("slot_item"+ (SpriteID - 6));
				child.GetComponent<Itemscript>().SlotIcon.gameObject.name = "slot_item" +  (SpriteID - 6);
				child.gameObject.name = child.GetComponent<Itemscript>().SlotIcon.gameObject.name;
			}
		}
	}
	void StopSpinning()
	{
		if(transform.position.z % 2 == 0)
		{

		}
		else
		{
			transform.position -= transform.forward * SpinSpeed;
			StopSpinning();
		}
	}
}
