using UnityEngine;
using System.Collections;
using System.IO;

public class TestScene : MonoBehaviour {

	//		/DefiniteSlots/LevelAssets

	[SerializeField]
	private tk2dSpriteCollectionData data;

	[SerializeField]
	private Texture2D texture;

	[SerializeField]
	private tk2dSprite sprite;

	private WWW www;

	// Use this for initialization
	void Start () {
		StartCoroutine(DownloadFile());
		sprite.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			SetTexture();
		}
	}

	public void SetTexture() {
		string url = GetLocalAssetPath() + "SlotLevel4.png";

		byte[] fileData = File.ReadAllBytes(url);

		Debug.Log (url);

		texture = new Texture2D(2, 2);
		texture.LoadImage(fileData);

		data.materials[0].SetTexture(0, texture);
		sprite.SetSprite(data, 0);
	}

	private IEnumerator DownloadFile() {

		string filename = "SlotLevel4.png";

		string url = "http://202.147.26.32/DefiniteSlots/LevelAssets/" + filename;
		www = new WWW(url);
		
		while (!www.isDone) {
			yield return null;
		}
		
		Debug.Log ("Finished downloading file: " + url);
		
		byte[] data = www.bytes;
		string docPath = 

		GetLocalAssetPath() + filename;

		
		if (File.Exists(docPath)) {
			File.Delete(docPath);
		}
		
		Debug.Log ("Writing to: " + docPath);
		File.WriteAllBytes(docPath, data);
		
		// Buffer
		yield return new WaitForSeconds(0.5f);
		
		Debug.Log ("Finish saving to streaming folder.");
		sprite.gameObject.SetActive(true);
	}

	private string GetLocalAssetPath() {
		#if UNITY_STANDALONE || UNITY_EDITOR
		return Application.streamingAssetsPath + "/AssetBundles/";
		#else
		return Application.persistentDataPath + "/";
		#endif
	}

}
