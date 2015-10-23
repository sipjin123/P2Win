using UnityEngine;
using UnityEditor;

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
		
        //if (GUILayout.Button("Randomize (Slot item only)")) {
        //    foreach(Object currentTarget in targets) {
        //        LevelSprite spriteScript = (LevelSprite)currentTarget;
        //        spriteScript.RandomizeItemOnly();
        //    }
        //}

        if (GUILayout.Button("Set sprite from name")) {
            foreach (Object currentTarget in targets) {
                LevelSprite spriteScript = (LevelSprite)currentTarget;
                spriteScript.SetSpriteFromCurrentName();
            }
        }
	}

}
