using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_voxelFarm : MonoBehaviour {

	public int xVoxels = 50;
	public int zVoxels = 50;

	GameObject[] voxels;
	bool createdVoxels = false;

	[Tooltip("True for minecraft style voxels")]
	public bool SnapToGrid = true;

	public Vector3 voxelSize = new Vector3(1f, 1f, 1f);

	public float amp = 7f;
	public float frq = 12f;
	public float seed = 99;



	public void generateGrid(){

		// Elongate voxels, to give illusion of layers.
		voxelSize = new Vector3(1f, 10f, 1f);

		// Make sure we are working with a blank slate
		// in case of recalculation of Perlin Noise.
		if (this.GetComponent<MeshFilter>() != null)
			//GameObject.Destroy(this.GetComponent<MeshFilter>());
		this.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();

		if (!createdVoxels) {
			voxels = new GameObject[xVoxels * zVoxels];
			//Debug.Log (this + " created new voxels @ " + Time.frameCount );
		}

		Vector3 oPos;// = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		int i = -1;

		for (int x = 0; x < xVoxels; x++){

			for (int z = 0; z < zVoxels; z++){

				i++;

				// Only instantiate voxel if not already instantiated.
				if (createdVoxels)
					voxels [i].SetActive (true);
				else {
					voxels [i] = GameObject.CreatePrimitive (PrimitiveType.Cube);

				}


				oPos = this.transform.position;
				oPos.y = 0f;
				oPos.x -= xVoxels/2 * voxelSize.x;
				oPos.z -= zVoxels/2 * voxelSize.z;

				oPos.x += x * voxelSize.x;
				oPos.z += z * voxelSize.z;

				// Perlin.
				//float o1 = Mathf.PerlinNoise(	( seed + 1000000f + (this.transform.position.x + oPos.x))/200f, 
				//	( 1000000f + (this.transform.position.z + oPos.z ))/2000f) * 200f;


				//float o2 = Mathf.PerlinNoise(	( seed + 1000000f + (this.transform.position.x + oPos.x))/80f, 
				//	( 1000000f + (this.transform.position.z + oPos.z ))/800f) * 100f;

				//float o3 = Mathf.PerlinNoise(	( seed + 1000000f + (this.transform.position.x + oPos.x))/7f, 
				//	( 1000000f + (this.transform.position.z + oPos.z ))/7f) * 3f;

				//oPos.y += o1 + o2 + o3;

				oPos.y += Mathf.PerlinNoise(	( seed + 1000000f + (this.transform.position.x + oPos.x))/frq, 
					( 1000000f + (this.transform.position.z + oPos.z ))/frq) * amp;

				// Snap to grid.
				if (SnapToGrid) {
					oPos.y = Mathf.Floor (oPos.y);
				}

				voxels[i].transform.position = oPos;
				voxels[i].transform.localScale = voxelSize;
				voxels[i].transform.SetParent(this.transform);

			}
		}

		createdVoxels = true;
		combineMeshes();

		for (int j = 0; j < voxels.Length; j++){
			voxels [i].transform.SetParent (null);
			//voxels [i].transform.position = Vector3.zero;
			voxels [j].SetActive (false);
			//Destroy(voxels[i]);
		}

	}

	void combineMeshes (){

		MeshFilter[] meshes = GetComponentsInChildren<MeshFilter> ();
		CombineInstance[] combined = new CombineInstance[meshes.Length];

				if (this.gameObject.GetComponent<MeshCollider>() != null)
				Destroy (this.gameObject.GetComponent<MeshCollider>());

		for (int i = meshes.Length - 1; i >= 0; i--) {
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
}
