using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedingFarm : MonoBehaviour {

	private GameObject currentBlockType;
	public GameObject[] blockTypes;

	[Tooltip("True for minecraft style voxels")]
	public bool SnapToGrid = true;

	public float amp = 10f;
	public float freq = 10f;

	private Vector3 myPos;

	public int seed = 0;

	private GameObject seedManager;


	void Start () {

		myPos = this.transform.position;
	
		generateTerrain ();
	}


	int getMySeed(){

		GameObject seed_manager = GameObject.Find ("seedManager");

		return seed_manager.GetComponent<seedBrain> ().seed;
	}


	void terrainFeatures(){
		freq = Mathf.Sin(seed * 12f);
		amp = freq * 100f;


	}

	void generateTerrain(){


		seed = getMySeed ();

		//terrainFeatures ();



		int cols = 100;
		int rows = 100;

		for (int x = 0; x < cols; x++) {


			for (int z = 0; z < rows; z++) {

				float y = Mathf.PerlinNoise ((seed + myPos.x + x) / freq, 
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
						new Vector3 (myPos.x + x, y + adjust, myPos.z + z);

					// Now for the head of the tree.
					GameObject treeHead = 
						GameObject.CreatePrimitive (PrimitiveType.Cube);

					treeHead.transform.localScale *= tT.y/4f + 4f;

					treeHead.transform.position =
						new Vector3 (myPos.x + x, y + adjust + tT.y/2f, myPos.z + z);

				}
					

			}

		}


	}

}
