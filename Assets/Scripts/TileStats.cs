using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStats : MonoBehaviour
{
	private GameObject _leftTile, _rightTile;
	private bool isNodeTile;
	public int InstructionID;
	
	// Use this for initialization
	void Start () {
		if (this.gameObject.name == "RootNode")
		{
			isNodeTile = true;
		}
		else
		{
			isNodeTile = false;
		}
	}
		
	// Update is called once per frame
	void Update () {
			
	}
	
	public void SetLeftTile(GameObject leftTile)
	{
		this._leftTile = leftTile;
	}
	
	public void SetRightTile(GameObject rightTile)
	{
		this._rightTile = rightTile;
	}
	
	public GameObject GetLeftTile()
	{
		return this._leftTile;
	}
		
	public GameObject GetRightTile()
	{
		return this._rightTile;
	}
		
	public bool IsNodeTile
	{
		get { return isNodeTile; }
	}
}