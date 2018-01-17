using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static bool inGame = false; 
	public static bool inInstructions = false;
	
	private GameplayMechanics gameplayMechanics;
	public GameObject tileObject, menuObject, gameplayObject;
	public GameUIManager gameUI;
	private CameraMenuMovement camera;
	
	private Levels levels;
	int currentLevel = 0, currentLevelphase = 0;

	private float menuCooldown = 1f, ogMenuCooldown = 1f;

	private void Awake()
	{
		camera = Camera.main.GetComponent<CameraMenuMovement>();
		this.gameplayMechanics = GetComponent<GameplayMechanics>();
		levels = new Levels();
		gameplayObject.SetActive(false);
		menuObject.SetActive(true);
//		StartGame();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (inGame && menuCooldown > 0)
		{
			menuCooldown -= Time.deltaTime;
			if (menuCooldown <= 0)
			{
				menuObject.SetActive(false);
			}
		}
	}

	public void StartGame()
	{
		inGame = true;
		camera.MoveToGame();
		gameplayObject.SetActive(true);
		menuCooldown = ogMenuCooldown;
		gameplayMechanics.InitGame();
		gameplayMechanics.LoadLevel(1, 5, 4, 5);
	}

	public void OnGoButtonClick(List<int> playerInstructions)
	{
		this.gameplayMechanics.SetPlayerInstructions(ParseInstructions(playerInstructions));
		this.gameplayMechanics.SetEnemyInstructions(ParseInstructions(levels.GetLevel(currentLevel)[currentLevelphase]));
		this.tileObject.SetActive(false);
		inInstructions = false;
		this.gameplayMechanics.PerformInstructions();
	}

	Queue<int> ParseInstructions(List<int> instructions)
	{
		Queue<int> instructionQueue = new Queue<int>();
		for (int i = 0; i < instructions.Count; i++)
		{
			switch (instructions[i])
			{
				case 0: //Defence
					break;
				case 1: //Fight
					break;
				case 2: // jump
					instructionQueue.Enqueue(2);
					if (i+1 < instructions.Count && instructions[i + 1] == 3)
					{
						instructionQueue.Enqueue(3);
					} 
					else if (i+1 < instructions.Count && instructions[i + 1] == 4)
					{
						instructionQueue.Enqueue(4);
					}
					instructionQueue.Enqueue(1);
					break;
				case 3: //left
					instructionQueue.Enqueue(3);
					break;
				case 4: //right
					instructionQueue.Enqueue(4);
					break;
				case 5: // special
					break;
				default:
					break;
			}
		}
		return instructionQueue;
	}
}
