﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{

	public GameObject playerHealth, enemyHealth, tileObject;
	public GameObject countDown;
	public GameObject[] monsterTiles;

	private float rootNodeX = -17, rootNodeY = -4.8f;
	private float tilePreviewX = 0, tilePreviewY = -8.5f;
	private float deleteTileX = 15.4f, deleteTileY = 0.2f;
	private float monsterInstructionsX = 0.24f, monsterInstructionsY = 8.8f;
	private float goButtonX = 8.18f, goButtonY = 0.8f;

	private float tilesZPosition = -20f;

	private GameObject rootNode, tilePreview, deleteTile, monsterInstructions, goButton, backGround;
	private float lerpSpeed = 10;
	private bool showInstructions = false;

	private void Awake()
	{
//		this.rootNode = tileObject.transform.GetChild(0).gameObject;
//		this.tilePreview = tileObject.transform.GetChild(2).gameObject;
//		this.deleteTile = tileObject.transform.GetChild(3).gameObject;
//		this.monsterInstructions = tileObject.transform.GetChild(4).gameObject;
//		this.goButton = tileObject.transform.GetChild(5).gameObject;
		this.backGround = tileObject.transform.GetChild(6).gameObject;
		
		monsterTiles = Resources.LoadAll<GameObject>("Prefab/MonsterTiles");
		
		this.playerHealth = transform.GetChild(0).gameObject;
		this.enemyHealth = transform.GetChild(1).gameObject;
		this.countDown = transform.GetChild(2).gameObject;
		playerHealth.SetActive(true);
		enemyHealth.SetActive(true);
		countDown.SetActive(false);
		
		
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		if (showInstructions)
		{
			Color bColor = this.backGround.GetComponent<SpriteRenderer>().color;
			float a = this.backGround.GetComponent<SpriteRenderer>().color.a;
			Debug.Log("SHOW: " + a);
			bColor.a = Mathf.Lerp(bColor.a, 0.75f, Time.deltaTime * lerpSpeed);
			this.backGround.GetComponent<SpriteRenderer>().color = bColor;

			this.tileObject.transform.position = Vector3.Lerp(
				tileObject.transform.position,
				new Vector3(
					tileObject.transform.position.x,
					tileObject.transform.position.y,
					0
				),
				Time.deltaTime * lerpSpeed
			);
		}
		else
		{
			Color bColor = this.backGround.GetComponent<SpriteRenderer>().color;
			float a = this.backGround.GetComponent<SpriteRenderer>().color.a;
			Debug.Log("HIDE: " + a);
			bColor.a = Mathf.Lerp(bColor.a, 0f, Time.deltaTime * lerpSpeed);
			this.backGround.GetComponent<SpriteRenderer>().color = bColor;
			this.tileObject.transform.position = Vector3.Lerp(
				tileObject.transform.position,
				new Vector3(
					tileObject.transform.position.x,
					tileObject.transform.position.y,
					tilesZPosition
				),
				Time.deltaTime * lerpSpeed
			);
		}
	}

	public void ShowInstructions()
	{
		showInstructions = true;
	}

	public void HideInstructions()
	{
		showInstructions = false;
	}
	
	public void SetEnemyHealth(int health)
	{
		if (health <= enemyHealth.transform.childCount)
		{
			int remainngHealth = health;
			for (int i = enemyHealth.transform.childCount - 1; i >= 0; i--)
			{
				if (remainngHealth > 0)
				{
					enemyHealth.transform.GetChild(i).gameObject.SetActive(true);
					remainngHealth--;
				}
				else
				{
					enemyHealth.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}
	
	public void SetPlayerHealth(int health)
	{
		if (health <= playerHealth.transform.childCount)
		{
			int remainngHealth = health;
			for (int i = 0; i < playerHealth.transform.childCount; i++)
			{
				if (remainngHealth > 0)
				{
					playerHealth.transform.GetChild(i).gameObject.SetActive(true);
					remainngHealth--;
				}
				else
				{
					playerHealth.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}

	public void SetEnemyInstructions(List<int> instructions)
	{
		for (int i = 0; i < instructions.Count; i++)
		{
			GameObject monsterTile = Instantiate(monsterTiles[instructions[i]]);
			monsterTile.transform.SetParent(tileObject.transform.GetChild(4).GetChild(0));
			monsterTile.transform.position = new Vector3(0, 0 ,0);
			monsterTile.transform.localPosition = new Vector3(-3+i, 0 ,0);
			monsterTile.transform.localScale = new Vector3(0.25f, 0.25f , 0.25f);
		}
		tileObject.transform.GetChild(4).GetChild(0).localPosition = new Vector3( ((float) instructions.Count)/2f, 0, 0);
	
	}
}
