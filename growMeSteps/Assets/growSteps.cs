using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growSteps : MonoBehaviour {

	public int numberOfSteps = 9;
	[Tooltip("True = steps will climb. False = steps will descend.")]
	public bool stepsGoUp = true;

	[Tooltip("True = steps will spiral. False = straight staircase.")]
	public bool spiral = false;
	public float spiralAmount = 10f;

	void Start () {

		int directionY = 1;

		if (stepsGoUp == false)
			directionY = -1;

		for (int i = 0; i < numberOfSteps; i++) {

			GameObject newStep = 
				GameObject.CreatePrimitive
			(PrimitiveType.Cube);

			newStep.transform.position =
			this.transform.position;

			newStep.transform.localScale =
			this.transform.lossyScale;

			if (spiral == true)
			newStep.transform.Rotate
				(Vector3.up * (i + spiralAmount));

			newStep.transform.Translate
		(Vector3.forward *
			this.transform.lossyScale.z * i);
		
			newStep.transform.Translate
		(Vector3.up *
				this.transform.lossyScale.y * i * directionY);
	
		}
		
		}
	

}
