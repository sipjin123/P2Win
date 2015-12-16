using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CupData : MonoBehaviour {

	private static CupData _instance;
	public static CupData Instance { get { return _instance; } }

	[SerializeField]
	private int cupID;
	[SerializeField]
	private int mainCup;
	[SerializeField]
	private GameObject posRef;

	void Start(){
		_instance = this;
	}

	public int getCupID(){
		return cupID;
	}
	public int getMainCup(){
		return mainCup;
	}
	public GameObject getPosRef(){
		return posRef;
	}

	public GameObject playCup(){
		return this.gameObject;
	}

	public void setPosRef(GameObject refPos){
		posRef = refPos;
	}
	
}
