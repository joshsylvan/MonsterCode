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
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float z_plane_of_2d_game = 0;
			Vector3 pos_at_z_0 = ray.origin + ray.direction * (z_plane_of_2d_game - ray.origin.z) / ray.direction.z;
			Vector2 point = new Vector2(pos_at_z_0.x,pos_at_z_0.y);
			RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
			
			if( hit.collider != null && hit.collider.CompareTag("TilePreview"))
			{
				GameObject tile = Resources.Load("Prefab/Tiles/" + GetTileName(hit.collider.name)) as GameObject;
				tile = Instantiate(tile);
				tile.SetActive(false);
				tile.transform.parent = this.transform.parent.GetChild(1);
				tile.transform.localScale = Vector3.one;
				Vector3 pos_at_z_0_2 = ray.origin + ray.direction * (z_plane_of_2d_game - ray.origin.z) / ray.direction.z;
				Vector2 point2 = new Vector2(pos_at_z_0.x,pos_at_z_0.y);
				Vector2 newPos = new Vector2(
					point2.x - (1.6f*tile.transform.parent.parent.localScale.x), 
					point2.y);
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