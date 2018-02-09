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


	private bool animationsEnabled = true, walking = false;
	bool attacking = false, inAttackAnimation = false;
	private Animator anim;
	private bool damageAnimationQueued = false;
	private float damageAnimationCooldown = 0.5f, ogDamageAnimationCooldown = 0.5f;
	
	private void Awake()
	{
		this.monsterStats = new MonsterStats();
		gm = GameObject.Find("GameManager").GetComponent<GameManagement>();
	}

	// Use this for initialization
	void Start () {
		if (animationsEnabled)
		{
			anim = this.transform.GetChild(0).GetComponent<Animator>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (damageAnimationQueued)
		{
			this.damageAnimationCooldown -= Time.deltaTime;
			if (this.damageAnimationCooldown <= 0)
			{
				PlayDamageAnimation();
				damageAnimationQueued = false;
			}
		}
		
		if (attacking)
		{
			if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !inAttackAnimation)
			{
				inAttackAnimation = true;
			}

			if (inAttackAnimation)
			{
				if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
				{
					attacking = false;
					inAttackAnimation = false;
				}	
			}
		}
		 else if (isPlayerLerping)
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
		anim.SetTrigger("Attack");
		if (gm.InBounds(monsterStats.GetXPosition() + playerDirection, monsterStats.GetYPosition()))
		{
			if (gm.GetEnemyMechanics().GetMonsterStats().GetXPosition() == monsterStats.GetXPosition() + playerDirection &&
			    gm.GetEnemyMechanics().GetMonsterStats().GetYPosition() == monsterStats.GetYPosition() &&
			    !gm.GetEnemyMechanics().IsEnemyDefending())
			{
				gm.GetEnemyMechanics().GetMonsterStats().DamageMonster(1);
				gm.SetEnemyHealth(gm.GetEnemyMechanics().GetMonsterStats().Health);
//				this.loadNextPhase = true;
				gm.ZoomIntoAttack();
				gm.GetEnemyMechanics().QueueDamageAnimation();
//				gm.LoadNextPhase();

//				if (playerDirection == 1)
//				{
//					gm.GetEnemyMechanics().GetInstructions().AddFirst(4);
//				}
//				else if (playerDirection == -1)
//				{
//					gm.GetEnemyMechanics().GetInstructions().AddFirst(3);
//				}
			}
		}
	}
	
	public void QueueDamageAnimation()
	{
		this.damageAnimationQueued = true;
		this.damageAnimationCooldown = this.ogDamageAnimationCooldown;
	}

	public void PlayDamageAnimation()
	{
		anim.SetTrigger("Damage");
		anim.ResetTrigger("Attack");
		anim.ResetTrigger("Walk");
		anim.ResetTrigger("Idle");
		attacking = false;
		walking = false;
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
		if (!attacking && !isPlayerLerping && !damageAnimationQueued)
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
