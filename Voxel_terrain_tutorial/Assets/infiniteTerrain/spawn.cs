using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour {


	// Should I be spawning?
	public bool on = false;

	// What to spawn.
	public GameObject wts;
	// What to spawn (if in brute mode).
	public GameObject wts_brute;

	// Pool of objects.
	GameObject[] pObs;
	public int amount = 99;

	// Have we loaded/instantiated our objects?
	private bool pooled = false;

	// Should we spawn in the ordinary way?
	public bool bruteSpawn = false;

	void _spawn (){
		GameObject newOb = GameObject.Instantiate (wts_brute);

		newOb.transform.position = this.transform.position + 
			(Vector3.up * 5f) + (Vector3.right * Random.Range(-1f,1f)) +
			(Vector3.forward * Random.Range(-1f,1f));

	}

	void loadPool(){

		pObs = new GameObject [amount];

		for (int i = 0; i < pObs.Length; i++) {
			pObs[i] = GameObject.Instantiate (wts);
			pObs[i].SetActive (false);
		}
		pooled = true;
	}

	public void _poolSpawn(){

		// How many to spawn at a time?
		int k = 12;
		int kn = 0;

		for (int i = 0; i < pObs.Length; i++) {
			if (pObs [i].activeSelf == false) {
				pObs [i].SetActive (true);
				pObs[i].transform.position = this.transform.position + 
					(Vector3.up * 5f) + (Vector3.right * Random.Range(-1f,1f)) +
					(Vector3.forward * Random.Range(-1f,1f));
				kn++;
				if (kn >= k) break;
			}
		}
	}

	void Update () {


		if (!pooled)
			loadPool ();

		if (Input.GetKeyUp(KeyCode.G)) {
			on = !on;
		}

		if (Input.GetKeyUp(KeyCode.B)) {
			bruteSpawn = !bruteSpawn;

		}

		if (on) {
			if (bruteSpawn)
			_spawn ();
			else
			_poolSpawn();
		}

	}
}
