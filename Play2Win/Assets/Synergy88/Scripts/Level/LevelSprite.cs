using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dSprite))]
public class LevelSprite : MonoBehaviour {

	[SerializeField]
	private string spriteName;

	void Start() {
        //UpdateLevelSprite();
	}

//    public void UpdateLevelSprite() {
//        tk2dSprite spriteComponent = GetComponent<tk2dSprite>();
//        spriteComponent.SetSprite(LevelSpriteCollectionManager.Instance.GetSpriteCollectionData(), spriteName);

////		Debug.Log("Setting sprite to " + LevelSpriteCollectionManager.Instance.GetSpriteCollectionData().name + " - " + spriteName);
//        SlotItem item = GetComponent<SlotItem>();
//        if (item != null) {
//            item.Randomize();
//            spriteName = SlotItem.SlotItemName(item.CurrentItemType);
//        }
//    }

//    public void SetFromCurrentSpriteName() {
//        tk2dSprite spriteComponent = GetComponent<tk2dSprite>();
//        spriteName = spriteComponent.CurrentSprite.name;
//    }

    public void SetSpriteFromCurrentName() {
        tk2dSprite spriteComponent = GetComponent<tk2dSprite>();
        spriteComponent.SetSprite(spriteName);
    }

//    public void RandomizeItemOnly() {
//        SlotItem item = GetComponent<SlotItem>();
//        if (item == null) {
//            Debug.LogWarning("Object " + name + " does not have a SlotItem component attached.");
//            return;
//        }

//        item.Randomize();
//        spriteName = SlotItem.SlotItemName(item.CurrentItemType);
//    }
}
