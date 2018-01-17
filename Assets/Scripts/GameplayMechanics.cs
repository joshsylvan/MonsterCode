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
	private LinkedList<int> playerInstructions;

	private int playerDirection = 1, enemyDirection = -1;
	
	private Vector2 enemyNewPosition = Vector2.zero;
	private bool isEnemyMoving = false;
	private LinkedList<int> enemyInstructions;

	private List<List<int>> enemyLevelInstructions;

	private float coolDownTimer = 3f, OGCoolDownTimer = 7f;
	private bool hasCoolDownStarted = false;

	private void Awake()
	{
		playerInstructions = new LinkedList<int>();
		enemyInstructions = new LinkedList<int>();
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
		this.arenaCellData[playerY, playerX] = 2;
		this.playerStats.GetMonsterStats().SetPosition(playerX, playerY);
		
		this.enemyObject.transform.position = arenaCells[enemyY, enemyX].transform.position;
		this.enemyStats.GetMonsterStats().SetPosition(enemyX, enemyY);
		this.arenaCellData[enemyY, enemyX] = 3;
		
		this.gameUI.SetPlayerHealth(this.playerStats.GetMonsterStats().Health);
		this.gameUI.SetEnemyHealth(this.enemyStats.GetMonsterStats().Health);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameManager.inGame)
		{
			if (!GameManager.inInstructions)
			{
//				string arena = "";
//				for (int i = 0; i < 6; i++)
//				{
//					for (int j = 0; j < 6; j++)
//					{
//						arena += this.arenaCellData[i, j] + " ";
//					}
//					arena += "\n";
//				}
//				Debug.Log(arena);
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
		if (playerInstructions.Count > 0 || enemyInstructions.Count > 0 || IsPlayerInAir() || IsEnemyInAir())
		{
			if (playerInstructions.Count > 0)
			{
				switch (playerInstructions.First.Value)
				{
					case 5:
						playerInstructions.RemoveFirst();
						PlayerAttack();
						break;
					case 4:
						playerInstructions.RemoveFirst();
						MovePlayerRight();
						break;
					case 3:
						playerInstructions.RemoveFirst();
						MovePlayerLeft();
						break;
					case 2:
						playerInstructions.RemoveFirst();
						MovePlayerUp();
						break;
					case 1:
						playerInstructions.RemoveFirst();
						MovePlayerDown();
						break;
				}
				if (IsPlayerInAir())
				{
					if(playerInstructions.Count > 0)
					if (playerInstructions.First.Value != 0 && playerInstructions.First.Value != 3 &&
					    playerInstructions.First.Value != 4)
					{
						playerInstructions.AddFirst(1);
					}
					
				}
			} 
			else if (IsPlayerInAir())
			{
				playerInstructions.AddFirst(1);
			}

			if (enemyInstructions.Count > 0)
			{
				switch (enemyInstructions.First.Value)
				{
					case 5:
						enemyInstructions.RemoveFirst();
						EnemyAttack();
						break;
					case 4:
						enemyInstructions.RemoveFirst();
						MoveEnemyRight();
						break;
					case 3:
						enemyInstructions.RemoveFirst();
						MoveEnemyLeft();
						break;
					case 2:
						enemyInstructions.RemoveFirst();
						MoveEnemyUp();
						break;
					case 1:
						enemyInstructions.RemoveFirst();
						MoveEnemyDown();
						break;
				}
				if (IsEnemyInAir())
				{
					if(enemyInstructions.Count > 0)
						if (enemyInstructions.First.Value != 0 && enemyInstructions.First.Value != 3 &&
						    enemyInstructions.First.Value != 4)
						{
							enemyInstructions.AddFirst(1);
						}
					
				}
			}
			else if (IsEnemyInAir())
			{
				enemyInstructions.AddFirst(1);
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
		Debug.Log("PLAYER: " + x + ":" + y);
		this.isPlayerLerping = true;
		this.arenaCellData[this.playerStats.GetMonsterStats().GetYPosition(), this.playerStats.GetMonsterStats().GetXPosition()] = 0;
		this.playerNewPosition = arenaCells[y, x].transform.position;
		this.playerStats.GetMonsterStats().SetPosition(x, y);
		this.arenaCellData[y, x] = 2;
	}

	public void MovePlayerLeft()
	{
		if (InBounds(playerStats.GetMonsterStats().GetXPosition()-1, playerStats.GetMonsterStats().GetYPosition()) &&
		    this.arenaCellData[playerStats.GetMonsterStats().GetYPosition(), playerStats.GetMonsterStats().GetXPosition()-1] == 0)
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition()-1, playerStats.GetMonsterStats().GetYPosition());
			playerDirection = -1;
		}
	}
	
	public void MovePlayerRight()
	{
		if (InBounds(playerStats.GetMonsterStats().GetXPosition()+1, playerStats.GetMonsterStats().GetYPosition()) &&
		    this.arenaCellData[playerStats.GetMonsterStats().GetYPosition(), playerStats.GetMonsterStats().GetXPosition()+1] == 0)
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition()+1, playerStats.GetMonsterStats().GetYPosition());
			playerDirection = 1;
		}
	}

	public void MovePlayerDown()
	{
		if(InBounds(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()+1) &&
		   this.arenaCellData[playerStats.GetMonsterStats().GetYPosition() + 1,
			   playerStats.GetMonsterStats().GetXPosition()] == 0)
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition() + 1);
		}
	}
	
	public void MovePlayerUp()
	{
		if(InBounds(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()-1) &&
		   this.arenaCellData[playerStats.GetMonsterStats().GetYPosition()-1, playerStats.GetMonsterStats().GetXPosition()] == 0)
		{
			LerpPlayerTo(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition()-1);
		}
	}

	public void PlayerAttack()
	{
		if(InBounds(playerStats.GetMonsterStats().GetXPosition()+playerDirection, playerStats.GetMonsterStats().GetYPosition()))
		{
			if (enemyStats.GetMonsterStats().GetXPosition() == playerStats.GetMonsterStats().GetXPosition() + playerDirection &&
			    enemyStats.GetMonsterStats().GetYPosition() == playerStats.GetMonsterStats().GetYPosition())
			{
				enemyStats.GetMonsterStats().DamageMonster(1);
				gameUI.SetEnemyHealth(enemyStats.GetMonsterStats().Health);
			}
		}
	}

	public bool IsPlayerInAir()
	{
		if (InBounds(playerStats.GetMonsterStats().GetXPosition(), playerStats.GetMonsterStats().GetYPosition() + 1) &&
		    this.arenaCellData[playerStats.GetMonsterStats().GetYPosition() + 1, playerStats.GetMonsterStats().GetXPosition()] == 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsPlayerReady()
	{
		return !isPlayerLerping;
	}

	public void SetPlayerInstructions(LinkedList<int> playerInstructions)
	{
		this.playerInstructions = playerInstructions;
	}
	
	private void LerpEnemyTo(int x, int y)
	{
		this.isEnemyMoving = true;
		this.arenaCellData[this.enemyStats.GetMonsterStats().GetYPosition(), this.enemyStats.GetMonsterStats().GetXPosition()] = 0;
		this.enemyNewPosition = arenaCells[y, x].transform.position;
		this.enemyStats.GetMonsterStats().SetPosition(x, y);
		this.arenaCellData[y, x] = 3;
	}
	
	public void MoveEnemyLeft()
	{
		if (InBounds(enemyStats.GetMonsterStats().GetXPosition()-1, enemyStats.GetMonsterStats().GetYPosition()) && 
		    this.arenaCellData[enemyStats.GetMonsterStats().GetYPosition(), enemyStats.GetMonsterStats().GetXPosition()-1] == 0)
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition()-1, enemyStats.GetMonsterStats().GetYPosition());
			enemyDirection = -1;
		}
	}
	
	public void MoveEnemyRight()
	{
		if (InBounds(enemyStats.GetMonsterStats().GetXPosition()+1, enemyStats.GetMonsterStats().GetYPosition()) &&
		    this.arenaCellData[enemyStats.GetMonsterStats().GetYPosition(), enemyStats.GetMonsterStats().GetXPosition()+1] == 0)
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition()+1, enemyStats.GetMonsterStats().GetYPosition());
			enemyDirection = 1;
		}
	}

	public void MoveEnemyDown()
	{
		if(InBounds(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()+1) &&
		   this.arenaCellData[enemyStats.GetMonsterStats().GetYPosition()+1, enemyStats.GetMonsterStats().GetXPosition()] == 0)
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()+1);
		}
	}
	
	public void MoveEnemyUp()
	{
		if(InBounds(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()-1) && 
		   this.arenaCellData[enemyStats.GetMonsterStats().GetYPosition()-1, enemyStats.GetMonsterStats().GetXPosition()] == 0)
		{
			LerpEnemyTo(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition()-1);
		}
	}
	
	public void EnemyAttack()
	{
		if(InBounds(enemyStats.GetMonsterStats().GetXPosition()+enemyDirection, enemyStats.GetMonsterStats().GetYPosition()))
		{
			if (playerStats.GetMonsterStats().GetXPosition() == enemyStats.GetMonsterStats().GetXPosition() + enemyDirection &&
			    playerStats.GetMonsterStats().GetYPosition() == enemyStats.GetMonsterStats().GetYPosition())
			{
				playerStats.GetMonsterStats().DamageMonster(1);
				gameUI.SetPlayerHealth(playerStats.GetMonsterStats().Health);
			}
		}
	}
	
	public void SetEnemyInstructions(LinkedList<int> enemyInstructions)
	{
		this.enemyInstructions = enemyInstructions;
	}
	
	public bool IsEnemyInAir()
	{
		if (InBounds(enemyStats.GetMonsterStats().GetXPosition(), enemyStats.GetMonsterStats().GetYPosition() + 1) &&
		    this.arenaCellData[enemyStats.GetMonsterStats().GetYPosition() + 1, enemyStats.GetMonsterStats().GetXPosition()] == 0)
		{
			return true;
		}
		else
		{
			return false;
		}
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
