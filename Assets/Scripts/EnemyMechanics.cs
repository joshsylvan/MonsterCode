using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics : MonoBehaviour {

	private GameManagement gm;

	private Vector2 enemyNewPosition = Vector2.zero;
	private bool isEnemyJumpting = false;
	private float movementSpeed = 5f;
	private float minimumDistance = 0.1f;
	private int enemyDirection = 1;
	private LinkedList<int> enemyInstructions;
	
	private MonsterStats monsterStats;

	private bool isEnemyLerping = false, loadNextPhase = false, isEnemyDefending = false;

	private bool animationsEnabled = true, walking = false;
	private bool attacking = false, inAttackAnimation = false;
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
		else if (isEnemyLerping)
		{
			Vector2 newPos = Vector2.MoveTowards(transform.position, enemyNewPosition, Time.deltaTime * movementSpeed);
			transform.position = newPos;
			if (Vector2.Distance(transform.position, enemyNewPosition) < minimumDistance)
			{
				isEnemyLerping = false;
				if (walking)
				{
					walking = false;
					anim.SetTrigger("Idle");
				}
			}
		}

		if (this.monsterStats.Health <= 0)
		{
			gm.LoadNextPhase();
		}
	}
	
	private void LerpEnemyTo(int x, int y)
	{
		this.isEnemyLerping = true;
		gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()] = 0;
		this.enemyNewPosition = gm.GetArenaCellsObjet()[y, x].transform.position;
		monsterStats.SetPosition(x, y);
		gm.GetArenaCellData()[y, x] = 3;
	}
	
	public void MoveEnemyLeft()
	{
		enemyDirection = -1;
		this.transform.localScale = new Vector3(
			Mathf.Abs(this.transform.localScale.x) * enemyDirection,
			this.transform.localScale.y,
			this.transform.localScale.z
		);
		if (gm.InBounds(monsterStats.GetXPosition()-1, monsterStats.GetYPosition()) && 
		    gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()-1] == 0)
		{
			this.walking = true;
			this.attacking = false;
			anim.SetTrigger("Walk");
			LerpEnemyTo(monsterStats.GetXPosition()-1, monsterStats.GetYPosition());
		}
	}
	
	public void MoveEnemyRight()
	{
		enemyDirection = 1;
		this.transform.localScale = new Vector3(
			Mathf.Abs(this.transform.localScale.x) * enemyDirection,
			this.transform.localScale.y,
			this.transform.localScale.z
		);
		if (gm.InBounds(monsterStats.GetXPosition()+1, monsterStats.GetYPosition()) &&
		    gm.GetArenaCellData()[monsterStats.GetYPosition(), monsterStats.GetXPosition()+1] == 0)
		{
			this.walking = true;
			this.attacking = false;
			anim.SetTrigger("Walk");
			LerpEnemyTo(monsterStats.GetXPosition()+1, monsterStats.GetYPosition());
		}
	}

	public void MoveEnemyDown()
	{
		if(gm.InBounds(monsterStats.GetXPosition(), monsterStats.GetYPosition()+1) &&
		   gm.GetArenaCellData()[monsterStats.GetYPosition()+1, monsterStats.GetXPosition()] == 0)
		{
			LerpEnemyTo(monsterStats.GetXPosition(), monsterStats.GetYPosition()+1);
		}
	}
	
	public void MoveEnemyUp()
	{
		if(gm.InBounds(monsterStats.GetXPosition(), monsterStats.GetYPosition()-1) && 
		   gm.GetArenaCellData()[monsterStats.GetYPosition()-1, monsterStats.GetXPosition()] == 0)
		{
			LerpEnemyTo(monsterStats.GetXPosition(), monsterStats.GetYPosition()-1);
		}
	}
	
	public void EnemyAttack()
	{
		attacking = true;
		anim.SetTrigger("Attack");
		if(gm.InBounds(monsterStats.GetXPosition()+enemyDirection, monsterStats.GetYPosition()))
		{
			if (gm.GetPlayerMechanics().GetMonsterStats().GetXPosition() == monsterStats.GetXPosition() + enemyDirection &&
			    gm.GetPlayerMechanics().GetMonsterStats().GetYPosition() == monsterStats.GetYPosition())
			{
				if (!gm.GetPlayerMechanics().IsPlayerDefending())
				{
					gm.GetPlayerMechanics().GetMonsterStats().DamageMonster(1);
					gm.SetPlayerHealth(gm.GetPlayerMechanics().GetMonsterStats().Health);
					gm.ZoomIntoAttack();
					gm.GetPlayerMechanics().QueueDamageAnimation();
				}

//				if (enemyDirection == 1)
//				{
//					gm.GetPlayerMechanics().GetInstructions().AddFirst(4);
//				}
//				else if(enemyDirection == -1)
//				{
//					gm.GetPlayerMechanics().GetInstructions().AddFirst(3);
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

	public void EnemyDefend()
	{
		this.isEnemyDefending = true;
	}
	
	public void EnemyDefendEnd()
	{
		this.isEnemyDefending = false;
	}
	
	public bool IsEnemyInAir()
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

	public bool IsEnemyReady()
	{
		if (!attacking && !isEnemyLerping && !damageAnimationQueued)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public MonsterStats GetMonsterStats()
	{
		return this.monsterStats;
	}

	public LinkedList<int> GetInstructions()
	{
		return enemyInstructions;
	}

	public void SetEnemyInstructions(LinkedList<int> enemyInstructions)
	{
		this.enemyInstructions = enemyInstructions;
	}

	public bool IsEnemyDefending()
	{
		return isEnemyDefending;
	}
}
