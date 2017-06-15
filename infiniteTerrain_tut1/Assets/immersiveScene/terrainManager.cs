using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainManager : MonoBehaviour {

	public Vector3 startPos = new Vector3(-20, -2, 20);
	public int numberOftilesX = 2;
	public int numberOftilesZ = 2;

	public float detailScale = 5f;
	public float detailScale2 = 42f;
	public float amplitude = 8f;
	public float amplitude2 = 15f;

	public float seed = 1f;
	public float waveSpeed = 0.004f;

	public GameObject perlinTile;

	public bool centreOnSubject = false;

	void Start () {
		//this.transform.position = startPos;

		if (centreOnSubject)
			centreTerrainOnSubject ();


		layTiles();
	}

	void centreTerrainOnSubject(){
		float yOffset = startPos.y;	// Record default y position (do not use player's!)

		startPos = GameObject.Find ("Subject").transform.position;
		startPos.x -= (numberOftilesX * 10f * perlinTile.transform.lossyScale.x) / 2;
		startPos.z -= (numberOftilesZ * 10f * perlinTile.transform.lossyScale.z) / 2;

		// Apply defauly y position -- so as not to use player's!
		startPos.y = yOffset;
	}


	void layTiles(){

		for (int x = 0; x < numberOftilesX; x++) {
			for (int z = 0; z < numberOftilesZ; z++) {
				GameObject newT = Instantiate (perlinTile, startPos, Quaternion.identity);
				newT.transform.Translate (Vector3.forward * z * 10f * newT.transform.lossyScale.z);
				newT.transform.Translate (Vector3.right * x * 10f * newT.transform.lossyScale.x);

				newT.GetComponent<perlinTerrain> ().waveSpeed = waveSpeed;
				newT.GetComponent<perlinTerrain> ().seed = seed;
				newT.GetComponent<perlinTerrain> ().amplitude = amplitude;
				newT.GetComponent<perlinTerrain> ().detailScale2 = detailScale2;
				newT.GetComponent<perlinTerrain> ().amplitude2 = amplitude2;
				newT.GetComponent<perlinTerrain> ().detailScale = detailScale;
				newT.GetComponent<perlinTerrain> ().generateTerrain ();
			}
		}

	}


}
