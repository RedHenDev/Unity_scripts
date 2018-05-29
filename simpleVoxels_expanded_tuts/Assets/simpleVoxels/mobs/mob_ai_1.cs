using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob_ai_1 : MonoBehaviour {

	// How long (secs) between actions?
	public float timeDelay = 5f;
	float timeStamp = 0f;

	public Transform[] arms;

	public Transform wayPoint;

	int polR = 1;
	int polL = 1;

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
	}

	void aiAction(){

		Vector3 dir = wayPoint.position - this.transform.position;

		this.transform.rotation =
			Quaternion.Lerp (this.transform.rotation,
				Quaternion.LookRotation (dir),
			0.1f);

		this.GetComponent<Rigidbody> ().AddForce 
		(this.transform.forward * 4f);


		// Arm waving.
		if (Mathf.Abs(arms[0].rotation.x) <= 0.01f) polL = -polL;
		if (Mathf.Abs(arms[1].rotation.x) <= 0.01f) polR = -polR;

		float rotAmount0 = Mathf.Sin(-polL * Time.frameCount/10f * -Random.value) * 4.5f;
		float rotAmount1 = Mathf.Sin(-polR * Time.frameCount/10f * -Random.value) * 4.5f;

		arms[0].Rotate(Vector3.right * rotAmount0);
		arms[1].Rotate(Vector3.right * rotAmount1);
	}
}
