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
				AudioManager.Instance.PlayGlobalAudio(AudioManager.GlobalAudioType.SPIN_TICK);
				int Randomizer = Random.Range(1,9);
				if(Randomizer < 7)
				{
					Randomizer+=10;
				}
				hitChild.GetComponent<tk2dSprite>().SetSprite("slot_item"+Randomizer);
				hitChild.gameObject.name = hitChild.gameObject.GetComponent<tk2dSprite>().CurrentSprite.name;
				hit.gameObject.name = hitChild.gameObject.name;
				hitChild.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}
}
