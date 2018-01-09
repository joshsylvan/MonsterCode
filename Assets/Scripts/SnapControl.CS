using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapControl : MonoBehaviour
{
	private GameObject attachedTile;
	private GameObject snapShadow;
	private TileStats tileStats;
	
	private Vector2 goalPosition;
	private float lerpSpeed = 10f;
	private bool isMoving = false;
	private GameObject tileToLerp;
	private List<GameObject> tilesToLerp;
	
	// Use this for initialization
	void Start ()
	{
		this.snapShadow = this.transform.GetChild(0).gameObject;
		this.tileStats = this.transform.parent.GetComponent<TileStats>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0) && this.attachedTile != null)
		{
//			this.attachedTile.transform.position = this.transform.position;
			
			
			this.snapShadow.SetActive(false);
			this.tileStats.SetRightTile(this.attachedTile);
			this.attachedTile.GetComponent<TileStats>().SetLeftTile(this.transform.parent.gameObject);
			this.isMoving = true;
			this.tileToLerp = this.attachedTile;
			this.tilesToLerp = GetConnectedTiles(this.tileToLerp.GetComponent<TileStats>());
//			this.AttachChildTiles(this.attachedTile);
		}
		
//
		if (this.isMoving)
		{
			if (tilesToLerp.Count <= 1)
			{
				Vector2 newPos = Vector2.Lerp(this.tileToLerp.transform.position, this.transform.position,
					Time.deltaTime * this.lerpSpeed);
				this.tileToLerp.transform.position = newPos;
				if (Vector2.Distance(newPos, this.transform.position) <= 0.001f)
				{
					this.isMoving = false;
					this.tileToLerp = null;
				}
			}
			else
			{
				float highestDistance = 0f;
				for (int i = 0; i < tilesToLerp.Count; i++)
				{
					if (i == 0)
					{
						Vector2 newPos = Vector2.Lerp(tilesToLerp[i].transform.position, this.transform.position,
							Time.deltaTime * this.lerpSpeed);
						this.tilesToLerp[i].transform.position = newPos;
						if(highestDistance <= Vector2.Distance(newPos, this.transform.position))
						{
							highestDistance = Vector2.Distance(newPos, this.transform.position);
						}
					}
					else
					{
						Vector2 newPos = Vector2.Lerp(tilesToLerp[i].transform.position, tilesToLerp[i-1].transform.GetChild(0).position,
							Time.deltaTime * this.lerpSpeed);
						this.tilesToLerp[i].transform.position = newPos;
						if(highestDistance <= Vector2.Distance(newPos, tilesToLerp[i-1].transform.GetChild(0).position))
						{
							highestDistance = Vector2.Distance(newPos, tilesToLerp[i-1].transform.GetChild(0).position);
						}
					}

					if (highestDistance <= 0.00001f)
					{
						this.isMoving = false;
						this.tileToLerp = null;
					}
				}
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
	
	void AttachChildTiles(GameObject tile)
	{
		if (tile.GetComponent<TileStats>().GetRightTile() != null)
		{
			GameObject rightTile = tile.GetComponent<TileStats>().GetRightTile();
			rightTile.transform.position = tile.transform.GetChild(0).transform.position;
			tile.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
			this.AttachChildTiles(rightTile);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Tile") && this.attachedTile == null && col.name != "RootNode" && col.GetComponent<TileStats>().GetLeftTile() == null)
		{
			if (col.transform.GetChild(0).GetChild(0).gameObject.activeSelf)
			{
				Debug.Log("CANT DO THAT!");
			}
			else
			{
				this.snapShadow.SetActive(true);
				this.attachedTile = col.gameObject;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (col.CompareTag("Tile"))
		{
			this.snapShadow.SetActive(false);
			if (col.gameObject == this.attachedTile)
			{
				this.tileStats.SetRightTile(null);
				this.attachedTile.GetComponent<TileStats>().SetLeftTile(null);
				this.attachedTile = null;
			}
		}
	}

	public GameObject GetAttachedTile()
	{
		return this.attachedTile;
	}

}