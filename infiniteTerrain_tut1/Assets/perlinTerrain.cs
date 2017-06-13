using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perlinTerrain : MonoBehaviour {

	private Mesh myMesh; 

	public float seed = 1;

	void Start () {

		// Get hold of the 3D mesh.
		myMesh = this.GetComponent<MeshFilter> ().mesh;



	}
	

	void Update () {
		if (Input.GetKey(KeyCode.Space))
			generateTerrain ();
	}



	void generateTerrain(){

		seed+=0.0001f;

		float detail = 10f;

		Vector3[] vertices = myMesh.vertices;

		for (int i = 0; i < vertices.Length; i++) {
			//vertices[i].y = Random.Range (0,3);
			vertices[i].y = -3.5f + Mathf.PerlinNoise(this.transform.position.x*detail +
				(vertices[i].x) + seed, 
				this.transform.position.z *detail + (vertices[i].z)) * 7f;
			//Debug.Log ("Y = " + vertices [i].y);
		}

		// Replace the collider and update vertices.
		GameObject.Destroy (this.GetComponent<MeshCollider> ());

		myMesh.vertices = vertices;
		myMesh.RecalculateBounds ();
		myMesh.RecalculateNormals ();
		this.gameObject.AddComponent<MeshCollider> ();

	}




}
