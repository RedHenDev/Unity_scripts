using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotion : MonoBehaviour {


	public Transform myCam;

	float move = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if (myCam.rotation.eulerAngles.z < 350f &&
		    myCam.rotation.eulerAngles.z > 340f) {

			move++;
		
		}

		move *= Time.deltaTime;

		this.transform.Translate (myCam.forward * move);

		move *= 0.996f;

	}
}
