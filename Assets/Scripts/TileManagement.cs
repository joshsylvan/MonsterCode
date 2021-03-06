﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class TileManagement : MonoBehaviour
{
	private GameObject tilePreviews;
	
	private GameObject rootNode;
	private GameObject selectedTile;
	private Vector2 tileOffset;
	private float tileSizeOffset = 3.3f;

	private List<GameObject> connectedTiles;
	
	// Use this for initialization
	void Start ()
	{
		tilePreviews = transform.GetChild(2).gameObject;
		rootNode = transform.GetChild(0).gameObject;
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
				tile.transform.parent = tilePreviews.transform.parent.GetChild(1);
				tile.transform.localScale = Vector3.one;
				Vector2 newPos = new Vector2(
					point.x - (1.6f*tile.transform.parent.parent.localScale.x), 
					point.y);
				tile.transform.position = newPos;
				tile.SetActive(true);
				SetSelectedTile(tile);
				tileOffset = (Vector2) (selectedTile.transform.position - new Vector3(point.x, point.y, 0));
			} 
			else if( hit.collider != null && hit.collider.CompareTag("Tile") && hit.collider.name != "RootNode")
			{
				if (hit.collider.GetComponent<TileStats>().GetRightTile() == null)
				{
					selectedTile = hit.collider.gameObject;
					tileOffset = (Vector2) (selectedTile.transform.position - new Vector3(point.x, point.y, 0));
				}
				else 
				{
					selectedTile = hit.collider.gameObject;
					tileOffset = (Vector2) (selectedTile.transform.position - new Vector3(point.x, point.y, 0));
					connectedTiles = GetConnectedTiles(hit.collider.GetComponent<TileStats>());
				} 
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (selectedTile != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float z_plane_of_2d_game = 0;
				Vector3 pos_at_z_0 = ray.origin + ray.direction * (z_plane_of_2d_game - ray.origin.z) / ray.direction.z;
				Vector2 point = new Vector2(pos_at_z_0.x, pos_at_z_0.y);
				Vector2 newPos = point;
//				tileOffset = (Vector2) (selectedTile.transform.position - new Vector3(point.x, point.y, 0));
				if (connectedTiles == null)
				{
					selectedTile.transform.position = newPos + tileOffset;	
				}
				else
				{
					for (int i = 0; i < connectedTiles.Count; i++)
					{
						Vector2 originOffset = newPos + tileOffset;
						connectedTiles[i].transform.position = new Vector2(
							originOffset.x + ((connectedTiles[i].GetComponent<SpriteRenderer>().size.x * transform.localScale.x)*i), 
							originOffset.y);
					}
				}
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			selectedTile = null;
			connectedTiles = null;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float z_plane_of_2d_game = 0;
			Vector3 pos_at_z_0 = ray.origin + ray.direction * (z_plane_of_2d_game - ray.origin.z) / ray.direction.z;
 			Vector2 point = new Vector2(pos_at_z_0.x,pos_at_z_0.y);
			RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
//			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if( hit.collider != null && hit.collider.name == "GoButton")
			{
//				GameObject.Find("GameManager").GetComponent<GameManager>().OnGoButtonClick(GetInstructionChain());
				GameObject.Find("GameManager").GetComponent<GameManagement>().OnGoButtonClick(GetInstructionChain());
			}
			
		}
	}
	
	private List<GameObject> GetConnectedTiles(TileStats tile)
	{
		List<GameObject> tiles = new List<GameObject>();
		tiles.Add(tile.gameObject);
		if (tile.GetRightTile() != null)
		{
			return tiles.Concat(GetConnectedTiles(tile.GetRightTile().GetComponent<TileStats>())).ToList();
		}
		else
		{
			return tiles;
		}

	}
	
	public List<int> GetInstructionChain()
	{
		if (rootNode.GetComponent<TileStats>().GetRightTile() != null)
		{
			List<int> instructions =
				GetInstructions(rootNode.GetComponent<TileStats>().GetRightTile().GetComponent<TileStats>());
			return instructions;
		}
		else
		{
			return new List<int>(){-1};
		}
	}

	private List<int> GetInstructions(TileStats stats)
	{
		List<int> instructions = new List<int>();
		instructions.Add(stats.InstructionID);
		if (stats.GetRightTile() != null)
		{
			return instructions.Concat(GetInstructions(stats.GetRightTile().GetComponent<TileStats>())).ToList();
		}
		else
		{
			return instructions;
		}
	}

	public void SetSelectedTile(GameObject selectedTile)
	{
		this.selectedTile = selectedTile;
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
