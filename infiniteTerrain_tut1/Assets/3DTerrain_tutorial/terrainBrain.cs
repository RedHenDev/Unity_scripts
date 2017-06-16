using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainBrain : MonoBehaviour {

	public int xTiles = 4;
	public int zTiles = 4;

	public GameObject tilePrefab = null;

	private float originalY;

	public Vector3 terrainO = new Vector3(-20 ,0, 20);

	public string playerName = "Subject";

	private Vector3 recordPpos;


	private GameObject[] myTiles; 

	void Start () {

		originalY = terrainO.y;
		recordPpos = returnPlayerPos (playerName);
		centreTerrainOnPlayer ();

		layoutTerrain ();
	}


	void centreTerrainOnPlayer(){

		// Change terrainO!

		Vector3 pPos = returnPlayerPos (playerName);

		terrainO = pPos;

		terrainO.x -= (10f * tilePrefab.transform.lossyScale.x *
		(xTiles - 1)) / 2;

		terrainO.z -= (10f * tilePrefab.transform.lossyScale.z *
			(zTiles - 1)) / 2;

		terrainO.y = originalY; 

	}

	Vector3 returnPlayerPos(string _playerName){

		return GameObject.Find (_playerName).transform.position;

	}


	void relayTerrain(){

		int iNum = -1;

		for (int x = 0; x < xTiles; x++) {
			for (int z = 0; z < zTiles; z++) {
				// Instantiate our prefab.

				Vector3 myPos = terrainO;

				myPos.x += x * 10f *
					tilePrefab.transform.lossyScale.x;

				myPos.z += z * 10f *
					tilePrefab.transform.lossyScale.z;

				myPos.y = terrainO.y;

				iNum++;

				myTiles [iNum].transform.position = myPos;

				myTiles[iNum].GetComponent<theHillsAreAlive> ().generatePerlinHills ();

			}

		}


	}

	void layoutTerrain(){


		myTiles = new GameObject[xTiles * zTiles];

		int iNum = -1;

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

				iNum++;
				myTiles [iNum] = newT;

			
			}

		}


	}


	void Update () {
		checkPlayerWander ();
	}


	void checkPlayerWander(){

		float dist = 
			Vector3.Distance (returnPlayerPos (playerName),
			recordPpos);

		if (dist > 10f * tilePrefab.transform.localScale.z) {
			recordPpos = returnPlayerPos (playerName);
			centreTerrainOnPlayer ();
			relayTerrain ();
		}
			

	}


}
