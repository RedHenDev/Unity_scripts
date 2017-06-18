using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainBrain : MonoBehaviour {

	public bool infiniteTerrain = true;
	public bool paintingTerrain = false;

	public int xTiles = 4;
	public int zTiles = 4;

	public GameObject tilePrefab = null;

	private float originalY;
	private float subOrigYaw;

	public Vector3 terrainO = new Vector3(-20 ,0, 20);

	public string playerName = "Subject";

	private Vector3 recordPpos;

	private GameObject[] myTiles;

	// For organising relaying of terrain, one tile per frame.
	private bool remakingTerrain = false;
	private int tileNum = 0;

	[Tooltip("Press 'u' in game to toggle 'running' terrain.")]
	public float runningSeedSpeed = 0.001f;

	[Tooltip("Terrain's x_Height, y_Gradient, z_Seed.")]
	public Vector3[] tHGS;

	void Start () {

		subOrigYaw = returnPlayerTrans (playerName).rotation.eulerAngles.y;
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
	Transform returnPlayerTrans(string _playerName){
		return GameObject.Find (_playerName).transform;
	}

	// Find farthest Perlin tile from player position.
	int findYonder(){

		// Farthest distance so far...
		float dist = 0f;
		int whichTile = 0;

		float newDist = 0f;

		Vector3 sP = returnPlayerPos (playerName);
		sP.y = 0f;

		for (int x = 0; x < xTiles * zTiles; x++) {

			Vector3 tP = new Vector3 (myTiles [x].transform.position.x,
				0f,
				myTiles [x].transform.position.z);

			// Measure distance between perlin tile and player...
			//newDist = Vector3.SqrMagnitude (sP - tP);
			newDist = Vector3.Distance(sP, tP);

			// Record measured distance if greatest distance so far...
			if (newDist > dist) {
				dist = newDist;
				whichTile = x;
			}
		}

		// Move farthest perlin tile to 'grid' position in front of subject...
		//paintOn(whichTile);
		return whichTile;
	}

	// An alternative method for infinite terrain.
	// Moves given perlin tile to correct position in front of subject.
	void paintOn(int _whichTile){

		Vector3 sForward = returnPlayerTrans(playerName).forward;
		sForward.Normalize ();

		myTiles [_whichTile].transform.position = returnPlayerPos (playerName);
		myTiles [_whichTile].transform.Translate(sForward * 
			tilePrefab.transform.lossyScale.z * 10f);

		// Correct for Y.
		myTiles[_whichTile].transform.position = 
			new Vector3(myTiles[_whichTile].transform.position.x,
				originalY,
				myTiles[_whichTile].transform.position.z);

		myTiles [_whichTile].GetComponent<theHillsAreAlive> ().generatePerlinHills ();
	}

	// Another alternative method!
	// Failed. Perhaps all we need is an underlying grid of tiles, which
	// snap into the holes left? Making the transition seamless?
	// This way we are only moving 2 tiles per frame update?
	void relaySmartTerrain(){

		int iNum = -1;
		bool breakNow = false;

		for (int x = 0; x < xTiles; x++) {
			for (int z = 0; z < zTiles; z++) {

				iNum++;

				if (tileNum == iNum) { 

					
					Vector3 myPos = terrainO;

					myPos.x += x * 10f *
					tilePrefab.transform.lossyScale.x;

					myPos.z += z * 10f *
					tilePrefab.transform.lossyScale.z;



					myTiles [iNum].transform.position = myPos;

					myTiles [iNum].GetComponent<theHillsAreAlive> ()
					.generatePerlinHills ();
					breakNow = true;
					tileNum++;
				}
				if (breakNow)
					break;
			}
			if (breakNow)
				break;
		}

		// Make sure we reset makingTerrain and tileNum if finished all tiles.
		if (tileNum >= myTiles.Length - 1) {
			remakingTerrain = false;
			tileNum = 0;
		}

	}

	void relayTerrain(){

		int iNum = -1;

		for (int x = 0; x < xTiles; x++) {
			for (int z = 0; z < zTiles; z++) {

				iNum++;

				Vector3 myPos = terrainO;

				myPos.x += x * 10f *
					tilePrefab.transform.lossyScale.x;

				myPos.z += z * 10f *
					tilePrefab.transform.lossyScale.z;

			

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

		// Alternative 'paint on' terrain method for infinite terrain.
		// This paints a single terrain tile according to the forward
		// direction of the named player.
		// Worse than relayTerrain() method below at moment, but potentially much better.
		if (!remakingTerrain && paintingTerrain && (
		    dist >= 10f * tilePrefab.transform.lossyScale.z ||
			Mathf.Abs(returnPlayerTrans(playerName).
				rotation.eulerAngles.y - subOrigYaw) > 1f)) {

			paintOn (findYonder ());

			subOrigYaw = returnPlayerTrans(playerName).
				rotation.eulerAngles.y;
			return;
		}

		if (!remakingTerrain && ! paintingTerrain && dist >= 
			(10f * tilePrefab.transform.localScale.z * zTiles)/4) {
			recordPpos = playerP;
			centreTerrainOnPlayer ();
			//tileNum = 0;	// Start again!
			relayTerrain ();
			//remakingTerrain = true;
		}

//		if (remakingTerrain) {
//			relaySmartTerrain ();
//		}

	}


}
