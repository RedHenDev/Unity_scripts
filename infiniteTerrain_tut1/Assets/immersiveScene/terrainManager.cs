using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainManager : MonoBehaviour {

	public Vector3 startPos = new Vector3(0,-2,0);
	public int numberOftilesX = 2;
	public int numberOftilesZ = 2;

	public float detailScale = 5f;
	public float amplitude = 8f;

	public GameObject perlinTile;

	void Start () {
		//this.transform.position = startPos;
		layTiles();
	}
	



	void layTiles(){

		for (int x = 0; x < numberOftilesX; x++) {
			for (int z = 0; z < numberOftilesZ; z++) {
				GameObject newT = Instantiate (perlinTile, startPos, Quaternion.identity);
				newT.transform.Translate (Vector3.forward * z * 10f * newT.transform.lossyScale.z);
				newT.transform.Translate (Vector3.right * x * 10f * newT.transform.lossyScale.x);

				newT.GetComponent<perlinTerrain> ().amplitude = amplitude;
				newT.GetComponent<perlinTerrain> ().detailScale = detailScale;
				newT.GetComponent<perlinTerrain> ().generateTerrain ();
			}
		}

	}


}
