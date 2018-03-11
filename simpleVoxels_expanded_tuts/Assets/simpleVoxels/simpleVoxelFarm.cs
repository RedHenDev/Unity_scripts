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

	public int seed = 1;

	private Vector3 myPos;

	void Start () {
		generateTerrain ();
		reposTerrain ();
	}
	


	void generateTerrain(){

		myPos = this.transform.position;

		int cols = 100;
		int rows = 100;

		for (int x = 0; x < cols; x++) {


			for (int z = 0; z < rows; z++) {
			
				float y = Mathf.PerlinNoise (( seed + myPos.x + x) / freq, 
					          (myPos.z + z) / freq) * amp;

			
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

				// Make the block a child, so that
				// we can grab it later to reposition.
				newBlock.transform.SetParent(this.transform);

			}

		}

	
	}

	void reposTerrain(){

		int cols = 100;
		int rows = 100;

		for (int x = 0; x < cols; x++) {

			for (int z = 0; z < rows; z++) {

				float y = Mathf.PerlinNoise (( seed + myPos.x + x) / freq, 
					(myPos.z + z) / freq) * amp;

				// Added terrain features via seed.
				if (seed % 9 == 0)
					y += Mathf.Sin (z + x)/2.2f;

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
		

	void Update(){
	
		if (Input.GetKey(KeyCode.E)) {
			seed += 1;
			reposTerrain();
		}
		else if (Input.GetKey(KeyCode.R)) {
			seed += 1000;
			reposTerrain();
		}
	
	}


}
