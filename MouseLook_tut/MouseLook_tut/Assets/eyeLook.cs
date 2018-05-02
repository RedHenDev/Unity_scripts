using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeLook : MonoBehaviour {

	// Mouse direction.
	private Vector2 mD;

	// The capsule parent!
	private Transform myBody;

	void Start () {

		myBody = this.transform.parent.transform;

	}
	

	void Update () {

		// How much has the mouse moved?
		Vector2 mC = new Vector2
			(Input.GetAxisRaw ("Mouse X"),
				Input.GetAxisRaw("Mouse Y"));

		mD += mC;

		this.transform.localRotation =
			Quaternion.AngleAxis (-mD.y, Vector3.right);

		myBody.localRotation =
			Quaternion.AngleAxis (mD.x, Vector3.up);

	}
}
