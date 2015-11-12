using UnityEngine;
using System.Collections;

public class TableScript : MonoBehaviour {

	public bool isOccupied;
	public GameObject myChild;
	public int TableNumber;
	void Start () {
		myChild = transform.FindChild("Chair").gameObject;
		isOccupied = true;
	
	}
}
