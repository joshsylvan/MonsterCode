using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManagement : MonoBehaviour {

	private GameObject rootNode;
	private GameObject selectedTile;
	private Vector2 tileOffset;
	private float tileSizeOffset = 3.3f;

	private List<GameObject> connectedTiles;
	
	// Use this for initialization
	void Start ()
	{
		rootNode = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if( hit.collider != null && hit.collider.CompareTag("Tile") && hit.collider.name != "RootNode")
			{
				if (hit.collider.GetComponent<TileStats>().GetRightTile() == null)
				{
					selectedTile = hit.collider.gameObject;
					tileOffset = (Vector2) (selectedTile.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
				}
				else 
				{
					selectedTile = hit.collider.gameObject;
					tileOffset = (Vector2) (selectedTile.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
					connectedTiles = GetConnectedTiles(hit.collider.GetComponent<TileStats>());
				} 
			}
		} 
		else if (Input.GetMouseButton(0))
		{
			if (selectedTile != null)
			{
				Vector2 newPos = new Vector2(
					Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
					Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
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
		return GetInstructions(rootNode.GetComponent<TileStats>().GetRightTile().GetComponent<TileStats>());
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
}
