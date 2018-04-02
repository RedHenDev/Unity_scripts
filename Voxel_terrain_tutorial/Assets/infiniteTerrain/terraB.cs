using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terraB : MonoBehaviour {

	public GameObject vFarm;

	public Transform myPlayer;

	public int gridSize;

	Vector3 pOP;



	GameObject[] myFarms = new GameObject[25];

	// Use this for initialization
	void Start () {

		gridSize = vFarm.GetComponent<i_voxelFarm> ().xVoxels/2;

		generateTerrain ();
	}
	
	void generateTerrain(){


		// Lay out a grid of 9 i_voxel_farms.
		// Co-ordinate the seeds of each farm.
		// Signal to each farm to generate its terrain.



		int cols = 3;
		int rows = 3;

		int k = -1;

		Vector3 tP = this.transform.position;

		for (int i = 0; i < cols; i++) {
			for (int j = 0; j < rows; j++){
				k++;
				myFarms [k] = GameObject.Instantiate (vFarm);
				myFarms [k].transform.Translate(Vector3.right * (float)i * gridSize);
				myFarms [k].transform.Translate(Vector3.forward * (float)j * gridSize);
				myFarms [k].GetComponent<i_voxelFarm> ().generateGrid ();
			}
		}

		// Now position player in centre of 5th terrain tile.
		myPlayer.position = myFarms [4].transform.position;
		myPlayer.Translate (Vector3.up * 100f);

		//myPlayer.Translate (Vector3.forward * gridSize);
		myPlayer.Translate (Vector3.right * gridSize);

		pOP = myPlayer.position;


	}




}
