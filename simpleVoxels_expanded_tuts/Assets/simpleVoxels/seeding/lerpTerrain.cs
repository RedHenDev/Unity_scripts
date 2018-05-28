using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerpTerrain : MonoBehaviour {

		private GameObject currentBlockType;
		public GameObject[] blockTypes;

		[Tooltip("True for minecraft style voxels")]
		public bool SnapToGrid = true;

		public float amp = 10f;
		public float freq = 10f;
	public seedingFarm targetTerrain;
	float targetFreq = 0f;
	float targetAmp = 0f;

		private Vector3 myPos;

		public int seed = 0;

		public bool lerp = false;

		private GameObject seedManager;


		void Start () {

			myPos = this.transform.position;

			// Grab target terrain's Perlin Noise values.
			targetAmp = targetTerrain.amp;
			targetFreq = targetTerrain.freq;

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

			int cols = 50;
			int rows = 50;

			for (int x = 0; x < cols; x++) {


				for (int z = 0; z < rows; z++) {


					bool makeTree = false;

					float y = 0;

					// First octave.
					y = Mathf.PerlinNoise ((seed + myPos.x + x) / (freq*4f), 
						(myPos.z + z) / (freq*4f)) * amp*4f;

					// Orig octave.
					y += Mathf.PerlinNoise ((seed + myPos.x + x) / freq, 
						(myPos.z + z) / freq) * amp;

					// Final octave.
					y += Mathf.PerlinNoise ((seed + myPos.x + x) / freq, 
						(myPos.z + z) / (freq*0.5f)) * (amp*0.5f);


					// *&*&*&*&*&*&*&*&*&*&**&*&&&&**
					// LERP TEST

					if (lerp) {

						// Basic version. Target is just hard-coded amp value.
					//y = Mathf.Lerp (y, 28f, (float)(x*1f/cols));




						// More sophisticated version.
						// Find target value for height (y) along edge of neighbour tile.
						float targetY;
						// Target terrain position, x and z, held in 2D Vector.
					Vector2 targetPos = new 
						Vector2 (seed + myPos.x + targetTerrain.transform.position.x,
						         targetTerrain.transform.position.z);
						
					targetY = Mathf.PerlinNoise ((targetPos.x) / (targetFreq*4f), 
						(myPos.z + z) / (targetFreq*4f)) * targetAmp*4f;
						// Orig octave.
					targetY += Mathf.PerlinNoise ((targetPos.x) / targetFreq, 
						(myPos.z + z) / targetFreq) * targetAmp;
						// Final octave.
					targetY += Mathf.PerlinNoise ((targetPos.x) / targetFreq, 
						(myPos.z + z) / (targetFreq*0.5f)) * (targetAmp*0.5f);

						// Lerp between current y height and target, 1% increment as we
						// move along x-axis voxels towards neighbour terrain tile.
					y = Mathf.Lerp(y, targetY, (float)(x * 1f/cols));



					}



					// *&*&*&*&*&*&*&*&*&*&**&*&&&&**



					if (SnapToGrid) {
						y = Mathf.Floor (y);
					}

					if (y > 24f)
						currentBlockType = 
							blockTypes [1];
					else
						currentBlockType = 
							blockTypes [0];

					GameObject newBlock = 
						GameObject.Instantiate (currentBlockType);

					newBlock.transform.SetParent (this.transform);

					// *&*&*&*&*&*&*&**&*&*&*&*&*&*&*&*&

				// Biomes!

				// Tree biome.
				//				if (myPos.z + z < 52f) {
				//					makeTree = true;
				//					y += Random.value * 1f;
				//				}

				// Tundra biome.
				// Bfreq is the biome's frequency value. No amplitude required.
				float Bfreq = 100f;

				// If in the 'North' on the Z axis, and 
				// if above threshold of 0.3f.
				if (myPos.z + z > 70f + Random.Range(-5,5) &&
					(Mathf.PerlinNoise ((seed+ myPos.x + x) / Bfreq, 
						(myPos.z + z) / Bfreq)
						> 0.3f))
					newBlock.GetComponent<Renderer> ().
					material.color = Color.white;


				// Taiga biome.
				// Bfreq is the biome's frequency value. No amplitude required.
				Bfreq = 24f;
				if (Mathf.PerlinNoise ((seed+ myPos.x + x) / Bfreq, 
					(myPos.z + z) / Bfreq)
					> 0.5f && myPos.z + z < 42f + Random.Range(-5,5)) {
					newBlock.GetComponent<Renderer> ().material.color =
						Color.green;
					makeTree = true;
						//						new Color (	0.9f + Random.value/10f, 
						//									0.9f+ Random.value/10f, 
						//									1f+ Random.value/10f);
						//y += 2f;
					}
					// *&*&*&*&*&*&*&**&*&*&*&*&*&*&*&*&

					// Finally, place block at correct position.
					newBlock.transform.position =
						new Vector3 (myPos.x + x * currentBlockType.transform.localScale.x, 
							y, myPos.z + z * currentBlockType.transform.localScale.z);





					//*********************************************************

					// Now decide whether to create a tree!
					if (Random.value * 100 < 1 && makeTree == true) {
						float adjust = newBlock.transform.lossyScale.y / 2f;

						GameObject treeBabe = 
							GameObject.CreatePrimitive (PrimitiveType.Cube);



						Vector3 tT = treeBabe.transform.localScale;
						tT.y = Random.value * 24;
						treeBabe.transform.localScale =
							tT;

						adjust += treeBabe.transform.localScale.y / 2f;

						treeBabe.transform.position =
							new Vector3 (myPos.x + x * currentBlockType.transform.localScale.x, y + adjust, 
								myPos.z + z * currentBlockType.transform.localScale.z);

						treeBabe.GetComponent<Renderer> ().material.color = new Color(0.6f,0.1f,0.12f);

						// Now for the head of the tree.
						GameObject treeHead = 
							GameObject.CreatePrimitive (PrimitiveType.Cube);

						treeHead.GetComponent<Renderer> ().material.color = new Color (0f, 0.6f, 0f,0.7f);

						treeHead.transform.localScale *= tT.y/4f + 4f;

						treeHead.transform.position =
							new Vector3 (myPos.x + x * currentBlockType.transform.localScale.x
								, y + adjust + tT.y/2f, myPos.z + z * currentBlockType.transform.localScale.z);

					}


				}

			}


		}

	}

