using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mLook : MonoBehaviour {


	// Mouse direction.
	private Vector2 mDir;

	// Parent body (e.g. a capsule).
	// This script should be attached to the
	// Main Camera, and the Main Camera should
	// be a child object of a capsule.
	private Transform myBody;


	void Start () {
		// Grab the capsule body's transform.
		// This is so that we can rotate the
		// main body of the character.
		myBody = this.transform.parent.transform;
	}
	

	void Update () {


		// How much has mouse moved across screen?
		Vector2 mc = new Vector2(Input.GetAxisRaw("Mouse X"),
			Input.GetAxisRaw("Mouse Y"));

		// Add new movement to current mouse direction.
		mDir += mc;

		// Rotate head up or down.
		// This rotates the camera on X-axis.
		this.transform.localRotation =
			Quaternion.AngleAxis (-mDir.y, Vector3.right);

		// Rotate body left or right.
		// This rotates the parent body (a capsule), not the camera,
		// on the Y-axis.
		myBody.localRotation =
			Quaternion.AngleAxis (mDir.x, Vector3.up);

	}
}
