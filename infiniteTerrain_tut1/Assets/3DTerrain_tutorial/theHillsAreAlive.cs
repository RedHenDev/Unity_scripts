using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theHillsAreAlive : MonoBehaviour {


	private Mesh myMeshFilter = null;

	// Terrain properties.
	[HideInInspector]
	public float gradient = 5f;
	[HideInInspector]
	public float height = 5f;
	[HideInInspector]
	public float seed = 0f;
	// We also now need a 'running seed'
	// as a separate variable, since the
	// seed itself will be reset each time
	// the Perlin noise is applied.
	private float runningSeed = 0f;
	[HideInInspector]
	public float runningSeedSpeed = 0.001f;
	[HideInInspector]
	public Vector3[] tHGS;

	private MeshCollider myCollider = null;

	private Vector3[] vertices;

	[HideInInspector]
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

			// Apply array of terrain properties.
			// Zero out the y component first, else
			// terrain will stretch out of shape over time.
			vertices [i].y = 0f;
			for (int o = 0; o < tHGS.Length; o++) {
				height = tHGS [o].x;
				gradient = tHGS [o].y;
				seed = tHGS [o].z;

				// Perlin noise!
				pX = (1000000 + this.transform.position.x
				+ vertices [i].x * this.transform.lossyScale.x) / gradient; 
				pZ = (1000000 + this.transform.position.z
					+ vertices [i].z * this.transform.lossyScale.z) / gradient + seed + runningSeed;

				vertices [i].y += -height/2 + Mathf.PerlinNoise 
					(pX, pZ) * height; 
			}

		}


		myMeshFilter.vertices = vertices;
		myMeshFilter.RecalculateBounds ();
		myMeshFilter.RecalculateNormals ();
		myCollider.sharedMesh = null;
		myCollider.sharedMesh = myMeshFilter;

	}


	void Update () {
	
		if (Input.GetKeyUp (KeyCode.U))
			isMoving = !isMoving;
			

		if (isMoving) {
			runningSeed += runningSeedSpeed;
			generatePerlinHills ();
		}




	}
}
