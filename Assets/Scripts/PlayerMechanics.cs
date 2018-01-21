using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
	
	private GameManagement gm;

	private Vector2 playerNewPosition = Vector2.zero;
	private bool isPlayerJumping = false;
	private float movementSpeed = 5f;
	private float minimumDistance = 0.1f;
	private int playerDirection = 1;
	private LinkedList<int> playerInstructions;
	
	private MonsterStats monsterStats;

	private bool isPlayerLerping = false, loadNextPhase = false, isPlayerDefending = false;

	private void Awake()
	{
		this.monsterStats = new MonsterStats();
		gm = GameObject.Find("GameManager").GetComponent<GameManagement>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerLerping)
		{
			Vector2 newPos = Vector2.Lerp(transform.position, playerNewPosition, Time.deltaTime * movementSpeed);
			transform.position = newPos;
			if (Vector2.Distance(transform.position, playerNewPosition) < minimumDistance)
			{
				isPlayerLerping = false;
			}
		}
	}

	public MonsterStats GetMonsterStats()
	{
		return this.monsterStats;
	}
	
	private void LerpPlayerTo(int x, int y)
	{
		this.isPlayerLerping = true;
		gm.GetArenaCellData()[this.monsterStats.GetYPosition(), this.monsterStats.GetXPosition()] = 0;
		this.playerNewPosition = gm.GetArenaCellsObjet()[y, x].transform.position;
		this.monsterStats.SetPosition(x, y);
		gm.GetArenaCellData()[y, x] = 2;
	}

	public void MovePlayerLeft()
	{
		playerDirection = -1;
		if (gm.InBounds(monsterStats.GetXPosition()-1, monsterStats.GetYPosition()) &&
		    gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()-1] == 0)
		{
			LerpPlayerTo(monsterStats.GetXPosition()-1, monsterStats.GetYPosition());
		}
	}
	
	public void MovePlayerRight()
	{
		playerDirection = 1;
		if (gm.InBounds(monsterStats.GetXPosition()+1, monsterStats.GetYPosition()) &&
		    gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()+1] == 0)
		{
			LerpPlayerTo(monsterStats.GetXPosition()+1, monsterStats.GetYPosition());
		}
	}

	public void MovePlayerDown()
	{
		if(gm.InBounds(monsterStats.GetXPosition(), monsterStats.GetYPosition()+1) &&
		   gm.GetArenaCellData()[monsterStats.GetYPosition() + 1,
			   monsterStats.GetXPosition()] == 0)
		{
			LerpPlayerTo(monsterStats.GetXPosition(), monsterStats.GetYPosition() + 1);
		}
	}
	
	public void MovePlayerUp()
	{
		if(gm.InBounds(monsterStats.GetXPosition(), monsterStats.GetYPosition()-1) &&
		   gm.GetArenaCellData()[monsterStats.GetYPosition()-1, monsterStats.GetXPosition()] == 0)
		{
			LerpPlayerTo(monsterStats.GetXPosition(), monsterStats.GetYPosition()-1);
		}
	}

	public void PlayerAttack()
	{
		if(gm.InBounds(monsterStats.GetXPosition()+playerDirection, monsterStats.GetYPosition()))
		{
			if (gm.GetEnemyMechanics().GetMonsterStats().GetXPosition() == monsterStats.GetXPosition() + playerDirection &&
			    gm.GetEnemyMechanics().GetMonsterStats().GetYPosition() == monsterStats.GetYPosition() &&
			    !gm.GetEnemyMechanics().IsEnemyDefending()) ;
			{
				gm.GetEnemyMechanics().GetMonsterStats().DamageMonster(1);
				gm.SetEnemyHealth(gm.GetEnemyMechanics().GetMonsterStats().Health);
				this.loadNextPhase = true;
				if (playerDirection == 1)
				{
					gm.GetEnemyMechanics().GetInstructions().AddFirst(4);
				}
				else if(playerDirection == -1)
				{
					gm.GetEnemyMechanics().GetInstructions().AddFirst(3);
				}
			}
		}
	}

	public void PlayerDefend()
	{
		this.isPlayerDefending = true;
	}

	public bool IsPlayerInAir()
	{
		if (gm.InBounds(monsterStats.GetXPosition(), monsterStats.GetYPosition() + 1) &&
		    gm.GetArenaCellData()[monsterStats.GetYPosition() + 1, monsterStats.GetXPosition()] == 0)
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

	public LinkedList<int> GetInstructions()
	{
		return playerInstructions;
	}
}
