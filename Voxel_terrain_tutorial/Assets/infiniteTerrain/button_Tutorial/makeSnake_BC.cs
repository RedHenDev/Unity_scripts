using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeSnake_BC : MonoBehaviour {

	public GameObject activationObject;

	public bool ready = false;

	void Start(){

		// Button is white at start.
		this.GetComponent<MeshRenderer> ().material
			.color = new Color (1f, 1f, 1f);
		

	}


	void OnTriggerEnter(Collider whatHitMe){

		// Button turns green.
		this.GetComponent<MeshRenderer> ().material
			.color = new Color (0f, 1f, 0f);

		// Button is ready.
		ready = true;

	}

	void OnTriggerExit(Collider whatHitMe){

		this.GetComponent<MeshRenderer> ().material
			.color = new Color (1f, 1f, 1f);

		// Button no longer active.
		ready = false;
	}


	void activate(){
		activationObject.
		GetComponent<spawn> ()._poolSpawn ();
	}

	void Update(){

		if (Input.GetMouseButtonUp (0)) {
			if (ready)
				activate ();
		}

	}


}
