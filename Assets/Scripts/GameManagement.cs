using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{

	public static bool InGame = false, InInstructions = false;

	public PlayerMechanics playerMechanics;
	public EnemyMechanics enemyMechanics;

	public GameUIManager gameUI;
	public GameObject instructionTiles;

	public Image fadeImage;
	private bool fadeIn = true;
	private float fadeSpeed = 5f;

	private Levels levels;

	private GameObject arenaGameObject, environment;
	private GameObject[,] arenaCells;
	private int[,] arenaCellData;

	public static int currentLevel = 0, currentPhase = 0;

	private float cameraZoomoutCooldown, ogCameraZoomoutCooldown = 1.5f;
	private bool cameraZooming = false;

	private void Awake()
	{
		levels = new Levels();
	}

	void InitGame()
	{
		InGame = true;
		InInstructions = false;
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
		LoadLevel(1, 5, 4, 5, levels.GetLevel(currentLevel)[currentPhase]);
	}

	void LoadLevel(int playerX, int playerY, int enemyX, int enemyY, List<int> enemyInstructions )
	{
		playerMechanics.gameObject.transform.position = arenaCells[playerY, playerX].transform.position;
		this.arenaCellData[playerY, playerX] = 2;
		playerMechanics.GetMonsterStats().SetPosition(playerX, playerY);
		
		enemyMechanics.gameObject.transform.position = arenaCells[enemyY, enemyX].transform.position;
		enemyMechanics.GetMonsterStats().SetPosition(enemyX, enemyY);
		this.arenaCellData[enemyY, enemyX] = 3;
		
		this.gameUI.SetPlayerHealth(this.playerMechanics.GetMonsterStats().Health);
		this.gameUI.SetEnemyHealth(this.enemyMechanics.GetMonsterStats().Health);
		
		this.gameUI.SetEnemyInstructions(enemyInstructions);
		
		this.GetComponent<Gameplay>().InitGame();
	}
	
	// Use this for initialization
	void Start () {
		fadeImage.color = new Color(0, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {

		if (cameraZooming)
		{
			this.cameraZoomoutCooldown -= Time.deltaTime;
			if (this.cameraZoomoutCooldown <= 0)
			{
				ZoomOutOfAttack();
				cameraZooming = false;
			}
		}
		
		if (fadeIn)
		{
			float newAlpha = Mathf.Lerp(fadeImage.color.a, 0, Time.deltaTime*fadeSpeed);
			fadeImage.color = new Color(0, 0, 0, newAlpha);
			if (fadeImage.color.a <= 0.01f)
			{
				Camera.main.GetComponent<CameraGameMovement>().MoveToGame();
				fadeIn = false;
				fadeImage.color = new Color(0, 0, 0, 0);
				InitGame();
			}
		}
	}
	
	public void ShowInstructionUI()
	{
//		this.instructionTiles.SetActive(true);
		this.gameUI.ShowInstructions();
	}

	public void HideInstructionUI()
	{
		this.gameUI.HideInstructions();
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

	public void OnGoButtonClick(List<int> instructions)
	{
		this.playerMechanics.SetPlayerInstructions(ParseInstructions(instructions));
		this.enemyMechanics.SetEnemyInstructions(ParseInstructions(levels.GetLevel(currentLevel)[currentPhase]));
		for (int i = 0; i < instructionTiles.transform.GetChild(1).childCount; i++)
		{
			Destroy(instructionTiles.transform.GetChild(1).GetChild(i).gameObject);
		}
//		this.instructionTiles.SetActive(false);
		this.gameUI.HideInstructions();
		InInstructions = false;
		this.GetComponent<Gameplay>().PerformInstructions();
	}

	public void ZoomIntoAttack()
	{
		this.cameraZooming = true;
		this.cameraZoomoutCooldown = this.ogCameraZoomoutCooldown;
		Camera.main.GetComponent<CameraGameMovement>().ZoomIntoAttack(playerMechanics.gameObject, enemyMechanics.gameObject);
	}
	
	public void ZoomOutOfAttack()
	{
		Camera.main.GetComponent<CameraGameMovement>().MoveToGame();
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
		gameUI.SetPlayerHealth(i);
	}
	
	public void SetEnemyHealth(int i)
	{
		gameUI.SetEnemyHealth(i);
	}

	public void SetEnemyInstructions(List<int> instructions)
	{
		gameUI.SetEnemyInstructions(instructions);
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
