using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Levels
{
	private List<List<List<int>>> levels;
	private List<List<int>> level_1, level_2, level_3;
	
	public Levels()
	{
		levels = new List<List<List<int>>>();
		level_1 = new List<List<int>>(){
			new List<int> {3, 3, 1},
			new List<int> {4, 4, 4},
			new List<int> {1, 1, 2}
		};
		
		level_2 = new List<List<int>>(){
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3},
			new List<int> {3, 3, 3}
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
}
