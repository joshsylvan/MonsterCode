using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameMovement : MonoBehaviour {

	public Transform gameTarget;
	private Transform currentTarget;
	public float cameraSpeed = 5f, cameraRotationSpeed = 2.5f;

	private void Awake()
	{
		this.currentTarget = null;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (currentTarget != null)
		{
			this.transform.position =
				Vector3.Lerp(this.transform.position, currentTarget.position, Time.deltaTime * cameraSpeed);
			this.transform.rotation =
				Quaternion.Lerp(this.transform.rotation, currentTarget.rotation, Time.deltaTime * cameraRotationSpeed);
		}
	}

	public void MoveToGame()
	{
		currentTarget = gameTarget;
		cameraSpeed = 1f;
	}
}
