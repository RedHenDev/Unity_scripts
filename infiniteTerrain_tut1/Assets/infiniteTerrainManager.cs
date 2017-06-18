using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infiniteTerrainManager : MonoBehaviour {

	public Vector3 startPos = new Vector3(-20, -2, 20);
	public int numberOftilesX = 2;
	public int numberOftilesZ = 2;

	public float detailScale = 5f;
	public float detailScale2 = 42f;
	public float amplitude = 8f;
	public float amplitude2 = 15f;

	public float seed = 1f;
	public float waveSpeed = 0.004f;

	public GameObject perlinTile = null;

	private float latestDist = 0f;

	public bool centreOnSubject = false;

	public string playerName = "Subject";

	private GameObject[] myTiles;

	Vector3 subOrigPos;
	float subOrigYaw;

	float originalY = 0;

	// Are we adjusting the entire terrain (rollOn2()), or just one tile using the
	// 'paint' where I look method (rollOn())?
	public bool paintTerrain = true;

	void Start () {
		//this.transform.position = startPos;

		originalY = startPos.y; 
		if (centreOnSubject)
			centreTerrainOnSubject ();
		

		// Record original pos of subject, so that we know
		// how far they have moved, so as to trigger new
		// terrain generation.
		subOrigPos = getSubjectPos (playerName);
		subOrigYaw = getSubjectTrans(playerName).rotation.eulerAngles.y;

		layTiles();
	}

	// Returns position of player (subject) by name of their gameObject.
	Vector3 getSubjectPos(string subjectName){
		return GameObject.Find (subjectName).transform.position;
	}

	Transform getSubjectTrans(string subjectName){
		return GameObject.Find (subjectName).transform;
	}

	// Find farthest Perlin tile from player position.
	int findYonder(){

		// Farthest distance so far...
		float dist = 0f;
		int whichTile = 0;

		float newDist = 0f;

		Vector3 sP = new Vector3 (	getSubjectPos (playerName).x,
			0f,
			getSubjectPos (playerName).z);

		for (int x = 0; x < numberOftilesX * numberOftilesZ; x++) {

			Vector3 tP = new Vector3 (myTiles [x].transform.position.x,
				0f,
				myTiles [x].transform.position.z);

			// Measure distance between perlin tile and player...
			//newDist = Vector3.SqrMagnitude (sP - tP);
			newDist = Vector3.Distance(sP, tP);

			// Record measured distance if greatest distance so far...
			if (newDist > dist) {
				latestDist = dist = newDist;
				whichTile = x;
			}
		}

		// Move farthest perlin tile to 'grid' position in front of subject...
		//rollOn(whichTile);
		return whichTile;
	}

	// Moves given perlin tile to correct position in front of subject.
	void rollOn(int _whichTile){

		Vector3 sForward = new Vector3 
			(getSubjectTrans (playerName).
			forward.x, 0f, getSubjectTrans (playerName).forward.z);
		sForward.Normalize ();

		myTiles [_whichTile].transform.position = getSubjectPos (playerName);
		myTiles [_whichTile].transform.Translate(sForward * 
			perlinTile.transform.lossyScale.z * 10f);

		myTiles[_whichTile].transform.position = 
			new Vector3(myTiles[_whichTile].transform.position.x,
			originalY,
			myTiles[_whichTile].transform.position.z);

		myTiles [_whichTile].GetComponent<perlinTerrain> ().generateTerrain ();
	}

	void rollOn2(){
		// The idea here is to move the entire grid of perlin tiles in the direction
		// the player is facing.
		centreTerrainOnSubject();
		relayTiles ();
	}

	void relayTiles(){
		int tNum = -1; // Index number of tiles 0->(total-1).

		for (int x = 0; x < numberOftilesX; x++) {
			for (int z = 0; z < numberOftilesZ; z++) {

				tNum++;
				myTiles [tNum].transform.position = startPos;
				myTiles[tNum].transform.Translate (Vector3.forward * z * 10f * myTiles[tNum].transform.lossyScale.z);
				myTiles[tNum].transform.Translate (Vector3.right * x * 10f * myTiles[tNum].transform.lossyScale.x);


				myTiles [tNum].GetComponent<perlinTerrain> ().generateTerrain ();

	
			}

		}
	}


	void centreTerrainOnSubject(){
		//float yOffset = originalY;	// Record default y position (do not use player's!)

		startPos = getSubjectPos (playerName);
		startPos.x -= ((numberOftilesX-1) * 10f * perlinTile.transform.lossyScale.x) / 2;
		startPos.z -= ((numberOftilesZ-1) * 10f * perlinTile.transform.lossyScale.z) / 2;

		// Apply defauly y position -- so as not to use player's!
		startPos.y = originalY;
	}


	void layTiles(){

		myTiles = new GameObject[numberOftilesX * numberOftilesZ];

		int tNum = -1; // Index number of tiles 0->(total-1).

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

				tNum++;
				myTiles [tNum] = newT;
			}

		}

	}


	void Update(){
		if (Input.GetKeyUp (KeyCode.K)) {
			//myTiles [20].transform.Translate (Vector3.up * -1f);
			//rollOn (findYonder());
			//rollOn2();
		}


		//if (Time.frameCount % 60 == 0)
			checkTerrain ();

	}


	void checkTerrain(){
		Vector3 sP = new Vector3 (getSubjectPos (playerName).x,
			             0f,
			             getSubjectPos (playerName).z);

		Vector3 soP = new Vector3 (subOrigPos.x,
			              0f,
			              subOrigPos.z);
		
		if (!paintTerrain){
			if (Vector3.Distance (soP, sP) > (10f * perlinTile.transform.lossyScale.z) / 4) {
				rollOn2 ();
				subOrigPos = getSubjectPos (playerName);
			}
		}
		else if (Vector3.Distance (soP, sP) > 
				(perlinTile.transform.lossyScale.z * 10f)/4 ||
				Mathf.Abs(getSubjectTrans(playerName).
				rotation.eulerAngles.y - subOrigYaw) > 0.5f) {
				
					rollOn (findYonder ());

					subOrigYaw = getSubjectTrans(playerName).
					rotation.eulerAngles.y;
				subOrigPos = getSubjectPos (playerName);
				}
	}

}
