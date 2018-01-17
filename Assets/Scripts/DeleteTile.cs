using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTile : MonoBehaviour
{
	private string hoverColour = "FFB4B4FF";
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Tile") && Input.GetMouseButtonUp(0))
		{
//			this.GetComponent<SpriteRenderer>().color = new Color(255, 180, 180);
//			this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
		}
	}

	private void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Tile") && Input.GetMouseButtonUp(0))
		{
			Destroy(col.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
//		this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
	}
}
