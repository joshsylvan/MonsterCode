using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{

	public static bool InGame = false, InInstructions = false;

	public GameObject playerObject, enemyObject;
	
	public PlayerMechanics playerMechanics;
	public EnemyMechanics enemyMechanics;

	public GameUIManager gameUI;
	public GameObject instructionTiles;

	public Image fadeImage;
	private bool fadeIn = false;
	private float fadeSpeed = 5f;

	private Levels levels;

	private GameObject arenaGameObject, environment;
	private GameObject[,] arenaCells;
	private int[,] arenaCellData;

	public static int currentLevel, currentPhase, playerMonster;
	private List<int> avaliableInstructions;

	private bool loadNextPhase = false, replayPhase = false, initPlayerDeath = false, initVictory = false,  victory = false;
	
	private float cameraZoomoutCooldown, ogCameraZoomoutCooldown = 1.5f;
	private bool cameraZooming = false;
	
	private void Awake()
	{
//		PlayerPrefs.SetInt("current_level", 0);
//		PlayerPrefs.SetInt("current_phase", 0);
//		PlayerPrefs.SetInt("player_health", 3);
//		PlayerPrefs.SetInt("enemy_health", 3);
//		PlayerPrefs.SetString("player_character", "Knight");
		
		
		if (SceneManager.GetActiveScene().name != "GameNewPhase")
		{
			fadeIn = true;
		}

		levels = new Levels();
		
		GameObject pO = Resources.Load("Prefab/Monsters/" + PlayerPrefs.GetString("player_character")) as GameObject;
		pO = Instantiate(pO);
		Vector3 scale = pO.transform.localScale;
		Vector3 pos = pO.transform.localPosition;
		pO.transform.SetParent(playerObject.transform);
		pO.transform.localPosition = pos;
		pO.transform.localScale = scale;

		Debug.Log(levels.GetLevel(currentLevel).GetEnemyName());
		GameObject eo = Resources.Load("Prefab/Enemies/" + levels.GetLevel(currentLevel).GetEnemyName()) as GameObject;
		eo = Instantiate(eo);
		Vector3 posenemy = eo.transform.localPosition;
		Debug.Log(posenemy);
		eo.transform.SetParent(enemyObject.transform);
		eo.transform.localPosition = posenemy;
		eo.transform.localScale = new Vector3(Mathf.Abs(eo.transform.localScale.x), eo.transform.localScale.y, eo.transform.localScale.z);
		
		currentLevel = PlayerPrefs.GetInt("current_level");
		currentPhase = PlayerPrefs.GetInt("current_phase");

		avaliableInstructions = levels.GetLevel(currentLevel).GetAvaliableTiles();

		gameUI.LoadAvaliableTiles(avaliableInstructions);

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
		LoadLevel(levels.GetLevel(currentLevel).GetPlayerPos()[0], 
			levels.GetLevel(currentLevel).GetPlayerPos()[1], 
			levels.GetLevel(currentLevel).GetEnemyPos()[0], 
			levels.GetLevel(currentLevel).GetEnemyPos()[1], 
			levels.GetLevel(currentLevel).GetPhases()[currentPhase]
		);
		if (SceneManager.GetActiveScene().name == "GameNewPhase")
		{
			fadeIn = true;
		}
	}

	void LoadLevel(int playerX, int playerY, int enemyX, int enemyY, List<int> enemyInstructions )
	{
		playerMechanics.gameObject.transform.position = arenaCells[playerY, playerX].transform.position;
		this.arenaCellData[playerY, playerX] = 2;
		playerMechanics.GetMonsterStats().SetPosition(playerX, playerY);
		playerMechanics.GetMonsterStats().Health = PlayerPrefs.GetInt("player_health");
		
		enemyMechanics.gameObject.transform.position = arenaCells[enemyY, enemyX].transform.position;
		enemyMechanics.GetMonsterStats().SetPosition(enemyX, enemyY);
		this.arenaCellData[enemyY, enemyX] = 3;
		enemyMechanics.GetMonsterStats().Health = levels.GetLevel(currentLevel).GetHeartsForPhase()[currentPhase];
		
		this.gameUI.SetPlayerHealth(PlayerPrefs.GetInt("player_health"));
		this.gameUI.SetEnemyHealth(enemyMechanics.GetMonsterStats().Health);
		
		this.gameUI.SetEnemyInstructions(enemyInstructions);
		
		this.GetComponent<Gameplay>().InitGame();	
	}

	public void LoadNextPhase()
	{
		loadNextPhase = true;
	}
	
	public void ReplayPhase()
	{
		replayPhase = true;
	}
	
	// Use this for initialization
	void Start () {
		fadeImage.color = new Color(0, 0, 0, 1);
		this.InitGame();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SceneManager.LoadScene("MenuNew");
		}
		
		if (cameraZooming)
		{
			this.cameraZoomoutCooldown -= Time.deltaTime;
			if (this.cameraZoomoutCooldown <= 0)
			{
				ZoomOutOfAttack();
				cameraZooming = false;
			}
		}
		
		if (fadeIn && !initPlayerDeath)
		{
			float newAlpha = Mathf.Lerp(fadeImage.color.a, 0, Time.deltaTime*fadeSpeed);
			fadeImage.color = new Color(0, 0, 0, newAlpha);
			if (fadeImage.color.a <= 0.01f)
			{
				Camera.main.GetComponent<CameraGameMovement>().MoveToGame();
				fadeIn = false;
				fadeImage.color = new Color(0, 0, 0, 0);
//				InitGame();
			}
		}
		if (playerMechanics.GetMonsterStats().IsDead())
		{
			if (!initPlayerDeath)
			{
				fadeImage.gameObject.SetActive(true);
				fadeImage.color = new Color(0, 0, 0, 0);
				initPlayerDeath = true;
			}
			StartCoroutine(StartFadeToBlack(false));
		} else if (victory)
		{
			if (!initVictory)
			{
				fadeImage.gameObject.SetActive(true);
				fadeImage.color = new Color(0, 0, 0, 0);
				initVictory = true;
			}
			StartCoroutine(FadeToBlack(true));
		}
		else
		{

			if (loadNextPhase && InInstructions && !gameUI.IsMessageDisplaying())
			{
				currentPhase++;
				if (currentPhase >= levels.GetLevel(currentLevel).GetPhases().Count)
				{
					currentPhase = 0;
					currentLevel++;
					PlayerPrefs.SetInt("player_health", 3);
					PlayerPrefs.SetInt("enemy_health", 3 + currentLevel);
					fadeIn = false;
					victory = true;
				}
				else
				{
					PlayerPrefs.SetInt("player_health", playerMechanics.GetMonsterStats().Health);
					PlayerPrefs.SetInt("enemy_health", enemyMechanics.GetMonsterStats().Health);
				}

				PlayerPrefs.SetInt("current_phase", currentPhase);
				PlayerPrefs.SetInt("current_level", currentLevel);
				if (!victory)
				{
					gameUI.ShowWellDone();
				}
			}

			if (!loadNextPhase && replayPhase && InInstructions && !gameUI.IsMessageDisplaying())
			{
				PlayerPrefs.SetInt("player_health", playerMechanics.GetMonsterStats().Health);
				gameUI.ShowTryAgain();
			}

			if (gameUI.IsMessageTimerFinished() && gameUI.IsMessageDisplaying())
			{
				SceneManager.LoadScene("GameNewPhase");
			}
		}
	}
	
	public void ShowInstructionUI()
	{
//		this.instructionTiles.SetActive(true);
		if (!replayPhase)
		{
			this.gameUI.ShowInstructions();
		}
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
		this.enemyMechanics.SetEnemyInstructions(ParseInstructions(levels.GetLevel(currentLevel).GetPhases()[currentPhase]));
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

	public bool IsNextPhaseReady()
	{
		return loadNextPhase;
	}

	IEnumerator StartFadeToBlack(bool victory)
	{
		yield return new WaitForSeconds(1.5f);
		StartCoroutine(FadeToBlack(victory));
	}
	
	IEnumerator FadeToBlack(bool victory)
	{
		float a = fadeImage.color.a;
		float newAlpha = Mathf.Lerp(a, 1, Time.deltaTime*5f);
		fadeImage.color = new Color(0, 0, 0, newAlpha);
		yield return new WaitForSeconds(1.5f);
		if (victory)
		{
			SceneManager.LoadScene("Victory");
		}
		else
		{
			SceneManager.LoadScene("Death");
		}
	}
	
}
