using UnityEngine;
using System.Collections;

public class SpotLightController : MonoBehaviour {
	
	private bool goingLeft;
	private float speed = 0;
	private bool isTurning = false;


	void Start(){
		goingLeft = Random.Range (0, 100f) > 50 ? true : false ;
		speed = Random.Range (40.0f, 50.0f);
	}

	void Update(){
		if (!goingLeft) {
			if(this.transform.localRotation.z < 0.3f){
				this.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
			}
			else{
				changeDirection();
			}
		}
		else{
			if(this.transform.localRotation.z > -0.3f){
				this.transform.Rotate(Vector3.back * speed * Time.deltaTime);
			}
			else{
				changeDirection();
			}
		}
	}

	void changeDirection(){
		goingLeft = !goingLeft ? true : false;
		speed = Random.Range (40.0f, 50.0f);
	}
}
