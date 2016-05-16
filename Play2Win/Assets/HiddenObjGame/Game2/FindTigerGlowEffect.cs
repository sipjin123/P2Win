using UnityEngine;
using System.Collections;

public class FindTigerGlowEffect : MonoBehaviour {

	[SerializeField] private GameObject _glowEffect;

	tk2dSpriteAnimator _animator;

	// Use this for initialization
	void Start () {
		_animator = _glowEffect.GetComponent<tk2dSpriteAnimator>();
	}
	
	void OnMouseDown()
	{
		if (!_animator.Playing) {
			_glowEffect.SetActive (true);
			StartCoroutine(playEffectAnimation());
		}
	}		

	IEnumerator playEffectAnimation()
	{
		_animator.Play ("ClickGlow");
		yield return new WaitForSeconds(0.5f);
		_glowEffect.SetActive (false);
	}
}
