using UnityEngine;
using System.Collections;

public class RouletteHandScript : MonoBehaviour {

	public string rouletteTextPrice;
	public int roulettePrice;

	void OnTriggerEnter(Collider col){
		rouletteTextPrice = col.gameObject.name; 
		if(rouletteTextPrice != "2X" && rouletteTextPrice != "3X")
			roulettePrice = int.Parse (rouletteTextPrice);
	}
}
