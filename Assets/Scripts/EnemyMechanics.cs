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
		if (isEnemyLerping)
		{
			Vector2 newPos = Vector2.Lerp(transform.position, enemyNewPosition, Time.deltaTime * movementSpeed);
			transform.position = newPos;
			if (Vector2.Distance(transform.position, enemyNewPosition) < minimumDistance)
			{
				isEnemyLerping = false;
			}
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
		if(gm.InBounds(monsterStats.GetXPosition()+enemyDirection, monsterStats.GetYPosition()))
		{
			if (gm.GetPlayerMechanics().GetMonsterStats().GetXPosition() == monsterStats.GetXPosition() + enemyDirection &&
			    gm.GetPlayerMechanics().GetMonsterStats().GetYPosition() == monsterStats.GetYPosition())
			{
				gm.GetPlayerMechanics().GetMonsterStats().DamageMonster(1);
				gm.SetPlayerHealth(gm.GetPlayerMechanics().GetMonsterStats().Health);
				if (enemyDirection == 1)
				{
					gm.GetPlayerMechanics().GetInstructions().AddFirst(4);
				}
				else if(enemyDirection == -1)
				{
					gm.GetPlayerMechanics().GetInstructions().AddFirst(3);
				}
			}
		}
	}

	public void EnemyDefend()
	{
		this.isEnemyDefending = true;
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
		return !isEnemyLerping;
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
