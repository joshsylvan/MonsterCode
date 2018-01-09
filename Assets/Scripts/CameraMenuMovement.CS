using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuMovement : MonoBehaviour
{

	public Transform titleTarget, creatorTarget;
	private Transform currentTarget;
	public float cameraSpeed = 5f, cameraRotationSpeed = 2.5f;
	
	// Use this for initialization
	void Start ()
	{
		this.currentTarget = titleTarget;
	}
	
	// Update is called once per frame
	void Update () {
		
		this.transform.position = Vector3.Lerp(this.transform.position, currentTarget.position, Time.deltaTime*cameraSpeed);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, currentTarget.rotation, Time.deltaTime*cameraRotationSpeed);
		

	}

	public void MoveToTitle()
	{
		currentTarget = titleTarget;
	}

	public void MoveToCreator()
	{
		currentTarget = creatorTarget;
	}
	
	
}