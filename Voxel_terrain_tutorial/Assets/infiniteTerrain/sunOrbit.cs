using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunOrbit : MonoBehaviour {


	public GameObject player;

	public float theta = 0.6f;

	void Update () {

		this.transform.LookAt(Vector3.zero);


		this.transform.RotateAround (
			Vector3.zero,
			Vector3.right,
			theta);

	}
}
