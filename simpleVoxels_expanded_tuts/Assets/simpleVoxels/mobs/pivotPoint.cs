using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivotPoint : MonoBehaviour {

	Color colour = Color.green;

	void OnDrawGizmos(){
		Gizmos.color = colour;
		Gizmos.DrawWireSphere (this.transform.position, 0.25f);
	}
}
