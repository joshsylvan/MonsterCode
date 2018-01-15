using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePreviewManager : MonoBehaviour
{
	private TileManagement tm;

	// Use this for initialization
	void Start ()
	{
		this.tm = transform.parent.GetComponent<TileManagement>();

		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if( hit.collider != null && hit.collider.CompareTag("TilePreview"))
			{
				GameObject tile = Resources.Load("Prefab/Tiles/" + GetTileName(hit.collider.name)) as GameObject;
				tile = Instantiate(tile);
				tile.SetActive(false);
				tile.transform.parent = this.transform.parent.GetChild(1);
				tile.transform.localScale = Vector3.one;
				Vector2 newPos = new Vector2(
					Camera.main.ScreenToWorldPoint(Input.mousePosition).x - (1.6f*tile.transform.parent.parent.localScale.x), 
					Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				tile.transform.position = newPos;
				tile.SetActive(true);
				tm.SetSelectedTile(tile);
			}
		} 
	}

	private string GetTileName(string name)
	{
		switch (name)
		{
			case "DefencePreview":
				return "DefendTile";
			case "FightPreview":
				return "FightTile";
			case "LeftPreview":
				return "LeftTile";
			case "RightPreview":
				return "RightTile";
			case "JumpPreview":
				return "JumpTile";
			case "SpecialPreview":
				return "SpecialTile";
			default:
				return "DefendTile";
		}
	}
}