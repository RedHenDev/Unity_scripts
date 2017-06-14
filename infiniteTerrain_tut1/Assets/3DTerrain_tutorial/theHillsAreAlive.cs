using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theHillsAreAlive : MonoBehaviour {


	private Mesh myMeshFilter = null;

	public float gradient = 5f;
	public float height = 5f;

	private MeshCollider myCollider = null;

	public float seed = 0f;

	private Vector3[] vertices;

	public bool isMoving = false;

	void Start () {




		//generatePerlinHills ();

	}

	void grabComponents(){

		myMeshFilter = this.
			GetComponent<MeshFilter> ().mesh;

		myCollider = this.
			GetComponent<MeshCollider> ();
	}

	public void generatePerlinHills(){

		if (myMeshFilter == null)
			grabComponents ();

		vertices = myMeshFilter.vertices;

		//Debug.Log ("There are " + vertices.Length +
		//" vertices on the plane!");


		float pX = 0;
		float pZ = 0;

		for (int i = 0; i < vertices.Length; i++) {


			pX = (1000000 + this.transform.position.x
				+ vertices [i].x * this.transform.lossyScale.x) / gradient; 
			pZ = (1000000 + this.transform.position.z
				+ vertices [i].z * this.transform.lossyScale.z) / gradient + seed;

			vertices [i].y = Mathf.PerlinNoise 
				(pX , pZ ) * height; 


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
