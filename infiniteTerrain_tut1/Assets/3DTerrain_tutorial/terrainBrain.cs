using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainBrain : MonoBehaviour {

	public int xTiles = 4;
	public int zTiles = 4;

	public GameObject tilePrefab = null;


	public Vector3 terrainO = new Vector3(-20 ,0, 20);


	void Start () {
		layoutTerrain ();
	}


	void layoutTerrain(){


		for (int x = 0; x < xTiles; x++) {
			for (int z = 0; z < zTiles; z++) {
				// Instantiate our prefab.

				Vector3 myPos = terrainO;

				myPos.x += x * 10f *
					tilePrefab.transform.lossyScale.x;
				
				myPos.z += z * 10f *
					tilePrefab.transform.lossyScale.z;

				myPos.y = terrainO.y;

				GameObject newT = GameObject.Instantiate (tilePrefab, 
					myPos, Quaternion.identity);

//				newT.transform.Translate (Vector3.right * x * 10f *
//					tilePrefab.transform.lossyScale.x);
//				newT.transform.Translate (Vector3.forward * z * 10f *
//					tilePrefab.transform.lossyScale.z);

				newT.GetComponent<theHillsAreAlive> ().generatePerlinHills ();
			
			
			}

		}


	}


	void Update () {
		
	}
}
