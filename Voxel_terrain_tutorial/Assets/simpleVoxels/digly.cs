using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class digly : MonoBehaviour {

	public Camera myEyes;
	

	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			AddCube ();
		}

		if (Input.GetMouseButtonUp (1)) {
			RemoveCube ();
		}

	}


	void AddCube(){

		Ray ray = myEyes.ScreenPointToRay 
			(new Vector3(Screen.width/2, Screen.height/2, 0f)); 
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 5f)) {

			// Add cube.
			Vector3 newPos = hit.transform.position;

			GameObject newB = Instantiate (hit.collider.gameObject);

			newB.transform.position = newPos;
			newB.transform.Translate (hit.normal * 1f);

			newB.gameObject.tag = "imadethis";

		}

	}

	void RemoveCube(){

		Ray ray = myEyes.ScreenPointToRay 
			(new Vector3(Screen.width/2, Screen.height/2, 0f)); 
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 5f)) {

			// Remove cube.
			//Vector3 newPos = hit.transform.position;

			//GameObject newB = Instantiate (hit.collider.gameObject);

			//newB.transform.position = newPos;

			//newB.transform.Translate (hit.normal * 1f);

			if (hit.collider.gameObject.CompareTag ("imadethis")) {
				DestroyObject (hit.collider.gameObject);
			} else if (!hit.collider.gameObject.CompareTag ("imadethis")) {
				hit.transform.Translate (Vector3.down * 1f);
			}

		}


	}

}
