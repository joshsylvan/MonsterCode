using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
	private readonly string enemy;
	private readonly List<List<int>> phases;
	private readonly List<int> heartsForPhase;
	private readonly List<int> avaliableTiles;


	public Level(string enemy, List<List<int>> phases, List<int> heartsForPhase, List<int> avaliableTiles)
	{
		this.enemy = enemy;
		this.phases = phases;
		this.heartsForPhase = heartsForPhase;
		this.avaliableTiles = avaliableTiles;
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
}
