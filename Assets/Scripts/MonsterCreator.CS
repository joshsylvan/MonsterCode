using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCreator : MonoBehaviour {

    //Head
	public GameObject headPreview, bodyPreview, legsPreview;
	private int headIndex = 0, bodyIndex = 0, legsIndex = 0;

	private Sprite[] characterSprites;
	
	// Use this for initialization
	void Start ()
	{
		characterSprites = Resources.LoadAll<Sprite>("Sprites/TestMonsters");
		UpdateSpritePreview(0);
		UpdateSpritePreview(1);
		UpdateSpritePreview(2);
	}
	
	// Update is called once per frame
	private void Update () {
		
	}

	public void OnRightArrowClick(int partIndex)
	{
		switch (partIndex)
		{
			case 0:
				headIndex = ShiftIndexRight(headIndex);
				UpdateSpritePreview(0);
				break;
			case 1:
				bodyIndex = ShiftIndexRight(bodyIndex);
				UpdateSpritePreview(1);
				break;
			case 2:
				legsIndex = ShiftIndexRight(legsIndex);
				UpdateSpritePreview(2);
				break;
			default:
				Debug.Log("ERROR: Not a valid body index!");
				break;
		}
	}
	
	public void OnLeftArrowClick(int partIndex)
	{
		switch (partIndex)
		{
			case 0:
				headIndex = ShiftIndexLeft(headIndex);
				UpdateSpritePreview(0);
				break;
			case 1:
				bodyIndex = ShiftIndexLeft(bodyIndex);
				UpdateSpritePreview(1);
				break;
			case 2:
				UpdateSpritePreview(2);
				legsIndex = ShiftIndexLeft(legsIndex);
				break;
			default:
				Debug.Log("ERROR: Not a valid body index!");
				break;
		}
	}

	private void UpdateSpritePreview(int partIndex)
	{
		switch (partIndex)
		{
			case 0:
				headPreview.GetComponent<Image>().sprite = characterSprites[headIndex+4];
				break;
			case 1:
				bodyPreview.GetComponent<Image>().sprite = characterSprites[bodyIndex];
				break;
			case 2:
				legsPreview.GetComponent<Image>().sprite = characterSprites[legsIndex+8];
				break;
			default:
				Debug.Log("ERROR: Part index not valid to udpate sprite");
				break;
		}
	}

	private int ShiftIndexRight(int indexID)
	{
		indexID++;
		if (indexID > 3)
		{
			indexID = 0;
		}
		return indexID;
	}
	
	private int ShiftIndexLeft(int indexID)
	{
		indexID--;
		if (indexID < 0)
		{
			indexID = 3;
		}
		return indexID;
	}

	public int GetHeadIndex()
	{
		return headIndex;
	}

	public int GetBodyIndex()
	{
		return bodyIndex;
	}

	public int GetLegsIndex()
	{
		return legsIndex;
	}
	
}