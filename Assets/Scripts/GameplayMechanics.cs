using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMechanics : MonoBehaviour
{
	private GameObject playerObject, enemyObject, arenaGameObject, environment, instructionsGameObject;
	private PlayerMechanics playerStats;
	private EnemyMechanics enemyStats;
	private GameUIManager gameUI;

	private GameObject[,] arenaCells;
	private int[,] arenaCellData;

	private bool performInstructions = false;
	
	private Vector2 playerNewPosition = Vector2.zero;
	private bool isPlayerLerping = false, isPlayerJumping = false;
	private float movementSpeed = 5f;
	private float minimumDistance = 0.1f;
	private Queue<int> playerInstructions;
	
	private Vector2 enemyNewPosition = Vector2.zero;
	private bool isEnemyMoving = false;
	private Queue<int> enemyInstructions;

	private List<List<int>> enemyLevelInstructions;

	private float coolDownTimer = 3f, OGCoolDownTimer = 7f;
	private bool hasCoolDownStarted = false;

	private void Awake()
	{
		playerInstructions = new Queue<int>();
		enemyInstructions = new Queue<int>();
	}

	// Use this for initialization
	void Start ()
	{
		
	}

	public void InitGame()
	{
		this.gameUI = GameObject.Find("GameUI").GetComponent<GameUIManager>();
		this.environment = GameObject.Find("Environment");
		this.playerObject = this.environment.transform.GetChild(1).GetChild(0).gameObject;
		this.enemyObject = this.environment.transform.GetChild(1).GetChild(1).gameObject;
		this.playerStats = playerObject.GetComponent<PlayerMechanics>();
		this.enemyStats = enemyObject.GetComponent<EnemyMechanics>();
		this.instructionsGameObject = GameObject.Find("Tiles");
		this.instructionsGameObject.SetActive(false);
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

		StartCoolDownTimer(10);
	}

	public void LoadLevel(int playerX, int playerY, int enemyX, int enemyY)
	{
		this.playerObject.transform.position = arenaCells[playerY, playerX].transform.position;
		this.arenaCellData[playerY, playerX] = 1;
		this.playerStats.GetMonsterStats().SetPosition(playerX, playerY);
		
		this.enemyObject.transform.position = arenaCells[enemyY, enemyX].transform.position;
		this.enemyStats.GetMonsterStats().SetPosition(enemyX, enemyY);
		this.arenaCellData[enemyY, enemyX] = 1;
		
		this.gameUI.SetPlayerHealth(this.playerStats.GetMonsterStats().Health);
		this.gameUI.SetEnemyHealth(this.enemyStats.GetMonsterStats().Health);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.inGame)
		{
			if (!GameManager.inInstructions)
			{
				if (this.IsCoolDownOver() && !performInstructions)
				{
					GameManager.inInstructions = true;
				}
				//Move Player
				if (performInstructions && IsEnemyReady() && IsPlayerReady())
				{
					PlayGame();
				}

				if (isPlayerLerping)
				{
					Vector2 newPos = Vector2.Lerp(playerObject.transform.position, playerNewPosition, Time.deltaTime * movementSpeed);
					playerObject.transform.position = newPos;
					if (Vector2.Distance(playerObject.transform.position, playerNewPosition) < minimumDistance)
					{
						isPlayerLerping = false;
					}
				}

				//Move Enemy
				if (isEnemyMoving)
				{
					Vector2 newPos = Vector2.Lerp(enemyObject.transform.position, enemyNewPosition, Time.deltaTime * movementSpeed);
					enemyObject.transform.position = newPos;
					if (Vector2.Distance(enemyObject.transform.position, enemyNewPosition) < minimumDistance)
					{
						isEnemyMoving = false;
					}
				}
			}
			else
			{
				instructionsGameObject.SetActive(true);
			}
		}
	}

	private void StartCoolDownTimer(float time)
	{
		if (!hasCoolDownStarted)
		{
			this.coolDownTimer = time;
			this.hasCoolDownStarted = true;
		}
	}

	private bool IsCoolDownOver()
	{
		if (this.coolDownTimer > 0)
		{
			this.coolDownTimer -= Time.deltaTime;
			return false;
		}
		else
		{
			this.hasCoolDownStarted = false;
			return true;
		}
	}

	private void PlayGame()
	{
		if (playerInstructions.Count > 0 || enemyInstructions.Count > 0)
		{
			if (playerInstructions.Count > 0)
			{
				switch (playerInstructions.Dequeue())
				{
					case 4:
						MovePlayerRight();
						break;
					case 3:
						MovePlayerLeft();
						break;
					case 2:
						MovePlayerUp();
						break;
					case 1:
						MovePlayerDown();
						break;
				}
			}

			if (enemyInstructions.Count > 0)
			{
				switch (enemyInstructions.Dequeue())
				{
					case 4:
						MoveEnemyRight();
						break;
					case 3:
						MoveEnemyLeft();
						break;
					case 2:
						MoveEnemyUp();
						break;
					case 1:
						MoveEnemyDown();
						break;
				}
			}
		}
		else
		{
			StartCoolDownTimer(3);
			performInstructions = false;
		}
	}

	private bool InBounds(int x, int y)
	{
		return x >= 0 && x < arenaCells.GetLength(1) && y >= 0 && y < arenaCells.GetLength(0);
	}

	private void LerpPlayerTo(int x, int y)
	{
		this.isPlayerLerping = true;
		this.playerNewPosition = arenaCells[y, x].transform.position;
		this.playerStats.GetMonsterStats().SetPosition(x, y);
	}

	public void MovePlayerLeft()
	{
		if (InBounds(playerStats.GetMonsterStats().GetXPosition()-1, playerStats.GetMonsterStats().GetYPosition()))
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition()-1, playerStats.GetMonsterStats().GetYPosition());
		}
	}
	
	public void MovePlayerRight()
	{
		if (InBounds(playerStats.GetMonsterStats().GetXPosition()+1, playerStats.GetMonsterStats().GetYPosition()))
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition()+1, playerStats.GetMonsterStats().GetYPosition());
		}
	}

	public void MovePlayerDown()
	{
		if(InBounds(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()+1))
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()+1);
		}
	}
	
	public void MovePlayerUp()
	{
		if(InBounds(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()-1))
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()-1);
		}
	}

	public bool IsPlayerReady()
	{
		return !isPlayerLerping;
	}

	public void SetPlayerInstructions(Queue<int> playerInstructions)
	{
		this.playerInstructions = playerInstructions;
	}
	
	private void LerpEnemyTo(int x, int y)
	{
		this.isEnemyMoving = true;
		this.enemyNewPosition = arenaCells[y, x].transform.position;
		this.enemyStats.GetMonsterStats().SetPosition(x, y);
	}
	
	public void MoveEnemyLeft()
	{
		if (InBounds(enemyStats.GetMonsterStats().GetXPosition()-1, enemyStats.GetMonsterStats().GetYPosition()))
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition()-1, enemyStats.GetMonsterStats().GetYPosition());
		}
	}
	
	public void MoveEnemyRight()
	{
		if (InBounds(enemyStats.GetMonsterStats().GetXPosition()+1, enemyStats.GetMonsterStats().GetYPosition()))
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition()+1, enemyStats.GetMonsterStats().GetYPosition());
		}
	}

	public void MoveEnemyDown()
	{
		if(InBounds(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()+1))
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()+1);
		}
	}
	
	public void MoveEnemyUp()
	{
		if(InBounds(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()-1))
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()-1);
		}
	}
	
	public void SetEnemyInstructions(Queue<int> enemyInstructions)
	{
		this.enemyInstructions = enemyInstructions;
	}

	public bool IsEnemyReady()
	{
		return !isEnemyMoving;
	}

	public void PerformInstructions()
	{
		this.performInstructions = true;
	}

	public PlayerMechanics GetPlayerStats()
	{
		return this.playerStats;
	}
	public EnemyMechanics GetEnemyStats()
	{
		return this.enemyStats;
	}

}
