using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesRotating : MonoBehaviour {

	private Quaternion leftRotation, rightRotation;
	private float speed = 1.5f;
	private float deviation = 5f;
	private float minAngle = 0.5f;
	private bool rotatingLeft = false;
	
	// Use this for initialization
	void Start ()
	{
		leftRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y+deviation, transform.rotation.eulerAngles.z));
		rightRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y-deviation, transform.rotation.eulerAngles.z));
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (!rotatingLeft)
		{
			Quaternion newRot = Quaternion.RotateTowards(this.transform.rotation, rightRotation, Time.deltaTime*speed);
			transform.rotation = newRot;
			if (Quaternion.Angle(this.transform.rotation, rightRotation) <= minAngle)
			{
				rotatingLeft = true;
			}
		}
		else
		{
			Quaternion newRot = Quaternion.RotateTowards(this.transform.rotation, leftRotation, Time.deltaTime*speed);
			transform.rotation = newRot;
			if (Quaternion.Angle(this.transform.rotation, leftRotation) <= minAngle)
			{
				rotatingLeft = false;
			}
		}

	}
}
