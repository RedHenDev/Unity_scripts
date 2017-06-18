using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {


	public bool isOrbiting = false;
	public float orbitSpeed = 1f;
	public bool centreLookAt = true;	// Do we want the orbiting object to LookAt zero?
	private Transform myTrans;

	public Transform mother;

	// Use this for initialization
	void Start () {
	
		//transform.LookAt(Vector3.zero); // To make sure our alien Sun is looking in the correct direction :)
		myTrans = this.transform;

		if (mother==null) {mother = this.transform; mother.position = Vector3.zero;}
	}
	
	// Update is called once per frame. Use 'FixedUpdate' to avoid jutter. Apparently.	
	void Update () {
	

		if(isOrbiting){
		//transform.LookAt(Vector3.zero); // Keeps looking the in the correct direction :)

			myTrans.RotateAround(mother.position, Vector3.left, orbitSpeed*Time.deltaTime); // Orbiting code.
			if (!centreLookAt) myTrans.RotateAround(mother.position, myTrans.forward, orbitSpeed*Time.deltaTime); // Orbiting code.
		}

	}
}
