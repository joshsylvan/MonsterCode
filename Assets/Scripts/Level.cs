using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
	private readonly string enemy;
	private readonly List<List<int>> phases;
	private readonly List<int> heartsForPhase;
	private readonly List<int> avaliableTiles;
	private readonly int[] playerPos;
	private readonly int[] enemyPos;


	public Level(string enemy, List<List<int>> phases, List<int> heartsForPhase, List<int> avaliableTiles, int[] playerPos, int[] enemyPos)
	{
		this.enemy = enemy;
		this.phases = phases;
		this.heartsForPhase = heartsForPhase;
		this.avaliableTiles = avaliableTiles;
		this.playerPos = playerPos;
		this.enemyPos = enemyPos;

	}

	public string GetEnemyName()
	{
		return this.enemy;
	}

	public List<int> GetPhase(int i)
	{
		return this.phases[i];
	}

	public List<List<int>> GetPhases()
	{
		return this.phases;
	}

	public List<int> GetAvaliableTiles()
	{
		return this.avaliableTiles;
	}

	public List<int> GetHeartsForPhase()
	{
		return this.heartsForPhase;
	}

	public int[] GetPlayerPos()
	{
		return playerPos;
	}
	
	public int[] GetEnemyPos()
	{
		return enemyPos;
	}
}
