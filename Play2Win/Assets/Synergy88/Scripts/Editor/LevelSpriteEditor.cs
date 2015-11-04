using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(LevelSprite))]
public class LevelSpriteEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
        //if (GUILayout.Button("Get Current Name")) {
        //    foreach(Object currentTarget in targets) {
        //        LevelSprite spriteScript = (LevelSprite)currentTarget;
        //        spriteScript.SetFromCurrentSpriteName();
        //    }
        //}

        if (GUILayout.Button("Randomize (Slot item only)")) {

             Dictionary<SlotItemType, float>_itemChances = new Dictionary<SlotItemType, float>();
            _itemChances.Add(SlotItemType.ITEM_1, 0f);
            _itemChances.Add(SlotItemType.ITEM_2, 10f);
            _itemChances.Add(SlotItemType.ITEM_3, 20f);
            _itemChances.Add(SlotItemType.ITEM_4, 30f);
            _itemChances.Add(SlotItemType.ITEM_5, 40f);
            _itemChances.Add(SlotItemType.ITEM_6, 45f);
            _itemChances.Add(SlotItemType.ITEM_7, 50f);
            _itemChances.Add(SlotItemType.ITEM_8, 55f);
            _itemChances.Add(SlotItemType.BONUS, 60f);
            _itemChances.Add(SlotItemType.SCATTER, 62.5f);
            _itemChances.Add(SlotItemType.WILD, 65f);

            foreach (Object currentTarget in targets) {
                LevelSprite spriteScript = (LevelSprite)currentTarget;
                spriteScript.RandomizeItemOnly(_itemChances);
            }
        }

        if (GUILayout.Button("Set sprite from name")) {
            foreach (Object currentTarget in targets) {
                LevelSprite spriteScript = (LevelSprite)currentTarget;
                spriteScript.SetSpriteFromCurrentName();
            }
        }
	}

}
