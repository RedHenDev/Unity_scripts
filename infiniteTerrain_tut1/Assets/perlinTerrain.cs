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

		seed+=0.01f;

		Vector3[] vertices = myMesh.vertices;

		for (int i = 0; i < vertices.Length; i++) {
			//vertices[i].y = Random.Range (0,3);
			vertices[i].y = -3.5f + Mathf.PerlinNoise(1/(vertices[i].x+0.1f) + seed, 1/(vertices[i].z+0.1f)*seed) * 7f;
			Debug.Log ("Y = " + vertices [i].y);
		}

		// Replace the collider and update vertices.
		GameObject.Destroy (this.GetComponent<MeshCollider> ());

		myMesh.vertices = vertices;
		myMesh.RecalculateBounds ();
		myMesh.RecalculateNormals ();
		this.gameObject.AddComponent<MeshCollider> ();

	}




}
