using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeBrain : MonoBehaviour {


	// Rate of movement, in seconds.
	float speed = 0.1f;
	// TimeStamp, for controlling snake movement.
	float timeStamp = 0f;

	// How spread are the segments?
	float spread = 1.5f;

	// Number of segments.
	int segmentN = 11;
	int whichSegment = 0;
	int headSegment = 0;

	// Are we ready to generate our tail?
	public bool ready = false;

	// 1 = forward. 
	int whichD = 1;
	int stepCount = 0;

	// Array of segments.
	public GameObject[] segments;

	void Start () {

		if (ready)
		generateTail ();
		
	}


	void generateTail(){
		// We want to clone the head, and
		// position n tail segments behind us.

		ready = false;

		segments = new GameObject[segmentN];

		segments [0] = this.gameObject;


		for (int i = 1; i < segmentN; i++) {
		
			GameObject s = 
				GameObject.CreatePrimitive(PrimitiveType.Cube);
			s.transform.position = this.transform.position;
			s.transform.Translate
			(this.transform.forward * -i * spread);

			segments [i] = s;
		}

		// Which was the last segment in the array?
		whichSegment = segments.Length - 1;

	}


	void Update () {

		// If number of seconds elapsed, move.
		if (Time.time - timeStamp > speed) {
			timeStamp = Time.time;
			moveSnake ();


			if (stepCount == 10) {
				//Debug.Log ("I should be changing direction...!");
				checkDirection ();
			}
				

		}



	}

	void checkDirection(){
		stepCount = 0;

		// Prevent the snake from moving back through
		// its own body!
		int oldD = whichD;

		whichD = (int)(Random.Range (1, 7));

		while (oldD + whichD == 7) {
			whichD = (int)(Random.Range (1, 7));
		}

		//Debug.Log ("New D = " + whichD);

		// Rotate the snake.

		switch (whichD){
		case 1:
			segments [whichSegment].transform.
			eulerAngles = new Vector3 (0f, 0f, 0f);
			break;
		case 2:
			segments [whichSegment].transform
				.eulerAngles = new Vector3 (-90f, 0f, 0);
			break;
		case 3:
			segments [whichSegment].transform
				.eulerAngles = new Vector3 (0f, 90f, 0f);
			break;
		case 4:
			segments [whichSegment].transform
				.eulerAngles = new Vector3 (0f, -90f, 0f);
			break;
		case 5:
			segments [whichSegment].transform
				.eulerAngles = new Vector3 (90f, 0f, 0f);
			break;
		case 6:
			segments [whichSegment].transform
				.eulerAngles = new Vector3 (0f, 180f, 0f);
			break;


		}

		for (int i = 0; i < segments.Length; i++) {
			segments [i].transform.rotation
			= segments [whichSegment].transform.rotation;
		}


	}


	void moveSnake(){

		// Move 'whichSegment' in chosen direction.

		// Position end segment on top of head.
		segments [whichSegment].transform.position =
			segments [headSegment].transform.position;
		// Move new head in chosen direction.
//		if (whichD == 1) {

			segments [whichSegment].transform.Translate (
			Vector3.forward
				* spread);
//		} else if (whichD == 2) {
			
//			segments [whichSegment].transform.Translate (
//				segments[whichSegment].transform.right
//				* -spread);
//		} else if (whichD == 3) {
//
//			segments [whichSegment].transform.Translate (
//				segments[whichSegment].transform.right
//				* spread);
//		} else if (whichD == 4) {
//
//			segments [whichSegment].transform.Translate (
//				segments[whichSegment].transform.up
//				* spread);
//		} else if (whichD == 5) {
//			
//			segments [whichSegment].transform.Translate (
//				segments[whichSegment].transform.up
//				* -spread);
//		} else if (whichD == 6) {
//
//			segments [whichSegment].transform.Translate (
//				segments[whichSegment].transform.forward
//				* -spread);
//		}
		


			// Which is our head segment?
			// And which is our tail?
			headSegment = whichSegment;
		whichSegment = whichSegment - 1;
		if (whichSegment == -1)
			whichSegment = segments.Length - 1;


		//segments [headSegment].GetComponent<MeshRenderer> ()
		//	.material.color = Random.ColorHSV ();

		// Snake has staken a step.
		stepCount++;

	}



}
