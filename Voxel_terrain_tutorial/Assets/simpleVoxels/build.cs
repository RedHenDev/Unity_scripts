using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build : MonoBehaviour {

	public Camera myCamera;

	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			tryBuild ();
		}
		if (Input.GetMouseButtonUp (1)) {
			tryDig ();
		}
	}


	void tryDig(){
		Ray ray = myCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0f));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 20.0f)) {
			//Vector3 newBlockPos = hit.collider.transform.position;
			//newBlockPos -= hit.normal * 1f;

			if (hit.collider.gameObject.CompareTag ("synthetic")) {
				DestroyObject (hit.collider.gameObject);
			} else if (!hit.collider.gameObject.CompareTag ("synthetic")) {

				hit.collider.transform.Translate (Vector3.down * 1f);
			}

		}
	}

	void tryBuild(){
		Ray ray = myCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0f));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 20.0f)) {
			Vector3 newBlockPos = hit.collider.transform.position;
			newBlockPos += hit.normal * 1f;
			GameObject 
			newB = GameObject.Instantiate (hit.collider.gameObject);
			newB.gameObject.tag = "synthetic";
			newB.transform.position = newBlockPos;

		}
	}


}
