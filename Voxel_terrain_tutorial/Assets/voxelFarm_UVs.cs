using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voxelFarm_UVs : MonoBehaviour {

	public int xVoxels = 12;
	public int zVoxels = 12;

	[Tooltip("True for minecraft style voxels")]
	public bool SnapToGrid = true;

	public Vector3 voxelSize = new Vector3(1f, 1f, 1f);

	public float amp = 7f;
	public float frq = 12f;
	public float seed = 99;

	public int pixels = 16;
	public int tileU = 1;
	public int tileV = 1;

	public Camera myCamera;

	void Start () {
		generateGrid ();
	}
	
	void generateGrid(){

		// For texture atlas.
		//tileU = Mathf.FloorToInt(Random.Range(0,15));
		//tileV = Mathf.FloorToInt(Random.Range(0,15));
		//setUVs (this.gameObject);

		GameObject[] voxels = new GameObject[xVoxels * zVoxels];

		Vector3 oPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		int i = -1;

		for (int x = 0; x < xVoxels; x++){

			for (int z = 0; z < zVoxels; z++){

				i++;

				voxels[i] = GameObject.CreatePrimitive (PrimitiveType.Cube);

				oPos = this.transform.position;
				oPos.y = 0f;
				oPos.x -= xVoxels/2 * voxelSize.x;
				oPos.z -= zVoxels/2 * voxelSize.z;

				oPos.x += x * voxelSize.x;
				oPos.z += z * voxelSize.z;

				 //Snap to grid.
//				oPos.x = Mathf.Round (oPos.x);
//				oPos.z = Mathf.Round (oPos.z);

				// Perlin.
				oPos.y += Mathf.PerlinNoise(	(1000000f + (this.transform.position.x + oPos.x))/frq, 
					(seed + 1000000f + (this.transform.position.z + oPos.z ))/frq) * amp;

				// Snap to grid.
				if (SnapToGrid) {
					oPos.y = Mathf.Floor (oPos.y);
				}

			
				// For texture atlas.
//				tileU = Mathf.FloorToInt(Random.Range(0,15));
//				tileV = Mathf.FloorToInt(Random.Range(0,15));
//				if (i < 100)
//					Debug.Log ("U = " + tileU + " V = " + tileV);
//				
//				setUVs ( voxels[i]);




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
//				Destroy (this.gameObject.GetComponent<MeshCollider>());
		// New uv stuff.
	

		for (int i = meshes.Length - 1; i >= 0; i--) {
			combined [i].mesh = meshes [i].sharedMesh;
			combined [i].transform = meshes [i].transform.localToWorldMatrix;

			meshes [i].gameObject.SetActive (false);
		}


		// New uv stuff.
		//Vector2[] oldUVs = this.GetComponent<MeshFilter>().mesh.uv;
		//Vector2[] newUVs = new Vector2[oldUVs.Length+24];


		if (this.gameObject.GetComponent<MeshFilter> () == null)
			this.transform.gameObject.AddComponent<MeshFilter> ();


		this.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
		this.transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combined, true);
		this.transform.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		this.transform.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();

		this.transform.gameObject.AddComponent<MeshCollider> ();

//		if (this.gameObject.GetComponent<MeshRenderer> () == null)
//			this.gameObject.AddComponent<MeshRenderer> ();

		this.transform.gameObject.SetActive (true);


	}
	

	void Update () {

		if (Input.GetKeyUp (KeyCode.U)) {
			seed += 1;
			//this.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
			generateGrid ();
		}



	}



	void setUVs(GameObject _whichObject){

		float pixelScale = 1 / pixels ;

		float uStart = pixelScale * tileU;
		float uEnd = pixelScale * (tileU+1);
		float vStart = pixelScale * tileV;
		float vEnd = pixelScale * (tileV+1);

		Vector2[] uvVoxels = new Vector2[24];

		uvVoxels[0] = new Vector2(uStart, vStart);
		uvVoxels[1] = new Vector2(uEnd, vStart);
		uvVoxels[2] = new Vector2(uStart, vEnd);
		uvVoxels[3] = new Vector2(uEnd, vEnd);
		uvVoxels[4] = new Vector2(uStart, vEnd);
		uvVoxels[5] = new Vector2(uEnd, vEnd);
		uvVoxels[6] = new Vector2(uStart, vEnd);
		uvVoxels[7] = new Vector2(uEnd, vEnd);
		uvVoxels[8] = new Vector2(uStart, vStart);
		uvVoxels[9] = new Vector2(uEnd, vStart);
		uvVoxels[10] = new Vector2(uStart, vStart);
		uvVoxels[11] = new Vector2(uEnd, vStart);
		uvVoxels[12] = new Vector2(uStart, vStart);
		uvVoxels[13] = new Vector2(uStart, vEnd);
		uvVoxels[14] = new Vector2(uEnd, vEnd);
		uvVoxels[15] = new Vector2(uEnd, vStart);
		uvVoxels[16] = new Vector2(uStart, vStart);
		uvVoxels[17] = new Vector2(uStart, vEnd);
		uvVoxels[18] = new Vector2(uEnd, vEnd);
		uvVoxels[19] = new Vector2(uEnd, vStart);
		uvVoxels[20] = new Vector2(uStart, vStart);
		uvVoxels[21] = new Vector2(uStart, vEnd);
		uvVoxels[22] = new Vector2(uEnd, vEnd);
		uvVoxels[23] = new Vector2(uEnd, vStart);



		_whichObject.transform.GetComponent<MeshFilter> ().mesh.uv = uvVoxels;

		//Debug.Log(_whichObject.GetComponent<MeshFilter> ().mesh.uv[0]);

	}


}
