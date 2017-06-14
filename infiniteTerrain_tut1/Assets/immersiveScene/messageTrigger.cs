using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class messageTrigger : MonoBehaviour {

	public string messageText = "Welcome to the Hidden Corridor.";

	public bool triggered = false;
	public bool canTriggerRepeat = false;

	public float secondsToShow = 10f;	// Timer for showing message.
	private float triggeredTime = 0;	

	public Canvas messageBoard;			// Enable this to show message.
	private Text theMessage;			// Assign to this to write new message.

	private AudioSource myMouth = null;

	private GameObject subject = null;

	void Start () {

		if (this.GetComponent<AudioSource> () != null)
			myMouth = this.GetComponent<AudioSource> ();

		subject = GameObject.Find ("Subject");

	}

	void setMessage(){

		theMessage = messageBoard.GetComponent<Text> ();
		theMessage.text = messageText.Replace("/n", "\n");
	}

	void OnTriggerEnter(Collider hitter){

		if (hitter.gameObject != subject)
			return;

		// Only trigger this behaviour once....?
		if (triggered)
			return;

		// Register that we have now triggered this message.
		triggered = true;
		triggeredTime = Time.unscaledTime;

		// Sound effect...
		if (myMouth != null) myMouth.Play();

		// Display message.
		setMessage();
		messageBoard.GetComponent<Canvas>().enabled = true;


	}

	void Update () {
		if (triggered &&  
		    Time.unscaledTime - triggeredTime >= secondsToShow) {

			// Hide message.
			messageBoard.GetComponent<Canvas>().enabled = false;

			if (canTriggerRepeat)
				triggered = false;
			else
				GameObject.Destroy (this, 0.1f);
			
			triggeredTime = 0;	// Redundant line?
		}

	}
}
