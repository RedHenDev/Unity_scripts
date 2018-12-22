using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob_ai_1 : MonoBehaviour {

	// How long (secs) between actions?
	public float timeDelay = 5f;
	float timeStamp = 0f;

	public Transform[] arms;

	public Transform wayPoint;

	float polR = -1;
	float polL = 1;

	void Start () {
		timeStamp = Time.realtimeSinceStartup;
	}
	

	void Update () {

		// Time to generate new mob action?
		if (Time.realtimeSinceStartup - timeStamp >= timeDelay) {
			// New time-stamp.
			timeStamp = Time.realtimeSinceStartup;
			aiHop ();
		}
			
			aiAction ();
	}

	void aiHop(){
		this.GetComponent<Rigidbody> ().AddForce
		(Vector3.up * 240f);
		//Debug.Log (arms[0].rotation.eulerAngles.x);
	}

	void aiAction(){

		Vector3 dir = wayPoint.position - this.transform.position;

		this.transform.rotation =
			Quaternion.Slerp (this.transform.rotation,
				Quaternion.LookRotation (dir),
			0.1f);

		this.GetComponent<Rigidbody> ().AddForce 
		(this.transform.forward * 4f);


		// Arm waving.
		if (polL == 1) {
			//Rotating forwards.
			if (arms [0].rotation.eulerAngles.x > 30f && 
				arms [0].rotation.eulerAngles.x < 90f)
				polL = -polL;
		}
		if (polL == -1) {
			//Rotating backwards.
			if (arms [0].rotation.eulerAngles.x < 350f &&
				arms [0].rotation.eulerAngles.x > 330f)
				polL = -polL;
		}

		if (polR == 1) {
			//Rotating forwards.
			if (arms [1].rotation.eulerAngles.x > 30f && 
				arms [1].rotation.eulerAngles.x < 90f)
				polR = -polR;
		}
		if (polR == -1) {
			//Rotating backwards.
			if (arms [1].rotation.eulerAngles.x < 350f &&
				arms [1].rotation.eulerAngles.x > 330f)
				polR = -polR;
		}

		float rotAmount0 = polL * 110f * Time.deltaTime;
		float rotAmount1 = polR * 110f * Time.deltaTime;

		arms[0].Rotate(Vector3.right * rotAmount0);
		arms[1].Rotate(Vector3.right * rotAmount1);
	}
}
