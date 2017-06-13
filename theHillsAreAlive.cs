using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theHillsAreAlive : MonoBehaviour {


	private Mesh myMeshFilter;

	public float gradient = 5f;
	public float height = 5f;

	private MeshCollider myCollider;

	public float seed = 0f;

	private Vector3[] vertices;

	public bool isMoving = true;

	void Start () {


		myMeshFilter = this.
			GetComponent<MeshFilter> ().mesh;

		myCollider = this.
			GetComponent<MeshCollider> ();

		generatePerlinHills ();

	}


	void generatePerlinHills(){

		vertices = myMeshFilter.vertices;

		//Debug.Log ("There are " + vertices.Length +
		//" vertices on the plane!");


		float pX = 0;
		float pZ = 0;

		for (int i = 0; i < vertices.Length; i++) {


			pX = (this.transform.position.x
				+ vertices [i].x) / gradient; 
			pZ = (this.transform.position.z
				+ vertices [i].z) / gradient + seed;

			vertices [i].y = Mathf.PerlinNoise (pX, pZ) *
												height; 


		}


		myMeshFilter.vertices = vertices;
		myMeshFilter.RecalculateBounds ();
		myMeshFilter.RecalculateNormals ();
		myCollider.sharedMesh = null;
		myCollider.sharedMesh = myMeshFilter;

	}


	void Update () {
	
		if (Input.GetKeyUp (KeyCode.Space))
			isMoving = !isMoving;
			

		if (isMoving) {
			seed += 0.03f;
			generatePerlinHills ();
		}




	}
}
