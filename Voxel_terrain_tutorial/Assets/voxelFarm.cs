using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voxelFarm : MonoBehaviour {

	public int xVoxels = 4;
	public int zVoxels = 4;

	public Vector3 voxelSize = new Vector3(1f, 1f, 1f);

	public float amp = 3f;
	public float frq = 12f;
	public float seed = 0;

	void Start () {
		generateGrid ();
	}
	
	void generateGrid(){
	

		GameObject[] voxels = new GameObject[xVoxels * zVoxels];

		Vector3 oPos = transform.position;

		int i = -1;

		for (int x = 0; x < xVoxels; x++){

			for (int z = 0; z < zVoxels; z++){

				i++;

				voxels[i] = GameObject.CreatePrimitive (PrimitiveType.Cube);

				oPos = transform.position;
				oPos.y = 0f;
				oPos.x -= xVoxels/2 * voxelSize.x;
				oPos.z -= zVoxels/2 * voxelSize.z;

				oPos.x += x * voxelSize.x;
				oPos.z += z * voxelSize.z;

				// Perlin.
				oPos.y = Mathf.PerlinNoise(	1000000f + (oPos.x * voxelSize.x)/frq, 
					1000000f + seed + (oPos.z * voxelSize.z)/frq) * amp;

				voxels[i].transform.position = oPos;
				voxels[i].transform.localScale = voxelSize;
				voxels[i].transform.parent = transform;

			}
		}

		combineMeshes();

		for (i = 0; i < voxels.Length; i++){
			Destroy(voxels[i]);
		}
	
	}

	void combineMeshes (){

		MeshFilter[] meshes = GetComponentsInChildren<MeshFilter> ();
		CombineInstance[] combined = new CombineInstance[meshes.Length];

//		if (this.gameObject.GetComponent<MeshCollider>() != null)
//		Destroy (this.gameObject.GetComponent<MeshCollider>());

		for (int i = 0; i < meshes.Length; i++) {
			combined [i].mesh = meshes [i].sharedMesh;
			combined [i].transform = meshes [i].transform.localToWorldMatrix;
			meshes [i].gameObject.SetActive (false);
		}

		if (this.gameObject.GetComponent<MeshFilter> () == null)
			this.transform.gameObject.AddComponent<MeshFilter> ();

		this.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
		this.transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combined, true);
		this.transform.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		this.transform.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();

		this.transform.gameObject.AddComponent<MeshCollider> ();

		if (this.gameObject.GetComponent<MeshRenderer> () == null)
		this.gameObject.AddComponent<MeshRenderer> ();

		this.transform.gameObject.SetActive (true);

	}

	void Update () {
		
	}
}
