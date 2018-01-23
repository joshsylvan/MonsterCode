using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Levels
{
	private List<List<List<int>>> levels;
	private List<List<int>> level_1, level_2, level_3;

	private List<int> avaliableTiles1, avaliableTiles2, avaliableTiles3;
	
	public Levels()
	{
		avaliableTiles1 = new List<int>()
		{
			0, 1, 2, 3, 4
		};
		
		levels = new List<List<List<int>>>();
		level_1 = new List<List<int>>(){
			new List<int> {3, 3},
			new List<int> {3, 3, 1},
			new List<int> {3, 4, 3, 3, 1}
		};
		
		avaliableTiles2 = new List<int>()
		{
			0, 1, 2, 3, 4, 5
		};
		
		level_2 = new List<List<int>>(){
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3}
		};
		
		avaliableTiles3 = new List<int>()
		{
			0, 1, 2, 3, 4, 5
		};
		
		level_3 = new List<List<int>>()
		{
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3}
		};
		
		levels.Add(level_1);
		levels.Add(level_2);
		levels.Add(level_3);
	}

	public List<List<List<int>>> GetLevels()
	{
		return levels;
	}

	public List<List<int>> GetLevel(int index)
	{
		return levels[index];
	}

	public List<int> GetAvaliableTiles1()
	{
		return avaliableTiles1;
	}
	
	public List<int> GetAvaliableTiles2()
	{
		return avaliableTiles2;
	}
	
	public List<int> GetAvaliableTiles3()
	{
		return avaliableTiles3;
	}
}
