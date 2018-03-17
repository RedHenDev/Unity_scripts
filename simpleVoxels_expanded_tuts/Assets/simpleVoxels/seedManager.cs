using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedManager : MonoBehaviour {

	public bool randomize = true;
	bool seedRandomized = false;

	public int seed = 911;

	void randomizeSeed(){
		// We can only randomize the seed once.
		if (seedRandomized)
			return;
		seed = Mathf.RoundToInt(Random.Range (0, 1000));
		seedRandomized = true;
	}

	// Have we randomized our seed yet?
	// If not, do this first before returning the seed number.
	public int getSeed(){
		if (randomize)
			randomizeSeed ();
		return seed;
	}

}
