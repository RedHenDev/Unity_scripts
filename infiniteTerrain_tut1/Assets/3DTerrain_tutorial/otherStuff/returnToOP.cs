using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnToOP : MonoBehaviour {

	private Vector3 OP;

	public float threshholdY = -2f;

	void Start () {
		// Recrod orignal position.
		OP = this.transform.position;
	}
	

	void Update () {
		// This object fallen below threshold?
		if (this.transform.position.y < threshholdY)
			this.transform.position = OP;
	}
}
