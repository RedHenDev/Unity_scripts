using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainBrain : MonoBehaviour {

	public bool infiniteTerrain = true;

	public int xTiles = 4;
	public int zTiles = 4;

	public GameObject tilePrefab = null;

	private float originalY;

	public Vector3 terrainO = new Vector3(-20 ,0, 20);

	public string playerName = "Subject";

	private Vector3 recordPpos;

	private GameObject[] myTiles; 

	[Tooltip("Press 'u' in game to toggle 'running' terrain.")]
	public float runningSeedSpeed = 0.001f;

	[Tooltip("Terrain's x_Height, y_Gradient, z_Seed.")]
	public Vector3[] tHGS;

	void Start () {

		originalY = terrainO.y;
		// Record player position, zero-ing out Y.
		Vector3 playerP = returnPlayerPos (playerName);
		playerP = new Vector3 (playerP.x, 0f, playerP.z);
		recordPpos = playerP;
		centreTerrainOnPlayer ();

		layoutTerrain ();
		relayTerrain ();
	}


	void centreTerrainOnPlayer(){

		// Change terrainO!

		Vector3 pPos = returnPlayerPos (playerName);

		terrainO = pPos;

		terrainO.x -= (10f * tilePrefab.transform.lossyScale.x *
			(xTiles)) / 2;

		terrainO.z -= (10f * tilePrefab.transform.lossyScale.z *
			(zTiles)) / 2;

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

				myTiles[iNum].GetComponent<theHillsAreAlive> ()
					.generatePerlinHills ();

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

				// Apply terrain properties via array.
				// First, make sure the tile's own array
				// of terrain properties is correct size.
				newT.GetComponent<theHillsAreAlive> ()
					.tHGS = new Vector3 [tHGS.Length];
				// Next, set 'running' terrain speed.
				newT.GetComponent<theHillsAreAlive> ()
					.runningSeedSpeed = runningSeedSpeed;
				for (int o = 0; o < tHGS.Length; o++) {
						// Height.
						newT.GetComponent<theHillsAreAlive> ()
						.tHGS [o].x = tHGS [o].x;
						// Gradient.
						newT.GetComponent<theHillsAreAlive> ()
						.tHGS [o].y = tHGS [o].y;
						// Seed.
						newT.GetComponent<theHillsAreAlive> ()
						.tHGS [o].z = tHGS [o].z;
				}

				newT.GetComponent<theHillsAreAlive> ().generatePerlinHills ();

				iNum++;
				myTiles [iNum] = newT;

			
			}

		}


	}


	void Update () {

		if (Time.frameCount % 10 == 0 &&
		infiniteTerrain)
		checkPlayerWander ();
	}


	void checkPlayerWander(){


		Vector3 playerP = returnPlayerPos (playerName);
		playerP = new Vector3 (playerP.x, 0f, playerP.z);


		float dist = 
			Vector3.Distance (playerP,
			recordPpos);

		if (dist >= (10f * tilePrefab.transform.localScale.z * zTiles)/4) {
			recordPpos = playerP;
			centreTerrainOnPlayer ();
			relayTerrain ();
		}
			

	}


}
