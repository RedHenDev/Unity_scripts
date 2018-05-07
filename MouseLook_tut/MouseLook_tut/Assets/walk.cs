using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour {
		

	void Update () {

		float movement = Input.GetAxis ("Vertical");
		movement *= Time.deltaTime;

		float sideStep = Input.GetAxis ("Horizontal");
		sideStep *= Time.deltaTime;

//		this.transform.Translate
//		(Vector3.forward * movement);

		if (this.GetComponent<Rigidbody> ().
			velocity.magnitude < 5f) {
			this.GetComponent<Rigidbody> ().AddForce
		(this.transform.forward * movement * 1000f); 
		}

		this.transform.Translate
		(Vector3.right * sideStep);
	

	}
}
