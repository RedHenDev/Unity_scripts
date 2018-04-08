using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mach3_snake : MonoBehaviour {

	// Rate of movement, in seconds.
	public float speed = 0.6f;
	// TimeStamp, for controlling snake movement.
	float timeStamp = 0f;

	// How spread are the segments?
	public float spread = 1.5f;

	// Number of segments.
	public int segmentN = 11;
	int whichSegment = 0;
	int headSegment = 0;

	// Should snake follow a Perlin Noise path?
	// Other, path is random.
	public bool PerlinPath = false;

	// Are we ready to generate our tail?
	public bool ready = false;

	// 1 = forward. 
	int whichD = 1;
	int stepCount = 0;
	// How many steps before change of direction?
	public int stepChange = 10;
	// Total steps, for Perlin Noise.
	int totalSteps = 0;

	// Player's tranform, so that we can place new waypoints.
	Transform playerTrans;
	// What's our waypoint fruit?
	public GameObject waypointFruit;

	public List<GameObject> segments = 
		new List<GameObject> ();

	void Start () {

		// Grab the player's transform.
		playerTrans = GameObject.Find ("snakeWizardEyes").transform;

		// Load waypoint.
		// Load a waypoint marker into the scene!
		waypointFruit = Instantiate (waypointFruit);
		waypointFruit.SetActive (false);

		if (ready) {
			growTail ();
			checkDirection ();
		}

	}

	void growTail(){
		ready = false;

		segments.Add(this.gameObject);

		for (int i = 0; i < segmentN; i++) {
			GameObject s = 
				GameObject.CreatePrimitive(PrimitiveType.Cube);
			s.transform.position = this.transform.position;

			// Set the shader of each segment to that of
			// 'brain' segment.
			s.GetComponent<MeshRenderer> ().material.shader =
				this.GetComponent<MeshRenderer> ().material.shader;

			segments.Add (s);
		}

		whichSegment = segments.Count-1;

	}


	void placeWayPoint(){

		// Record new waypoint.
		waypointFruit.SetActive(true);


		waypointFruit.transform.position = 
			playerTrans.position;

		waypointFruit.transform.rotation = playerTrans.rotation;

		waypointFruit.transform.Translate (Vector3.forward * 5f);



	}

	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			//growSnake ();

			// Place a waypoint.
			placeWayPoint();

		}

		if (Input.GetMouseButtonUp (1))
			shrinkSnake ();

		// If number of seconds elapsed, move.
		if (Time.time - timeStamp > speed) {
			timeStamp = Time.time;
			moveSnake ();

			if (stepCount == stepChange) {
				//Debug.Log ("I should be changing direction...!");
				stepCount = 0;
				// If a waypoint exists, go find it!
				// Else, pick a random/Perlin direction.
				if (waypointFruit.activeSelf)
					gobbleChase ();
				else
					checkDirection ();
			}

		}



	}

	// Add a segment to the end of snake.
	void growSnake(){

		GameObject s = 
			GameObject.CreatePrimitive(PrimitiveType.Cube);

		s.transform.position = 
			segments[whichSegment].transform.position;
		s.transform.rotation = 
			segments[whichSegment].transform.rotation;

		segments.Insert(whichSegment, s);

	}

	// Remove a segment...if possible.
	void shrinkSnake(){

		if (segments.Count == 1)
			return;

		// Only remove segment if *NOT* the 'brain segment'.
		if (segments [whichSegment].gameObject.
			GetComponent<mach3_snake>() ==
			null) {

			GameObject poop = segments [whichSegment].gameObject;
			// Remove the tail-end segment!
			segments.RemoveAt (whichSegment);
			GameObject.DestroyObject (poop);

			// Find new end of tail.
			if (whichSegment > segments.Count - 1)
				whichSegment =
					0;

			// Make sure this new end-segment exists!
			if (whichSegment < 0)
				whichSegment = segments.Count - 1;

			// Make sure that headSegment does not now refer
			// to a deleted segment.
			if (headSegment >= segments.Count)
				headSegment = whichSegment;

		} 


	}

	void checkDirection(){

		// Prevent the snake from moving back through
		// its own body!
		int oldD = whichD;

		if (!PerlinPath) {
			whichD = (int)(Random.Range (1, 7));

			while (oldD + whichD == 7) {
				whichD = (int)(Random.Range (1, 7));
			}
		} else {
			// Use Perlin noise to decide new direction :)
			whichD = 1 + (int)
				(Mathf.PerlinNoise (totalSteps / 6f, 0f) * 6);

			while (oldD + whichD == 7) {
				totalSteps++;
				whichD = 1 + (int)
					(Mathf.PerlinNoise (totalSteps / 6f, 0f) * 6);
			}
		}



		//Debug.Log ("New D = " + whichD);

		// Rotate the snake.
		steerSnake();



	}


	void gobbleChase(){

		// Choose a random number out of 3,
		// which decides which axis snake will consider.

		int ax = (int)Random.Range (1f, 4f);

		//Debug.Log ("My gobble number is " + ax);

		switch (ax) {

		case 1:
			if (this.transform.position.x < waypointFruit.transform.position.x)
				whichD = 3;
			else if (this.transform.position.x > waypointFruit.transform.position.x)
				whichD = 4;
			else
				whichD = 2;
			break;

		case 2:
			if (this.transform.position.y < waypointFruit.transform.position.y)
				whichD = 2;
			else if (this.transform.position.y > waypointFruit.transform.position.y)
				whichD = 5;
			else
				whichD = 1;
			break;

		case 3:
			if (this.transform.position.z < waypointFruit.transform.position.z)
				whichD = 1;
			else if (this.transform.position.z > waypointFruit.transform.position.z)
				whichD = 6;
			else
				whichD = 2;
			break;
		}

		steerSnake ();

	}


	// Rotate snake segments according to whichD, having just
	// been decided.
	void steerSnake(){
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

		for (int i = 0; i < segments.Count; i++) {
			segments [i].transform.rotation
			= segments [whichSegment].transform.rotation;
		}
	}

	void moveSnake(){

		// Move 'whichSegment' in chosen direction.

		// Position end segment on top of head.
		segments [whichSegment].transform.position =
			segments [headSegment].transform.position;

		segments [whichSegment].transform.Translate (
			Vector3.forward
			* spread);

		// Which is our head segment?
		// And which is our tail?
		headSegment = whichSegment;
		whichSegment++;
		if (whichSegment >= segments.Count)
			whichSegment = 0;

		// Adjust colour of new head segment.
		if (headSegment != 0)
		segments [headSegment].GetComponent<MeshRenderer> ()
				.material.color = 
					new Color(0f,Random.Range (0f, 1f),0);

		// Snake has staken a step.
		stepCount++;
		totalSteps++;
	}



}

