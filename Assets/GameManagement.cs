using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{

	public PlayerMechanics playerMechanics;
	public EnemyMechanics enemyMechanics;

	public GameObject gameUI;
	public GameObject instructionTiles;

	public Image fadeImage;
	private bool fadeIn = true;
	private float fadeSpeed = 5f;


	private GameObject arenaGameObject, environment;
	private GameObject[,] arenaCells;
	private int[,] arenaCellData;

	void InitGame()
	{
		this.environment = GameObject.Find("Environment");
		this.arenaGameObject = environment.transform.GetChild(0).gameObject;
		this.arenaCells = new GameObject[6,6];
		this.arenaCellData = new int[6,6];
		
		for (int i = 0; i < this.arenaCells.GetLength(0); i++)
		{
			for (int j = 0; j < this.arenaCells.GetLength(1); j++)
			{
				this.arenaCells[i, j] = this.arenaGameObject.transform.GetChild(i).GetChild(j).gameObject;
				if (this.arenaCells[i, j].transform.childCount > 0)
				{
					arenaCellData[i, j] = 1;
				}
				else
				{
					arenaCellData[i, j] = 0;
				}
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		fadeImage.color = new Color(0, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn)
		{
			float newAlpha = Mathf.Lerp(fadeImage.color.a, 0, Time.deltaTime*fadeSpeed);
			fadeImage.color = new Color(0, 0, 0, newAlpha);
			if (fadeImage.color.a <= 0.01f)
			{
				Camera.main.GetComponent<CameraGameMovement>().MoveToGame();
				fadeIn = false;
			}
		}
	}
	
	LinkedList<int> ParseInstructions(List<int> instructions)
	{
		bool skip = false;
		LinkedList<int> instructionQueue = new LinkedList<int>();
		for (int i = 0; i < instructions.Count; i++)
		{
			if (!skip)
			{
				switch (instructions[i])
				{
					case 0: //Defence
						instructionQueue.AddLast(6);
						break;
					case 1: //Fight
						instructionQueue.AddLast(5);
						break;
					case 2: // jump
						instructionQueue.AddLast(2);
						if (i + 1 < instructions.Count && instructions[i + 1] == 3)
						{
							instructionQueue.AddLast(3);
							skip = true;
						}
						else if (i + 1 < instructions.Count && instructions[i + 1] == 4)
						{
							instructionQueue.AddLast(4);
							skip = true;
						}

						instructionQueue.AddLast(1);
						break;
					case 3: //left
						instructionQueue.AddLast(3);
						break;
					case 4: //right
						instructionQueue.AddLast(4);
						break;
					case 5: // special
						break;
					default:
						break;
				}
			}
			else
			{
				skip = false;
			}
		}
		return instructionQueue;
	}
	
	public bool InBounds(int x, int y)
	{
		return x >= 0 && x < arenaCells.GetLength(1) && y >= 0 && y < arenaCells.GetLength(0);
	}

	public int[,] GetArenaCellData()
	{
		return this.arenaCellData;
	}
	
	public GameObject[,] GetArenaCellsObjet()
	{
		return this.arenaCells;
	}

	public void SetPlayerHealth(int i)
	{
		
	}
	
	public void SetEnemyHealth(int i)
	{
		
	}

	public PlayerMechanics GetPlayerMechanics()
	{
		return playerMechanics;
	}

	public EnemyMechanics GetEnemyMechanics()
	{
		return enemyMechanics;
	}
}
