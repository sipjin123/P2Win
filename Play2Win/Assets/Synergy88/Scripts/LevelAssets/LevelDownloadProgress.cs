using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelDownloadProgress : MonoBehaviour {

	[SerializeField]
	private tk2dTextMesh _textPercentage;

	[SerializeField]
	private Image _circularBarImage;

	[SerializeField]
	private Transform _circularBarHeadRotationParent;

	private Vector3 tempRotationHolder;

	public void SetProgress(float progress) {
		_textPercentage.text = (progress * 100f).ToString("F1") + "%";
		_circularBarImage.fillAmount = progress;
		tempRotationHolder = _circularBarHeadRotationParent.localEulerAngles;
		tempRotationHolder.z = 0f - (360f * progress);
		_circularBarHeadRotationParent.localEulerAngles = tempRotationHolder;
	}

#if UNITY_EDITOR
	public void AutoPopulateSerializedFields() {
		_textPercentage = gameObject.GetComponent<tk2dTextMesh>();
		_circularBarImage = transform.FindChild("download_green").GetComponent<Image>();
		_circularBarHeadRotationParent = transform.FindChild("download_head_rotation");
	}
#endif
}
