using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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


	private bool animationsEnabled = true, walking = false, attacking = false;
	private Animator anim;
	
	private void Awake()
	{
		this.monsterStats = new MonsterStats();
		gm = GameObject.Find("GameManager").GetComponent<GameManagement>();
		if (animationsEnabled)
		{
			anim = this.transform.GetChild(0).GetComponent<Animator>();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerLerping)
		{
			Vector2 newPos = Vector2.MoveTowards(transform.position, playerNewPosition, Time.deltaTime * movementSpeed);
			transform.position = newPos;
			if (Vector2.Distance(transform.position, playerNewPosition) < minimumDistance)
			{
				isPlayerLerping = false;
				if (walking)
				{
					walking = false;
					anim.SetTrigger("Idle");
				}
			}
		}
		if(attacking)
		{
			if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
			{
				attacking = false;
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
		this.transform.localScale = new Vector3(
			Mathf.Abs(this.transform.localScale.x) * playerDirection,
			this.transform.localScale.y,
			this.transform.localScale.z
		);
		if (gm.InBounds(monsterStats.GetXPosition()-1, monsterStats.GetYPosition()) &&
		    gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()-1] == 0)
		{
			this.walking = true;
			this.attacking = false;
			anim.SetTrigger("Walk");
			LerpPlayerTo(monsterStats.GetXPosition()-1, monsterStats.GetYPosition());
		}
	}
	
	public void MovePlayerRight()
	{
		playerDirection = 1;
		this.transform.localScale = new Vector3(
			Mathf.Abs(this.transform.localScale.x) * playerDirection,
			this.transform.localScale.y,
			this.transform.localScale.z
		);
		if (gm.InBounds(monsterStats.GetXPosition()+1, monsterStats.GetYPosition()) &&
		    gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()+1] == 0)
		{
			this.walking = true;
			this.attacking = false;
			anim.SetTrigger("Walk");
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
		attacking = true;
		isPlayerLerping = false;
		anim.SetTrigger("Attack");
		if (gm.InBounds(monsterStats.GetXPosition() + playerDirection, monsterStats.GetYPosition()))
		{
			if (gm.GetEnemyMechanics().GetMonsterStats().GetXPosition() == monsterStats.GetXPosition() + playerDirection &&
			    gm.GetEnemyMechanics().GetMonsterStats().GetYPosition() == monsterStats.GetYPosition() &&
			    !gm.GetEnemyMechanics().IsEnemyDefending())
			{
					Debug.Log(gm.GetEnemyMechanics().GetMonsterStats().GetXPosition() + " = " + monsterStats.GetXPosition());
				gm.GetEnemyMechanics().GetMonsterStats().DamageMonster(1);
				gm.SetEnemyHealth(gm.GetEnemyMechanics().GetMonsterStats().Health);
				this.loadNextPhase = true;
				if (playerDirection == 1)
				{
					gm.GetEnemyMechanics().GetInstructions().AddFirst(4);
				}
				else if (playerDirection == -1)
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
		if (!attacking && !isPlayerLerping)
		{
			return true;
		}
		else
		{
			return false;
		}
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
