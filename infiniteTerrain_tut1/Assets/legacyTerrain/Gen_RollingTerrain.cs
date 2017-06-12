using UnityEngine;
using System.Collections;

public class Gen_RollingTerrain : MonoBehaviour {

	// Variables for Perlin Noise calculation. Thanks Ken :)
	public int heightScale = 5;
	public float detailScale = 5.0f;
	public bool noHardMesh = false;

	public bool inverted = false;	// For creating cave ceilings etc. Remember to rotate plane itself Z * 180f. NB. Urizen will undo this.

	//public bool genVeg = false;		// True to spawn vegetation at specific height.
	//public GameObject myVeg;

	public float seed = 221188;
	// A legacy of Whale Ride VR :)
	// public GameObject seaweed;

	// For initialization.
	void Start () {

		//int offsetY = 2; 	// Adjustment down of seaweed, so as not to seem to hover over seabed.


		Mesh mesh = this.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		for (int v = 0; v < vertices.Length ; v++)
		{
			// Y scalar of each vertex calculated relative to X and Z.
			// Note that division by this.transform.locaScale allows for varying sizes (scales) of tiles -- see prefab scale.
			if (!inverted)	{
			vertices[v].y = Mathf.PerlinNoise((vertices[v].x + this.transform.position.x/this.transform.localScale.x)/detailScale + seed,
					(vertices[v].z + this.transform.position.z/this.transform.localScale.z)/detailScale + seed) * heightScale;}
			else
				vertices[v].y = Mathf.PerlinNoise((vertices[v].x + this.transform.position.x/this.transform.localScale.x)/detailScale + seed,
					(vertices[v].z + this.transform.position.z/this.transform.localScale.z)/detailScale + seed) * -heightScale;

			/*
			if(vertices[v].y > 0f)
			{
				Vector3 weedPos = new Vector3 (	vertices[v].x + this.transform.position.x,
												vertices[v].y, 
												vertices[v].z + this.transform.position.z);

				Instantiate(myVeg, weedPos, Quaternion.identity);
			}
			*/
			

		}

		// Recalculate normals and add a collider specific to the new arrage of vertices.
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		if (!noHardMesh) this.gameObject.AddComponent<MeshCollider>();

	}

}
