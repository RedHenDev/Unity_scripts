using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob_ai_1 : MonoBehaviour {

	// How long (secs) between actions?
	public float timeDelay = 5f;
	float timeStamp = 0f;

	public Transform wayPoint;

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

		//if (Time.frameCount % 2 == 0)
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
	}
}
