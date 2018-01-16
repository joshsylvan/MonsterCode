using System.Collections;
using System.Collections.Generic;
public class MonsterStats
{
	private int health, damage;
	private bool isDefending;
	private int xPosition, yPosition;
	
    public MonsterStats()
    {
	    health = 3;
	    damage = 1;
	    isDefending = false;
	    xPosition = 0;
	    yPosition = 0;
    }

	public MonsterStats(int health, int damage, int xPosition, int yPosition)
	{
		this.health = health;
		this.damage = damage;
		this.xPosition = xPosition;
		this.yPosition = yPosition;
		this.isDefending = false;
	}

	public MonsterStats(int xPosition, int yPosition)
	{
		this.xPosition = xPosition;
		this.yPosition = yPosition;
		this.isDefending = false;
	}

	public bool IsDead()
	{
		return health <= 0;
	}

	public void DamageMonster(int damage)
	{
		this.health -= damage;
	}

	public int Health
	{
		get { return health; }
		set { health = value; }
	}

	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}

	public bool IsDefending
	{
		get { return isDefending; }
		set { isDefending = value; }
	}

	public int GetXPosition()
	{
		return this.xPosition;
	}
	
	public int GetYPosition()
	{
		return this.yPosition;
	}

	public void SetPosition(int xPosition, int yPosition)
	{
		this.xPosition = xPosition;
		this.yPosition = yPosition;
	}
}
