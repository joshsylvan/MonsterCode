using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameMovement : MonoBehaviour {

	public Transform gameTarget;
	private Transform currentTarget;
	private GameObject zoomTarget;
	public float cameraSpeed = 1f, cameraRotationSpeed = 2.5f;
	bool zoom = false;

	private void Awake()
	{
		this.currentTarget = null;
		zoomTarget = new GameObject("ZoomTarget");
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
				Quaternion.Lerp(this.transform.rotation, currentTarget.rotation, Time.deltaTime * cameraSpeed);
			if (zoom)
			{
				if (Vector3.Distance(currentTarget.position, transform.position) < 0.1f)
				{
					zoom = false;
				}
			}
		}
	}

	public void MoveToGame()
	{
		currentTarget = gameTarget;
		cameraSpeed = 1f;
	}

	public void ZoomIntoAttack(GameObject player, GameObject enemy)
	{
		if (!zoom)
		{
			Vector3 target = (player.transform.position + enemy.transform.position) / 2 + new Vector3(0, 0, -5);
			zoomTarget.transform.position = target;			
			currentTarget = zoomTarget.transform;
			cameraSpeed = 10f;
			zoom = true;
		}
	}

	public bool IsZooming()
	{
		return zoom;
	}
}
