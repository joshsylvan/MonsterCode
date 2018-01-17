using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFloating : MonoBehaviour
{
	private Vector3 upPosition, downPosition;
	private float speed = .25f;
	private float deviation = 0.5f;
	private bool movingUp = false;
	
	
	// Use this for initialization
	void Start () {
		upPosition = new Vector3(transform.position.x, transform.position.y+deviation, transform.position.z);
		downPosition = new Vector3(transform.position.x, transform.position.y-deviation, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!movingUp)
		{
			Vector3 newPos = Vector3.MoveTowards(transform.position, downPosition, Time.deltaTime*speed);
			transform.position = newPos;
			if (Vector3.Distance(downPosition, transform.position) <= 0.01f)
			{
				movingUp = true;
			}
		}
		else
		{
			
			Vector3 newPos = Vector3.MoveTowards(transform.position, upPosition, Time.deltaTime*speed);
			transform.position = newPos;
			if (Vector3.Distance(upPosition, transform.position) <= 0.01f)
			{
				movingUp = false;
			}
		}

	}
}
