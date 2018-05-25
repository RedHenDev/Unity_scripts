using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleVoxelFarm : MonoBehaviour {


	private GameObject currentBlockType;
	public GameObject[] blockTypes;

	[Tooltip("True for minecraft style voxels")]
	public bool SnapToGrid = true;

	public float amp = 10f;
	public float freq = 10f;

	public int seed = 0;
	bool seeded = false;
	private GameObject mySeeder;

	private Vector3 myPos;

	int currentX = 0;
	int currentZ = 0;
	bool finishedTerrain;
	public bool useRunningTerrain = false;

	void Start () {

		mySeeder = GameObject.Find ("seed Manager");

		seed = getSeed ();

		myPos = this.transform.position;

		if (useRunningTerrain==false){
			generateTerrain ();
		}
	}
	


	void generateTerrain(){

		int cols = 100;
		int rows = 100;

		for (int x = 0; x < cols; x++) {


			for (int z = 0; z < rows; z++) {
			
				float y = Mathf.PerlinNoise (( seed + myPos.x + x) / freq, 
					          (myPos.z + z) / freq) * amp;

				//y += seedFeatures (x, z);
			
				if (SnapToGrid) {
					y = Mathf.Floor (y);
				}

				if (y > amp / 2)
					currentBlockType = 
					blockTypes [1];
				else
					currentBlockType = 
					blockTypes [0];

				GameObject newBlock = 
					GameObject.Instantiate (currentBlockType);

				newBlock.transform.position =
					new Vector3 (myPos.x + x, y, myPos.z + z);

				// Now decide whether to create a tree!
				if (Random.value * 100 < 0.1) {
					float adjust = newBlock.transform.lossyScale.y / 2f;
					GameObject treeBabe = 
						GameObject.CreatePrimitive (PrimitiveType.Cube);

					Vector3 tT = treeBabe.transform.localScale;
					tT.y = Random.value * 24;
					treeBabe.transform.localScale =
						tT;

					adjust += treeBabe.transform.localScale.y / 2f;

					treeBabe.transform.position =
						new Vector3 (myPos.x + x, y + 1f + adjust, myPos.z + z);
					
				}


				// Make the block a child, so that
				// we can grab it later to reposition.
				newBlock.transform.SetParent(this.transform);

			}

		}

	
	}

	float seedFeatures(int _x, int _z){

		return Mathf.Sin (_x + _z);
	}

	void reposTerrain(){

		if (useRunningTerrain)
			return;

		int cols = 100;
		int rows = 100;

		for (int x = 0; x < cols; x++) {

			for (int z = 0; z < rows; z++) {

				float y = Mathf.PerlinNoise (( seed + myPos.x + x) / freq, 
					(myPos.z + z) / freq) * amp;

				//y += seedFeatures (x, z);

				if (SnapToGrid) {
					y = Mathf.Floor (y);
				}

				int ind = ((z+1) * cols) + (x+1) - cols - 1;

				//if (ind < this.transform.childCount) {
					this.transform.GetChild (ind).transform.position =
						new Vector3 (myPos.x + x, y, myPos.z + z);
				//}
				

			}

		}


	}

	int getSeed(){
		
		return mySeeder.GetComponent<seedManager> ().getSeed ();

	}

	void runningTerrain(){
		
		if (finishedTerrain)
			return;

		int cols = 100;
		int rows = 100;

		currentX++;
		if (currentX == cols) {
			currentX = 1;
			currentZ++;
			if (currentZ == rows) {
				finishedTerrain = true;
			}
		}

		float y = Mathf.PerlinNoise (( seed + myPos.x + currentX) / freq, 
			(myPos.z + currentZ) / freq) * amp;


				if (SnapToGrid) {
					y = Mathf.Floor (y);
				}

				if (y > amp / 2)
					currentBlockType = 
						blockTypes [1];
				else
					currentBlockType = 
						blockTypes [0];

				GameObject newBlock = 
					GameObject.Instantiate (currentBlockType);

				newBlock.transform.position =
			new Vector3 (myPos.x + currentX, y, myPos.z + currentZ);

				// Make the block a child, so that
				// we can grab it later to reposition.
				newBlock.transform.SetParent(this.transform);

	}

	void Update(){
	

//		if (!seeded) {
//			seed = getSeed ();
//			seeded = true;
//			//reposTerrain ();
//		}

		if (useRunningTerrain && seeded && Time.frameCount % 1 == 0) {
			for (int i = 0; i < 64; i++) {
				runningTerrain ();
			}
		}


		if (Input.GetKeyUp(KeyCode.E)) {
			seed += 1;
			reposTerrain();
		}
		else if (Input.GetKeyUp(KeyCode.R)) {
			seed += 1000;
			reposTerrain();
		}
	
	}


}
