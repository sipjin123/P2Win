using UnityEngine;
using System.Collections;

public class SlotCollider : MonoBehaviour {

	[SerializeField] bool RevealorReset;
	void OnTriggerEnter(Collider hit)
	{

		if(hit.GetComponent<Itemscript>() != null)
		{
			GameObject hitChild = hit.GetComponent<Itemscript>().SlotIcon;
			if(!RevealorReset)
			{
				hit.transform.localPosition = new Vector3 ( hit.transform.localPosition.x,
				                                           hit.transform.localPosition.y + 24,
				                                           hit.transform.localPosition.z );
			
				hitChild.GetComponent<MeshRenderer>().enabled = false;
			}
			else if(RevealorReset)
			{
				if(Random.Range(1,3) == 1)
				{
					AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_SPIN1);
				}
				else
					AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.BARFRENZY_SPIN2);
				int Randomizer = Random.Range(1,10);
				if(Randomizer < 7)
				{
					Randomizer+=10;
				}
				if(Randomizer == 7 || Randomizer == 9)
				{
					Randomizer = Random.Range(1,10);
				}
				hitChild.GetComponent<tk2dSprite>().SetSprite("slot_item" + Randomizer);
				hitChild.gameObject.name = hitChild.gameObject.GetComponent<tk2dSprite>().CurrentSprite.name;
				hit.gameObject.name = hitChild.gameObject.name;
				hitChild.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}
}
