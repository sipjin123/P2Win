using UnityEngine;
using System.Collections;

public class SlotCollider : MonoBehaviour {

	[SerializeField] bool RevealorReset;
	void OnTriggerEnter(Collider hit)
	{
		if(!RevealorReset)
		{
			hit.transform.localPosition = new Vector3 ( hit.transform.localPosition.x,
			                                           hit.transform.localPosition.y + 24,
			                                           hit.transform.localPosition.z );
			hit.GetComponent<tk2dSprite>().SetSprite("slot_item"+Random.Range(1,9));
			hit.gameObject.name = hit.gameObject.GetComponent<tk2dSprite>().CurrentSprite.name;
			hit.GetComponent<MeshRenderer>().enabled = false;
		}
		else if(RevealorReset)
		{
			hit.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
