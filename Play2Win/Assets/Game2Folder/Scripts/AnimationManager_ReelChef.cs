using UnityEngine;
using System.Collections;

public class AnimationManager_ReelChef : MonoBehaviour {

	
	private static AnimationManager_ReelChef _instance;
	public static AnimationManager_ReelChef Instance { get { return _instance; } }

	public tk2dSpriteAnimator []SparkleExplosion;
	public tk2dSpriteAnimator Chef;
	void Awake()
	{
		_instance = this;
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		}
}
