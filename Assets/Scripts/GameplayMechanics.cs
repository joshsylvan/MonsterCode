using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMechanics : MonoBehaviour
{
	private GameObject playerObject, enemyObject, arenaGameObject, environment;
	private PlayerMechanics playerStats;
	private EnemyMechanics enemyStats;

	private GameObject[,] arenaCells;

	private bool performInstructions = false;
	
	private Vector2 playerNewPosition = Vector2.zero;
	private bool isPlayerLerping = false, isPlayerJumping = false;
	private float movementSpeed = 5f;
	private float minimumDistance = 0.1f;
	private Queue<int> playerInstructions;
	
	private Vector2 enemyNewPosition = Vector2.zero;
	private bool isEnemyMoving = false;
	private Queue<int> enemyInstructions;

	private void Awake()
	{

		playerInstructions = new Queue<int>();
		playerInstructions.Enqueue(1);
		playerInstructions.Enqueue(1);
		playerInstructions.Enqueue(1);
		playerInstructions.Enqueue(1);
		playerInstructions.Enqueue(1);
		playerInstructions.Enqueue(1);
		playerInstructions.Enqueue(2);
		playerInstructions.Enqueue(3);
		
		enemyInstructions = new Queue<int>();
		enemyInstructions.Enqueue(2);
		enemyInstructions.Enqueue(2);
		enemyInstructions.Enqueue(2);
		enemyInstructions.Enqueue(2);
		enemyInstructions.Enqueue(2);
		enemyInstructions.Enqueue(2);
		enemyInstructions.Enqueue(1);
		enemyInstructions.Enqueue(3);
		enemyInstructions.Enqueue(4);

	}

	// Use this for initialization
	void Start ()
	{
		this.environment = GameObject.Find("Environment");
		this.playerObject = this.environment.transform.GetChild(1).GetChild(0).gameObject;
		this.enemyObject = this.environment.transform.GetChild(1).GetChild(1).gameObject;
		this.playerStats = playerObject.GetComponent<PlayerMechanics>();
		this.enemyStats = enemyObject.GetComponent<EnemyMechanics>();
		this.arenaGameObject = environment.transform.GetChild(0).gameObject;
		
		this.arenaCells = new GameObject[6,6];
		for (int i = 0; i < this.arenaCells.GetLength(0); i++)
		{
			for (int j = 0; j < this.arenaCells.GetLength(1); j++)
			{
				this.arenaCells[i, j] = this.arenaGameObject.transform.GetChild(i).GetChild(j).gameObject;
			}
		}
		
		int startX = 1, startY = 4;
		this.playerObject.transform.position = arenaCells[startY, startX].transform.position;
		this.playerStats.GetMonsterStats().SetPosition(startX, startY);
		
		this.enemyObject.transform.position = arenaCells[4, 4].transform.position;
		this.enemyStats.GetMonsterStats().SetPosition(4, 4);
		performInstructions = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Move Player
		if (performInstructions && IsEnemyReady() && IsPlayerReady())
		{
			PlayGame();
		}
		
		
		if (isPlayerLerping)
		{
			Vector2 newPos = Vector2.Lerp(playerObject.transform.position, playerNewPosition, Time.deltaTime*movementSpeed);
			playerObject.transform.position = newPos;
			if (Vector2.Distance(playerObject.transform.position, playerNewPosition) < minimumDistance)
			{
				isPlayerLerping = false;
			}
		}
		
		//Move Enemy
		if (isEnemyMoving)
		{
			Vector2 newPos = Vector2.Lerp(enemyObject.transform.position, enemyNewPosition, Time.deltaTime*movementSpeed);
			enemyObject.transform.position = newPos;
			if (Vector2.Distance(enemyObject.transform.position, enemyNewPosition) < minimumDistance)
			{
				isEnemyMoving = false;
			}
		}
		
	}

	private void PlayGame()
	{
		if (playerInstructions.Count > 0)
		{
			switch (playerInstructions.Dequeue())
			{
				case 1:
					MovePlayerRight();
					break;
				case 2:
					MovePlayerLeft();
					break;
				case 3:
					MovePlayerUp();
					break;
				case 4:
					MovePlayerDown();
					break;
			}
		}
		if (enemyInstructions.Count > 0)
		{
			switch (enemyInstructions.Dequeue())
			{
				case 1:
					MoveEnemyRight();
					break;
				case 2:
					MoveEnemyLeft();
					break;
				case 3:
					MoveEnemyUp();
					break;
				case 4:
					MoveEnemyDown();
					break;
			}
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
	
}
