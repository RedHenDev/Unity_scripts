using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spheroAI : MonoBehaviour {


	public Transform targetPos;

	

	void Update () {

		lookatMe ();

	}


	void lookatMe(){

		Vector3 dir = targetPos.position - 
			this.transform.position;

		this.transform.rotation = 
			Quaternion.Lerp (this.transform.rotation,
			Quaternion.LookRotation (dir), 0.018f);

		//this.transform.LookAt (targetPos.position);
	}


}
