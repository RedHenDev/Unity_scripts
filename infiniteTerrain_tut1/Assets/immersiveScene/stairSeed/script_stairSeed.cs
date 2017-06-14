using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The idea will be to attach this script to a 'seed' gameObject,
// and from that object, and relative to that object's attributes,
// steps will be 'grown' from the seed object.

// For now, let's try to resist coding for the following,
// so as to keep things simple and to get this working asap:
// corners; real-time growth; styles; custom step attributes;
// variations of step attributes; procedural anything; 

// OK. So we instantiate a new cube primitive, then modify
// its transform dimensions to match the seed object.
// Next, we loop through the number of steps, adjusting
// the position of each step to form a staircase.
// Nothing else to it :)



public class script_stairSeed : MonoBehaviour {

	[Tooltip("How many steps do you want?")]
	public int stepNumber = 9;
	[Tooltip("Steps going up by default.")]
	public bool goingUp = true;

	void Start () {

		generateSteps ();

	}
		

	void generateSteps(){

		// Y-direction of steps. +1 is up; -1 is down (think of Y co-ords).
		int yDir = 1;

		if (!goingUp)
			yDir = -1;

		for (int i = 0; i < stepNumber; i++) {
			// First, instantiate a clone of this 
			// seed block.
			GameObject newStep = 
				GameObject.CreatePrimitive(PrimitiveType.Cube);

			// Copy this seed block's transform attributes.
			newStep.transform.position = this.transform.position;
			newStep.transform.rotation = this.transform.rotation;
			newStep.transform.localScale = this.transform.lossyScale;

			// Next, translate the clone to its
			// correct position in the stairway.
			// Adjust forward direction...
			newStep.transform.Translate(Vector3.forward * 
				this.transform.lossyScale.z * (i+1));
			// Adjust height...
			newStep.transform.Translate(Vector3.up * 
				this.transform.lossyScale.y * yDir * (i+1));
		}
			

	}
	
}
