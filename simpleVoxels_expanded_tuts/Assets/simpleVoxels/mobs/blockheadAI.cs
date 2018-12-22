using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockheadAI : MonoBehaviour {


	public float hopForce = 10f;

	public Transform targetPosition;

	float lastHopTime = 0;
	
	// Update is called once per frame
	void Update () {
		
		rotateHead ();
		chaseTarget ();

		if (Time.realtimeSinceStartup -
		    lastHopTime >= 2f) {
			lastHopTime = Time.realtimeSinceStartup;
			hop ();
		}

	}


	void hop (){

		this.GetComponent<Rigidbody> ().AddForce (
			Vector3.up * hopForce);

	}


	void chaseTarget(){

		this.transform.Translate (Vector3.forward *
		0.4f * Time.deltaTime);

	}

	void rotateHead(){


		Vector3 targetDirection = targetPosition.position -
		                          this.transform.position;

		this.transform.rotation =
			Quaternion.Slerp (this.transform.rotation,
			Quaternion.LookRotation (targetDirection),
			0.1f);

	}

}
