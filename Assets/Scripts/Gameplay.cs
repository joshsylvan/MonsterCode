using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{


	private GameManagement gm;
	private PlayerMechanics playerMechanics;
	private EnemyMechanics enemyMechanics;
	
	// Timers
	private bool hasCoolDownStarted = false;
	private float coolDownTimer = 3f;
	
	// Gameplay
	private bool performInstructions = false;
	private bool loadNextPhase = false;

	private void Awake()
	{
		gm = GetComponent<GameManagement>();
		playerMechanics = gm.GetPlayerMechanics();
		enemyMechanics = gm.GetEnemyMechanics();
	}

	public void InitGame()
	{
//		this.gameUI = GameObject.Find("GameUI").GetComponent<GameUIManager>();
	
		playerMechanics = gm.GetPlayerMechanics();
		enemyMechanics = gm.GetEnemyMechanics();
		
		gm.instructionTiles.SetActive(false);
		
		StartCoolDownTimer(1);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManagement.InGame)
		{
			if (!GameManagement.InInstructions)
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
					GameManagement.InInstructions = true;
					if (loadNextPhase)
					{
						loadNextPhase = false;
						GameManager.currentLevelphase++;
					}
				}
				
				if (performInstructions)
				{
					PlayGame();
				}
			}
			else
			{
				gm.ShowInstructionUI();
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
		if (playerMechanics.GetInstructions().Count > 0 || enemyMechanics.GetInstructions().Count > 0 || playerMechanics.IsPlayerInAir() || enemyMechanics.IsEnemyInAir())
		{
			if (gm.enemyMechanics.IsEnemyReady() && gm.playerMechanics.IsPlayerReady())
			{
				if (playerMechanics.GetInstructions().Count > 0)
				{
					switch (playerMechanics.GetInstructions().First.Value)
					{
						case 6:
							playerMechanics.GetInstructions().RemoveFirst();
							playerMechanics.PlayerDefend();
							break;
						case 5:
							playerMechanics.GetInstructions().RemoveFirst();
							playerMechanics.PlayerAttack();
							break;
						case 4:
							playerMechanics.GetInstructions().RemoveFirst();
							playerMechanics.MovePlayerRight();
							break;
						case 3:
							playerMechanics.GetInstructions().RemoveFirst();
							playerMechanics.MovePlayerLeft();
							break;
						case 2:
							playerMechanics.GetInstructions().RemoveFirst();
							playerMechanics.MovePlayerUp();
							break;
						case 1:
							playerMechanics.GetInstructions().RemoveFirst();
							playerMechanics.MovePlayerDown();
							break;
					}

					if (playerMechanics.IsPlayerInAir())
					{
						if (playerMechanics.GetInstructions().Count > 0)
							if (playerMechanics.GetInstructions().First.Value != 0 && playerMechanics.GetInstructions().First.Value != 3 &&
							    playerMechanics.GetInstructions().First.Value != 4)
							{
								playerMechanics.GetInstructions().AddFirst(1);
							}

					}
				}
				else if (playerMechanics.IsPlayerInAir())
				{
					playerMechanics.GetInstructions().AddFirst(1);
				}

				if (enemyMechanics.GetInstructions().Count > 0)
				{
					switch (enemyMechanics.GetInstructions().First.Value)
					{
						case 6:
							enemyMechanics.GetInstructions().RemoveFirst();
							enemyMechanics.EnemyDefend();
							break;
						case 5:
							enemyMechanics.GetInstructions().RemoveFirst();
							enemyMechanics.EnemyAttack();
							break;
						case 4:
							enemyMechanics.GetInstructions().RemoveFirst();
							enemyMechanics.MoveEnemyRight();
							break;
						case 3:
							enemyMechanics.GetInstructions().RemoveFirst();
							enemyMechanics.MoveEnemyLeft();
							break;
						case 2:
							enemyMechanics.GetInstructions().RemoveFirst();
							enemyMechanics.MoveEnemyUp();
							break;
						case 1:
							enemyMechanics.GetInstructions().RemoveFirst();
							enemyMechanics.MoveEnemyDown();
							break;
					}

					if (enemyMechanics.IsEnemyInAir())
					{
						if (enemyMechanics.GetInstructions().Count > 0)
							if (enemyMechanics.GetInstructions().First.Value != 0 && enemyMechanics.GetInstructions().First.Value != 3 &&
							    enemyMechanics.GetInstructions().First.Value != 4)
							{
								enemyMechanics.GetInstructions().AddFirst(1);
							}

					}
				}
				else if (enemyMechanics.IsEnemyInAir())
				{
					enemyMechanics.GetInstructions().AddFirst(1);
				}
			}
		}
		else
		{
			StartCoolDownTimer(3);
			performInstructions = false;
		}
	}
	
	public void PerformInstructions()
	{
		this.performInstructions = true;
	}
}
