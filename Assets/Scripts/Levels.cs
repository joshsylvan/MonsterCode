using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Levels
{
	private List<List<List<int>>> levels;
	private List<List<int>> level_1, level_2, level_3;

	private List<Level> levelsNew;

	private List<int> avaliableTiles1, avaliableTiles2, avaliableTiles3;
	
	public Levels()
	{
//		avaliableTiles1 = new List<int>()
//		{
//			0, 1, 2, 3, 4
//		};
//		
//		levels = new List<List<List<int>>>();
//		level_1 = new List<List<int>>(){
//			new List<int> {3, 3},
//			new List<int> {3, 3, 1},
//			new List<int> {3, 4, 3, 3, 1}
//		};
		levelsNew = new List<Level>();
		levelsNew.Add( new Level( 
			"Skeleton", 
			new List<List<int>>(){
				new List<int> {3, 3},
//				new List<int> {3, 3, 1},
//				new List<int> {3, 4, 3, 3, 1}
			},
			new List<int>() {1, 1, 1},
			new List<int>() { 0, 1, 2, 3, 4}
		));
		
//		avaliableTiles2 = new List<int>()
//		{
//			0, 1, 2, 3, 4, 5
//		};
//		
//		level_2 = new List<List<int>>(){
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3}
//		};
		
		levelsNew.Add( new Level( 
			"Undead", 
			new List<List<int>>(){
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3}
			},
			new List<int>() {1, 1, 2, 2},
			new List<int>() { 0, 1, 2, 3, 4}
		));
		
//		avaliableTiles3 = new List<int>()
//		{
//			0, 1, 2, 3, 4, 5
//		};
//		
//		level_3 = new List<List<int>>()
//		{
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3},
//			new List<int> {3, 3, 3}
//		};
//		
		levelsNew.Add( new Level( 
			"Knight", 
			new List<List<int>>(){
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3},
				new List<int> {3, 3, 3}
			},
			new List<int>() {2, 2, 2, 3, 3},
			new List<int>() { 0, 1, 2, 3, 4}
		));
		
//		levels.Add(level_1);
//		levels.Add(level_2);
//		levels.Add(level_3);
	}

	public Level GetLevel(int index)
	{
//		return levels[index];
		return levelsNew[index];
	}

//	public List<int> GetAvaliableTiles1()
//	{
//		return avaliableTiles1;
//	}
//	
//	public List<int> GetAvaliableTiles2()
//	{
//		return avaliableTiles2;
//	}
//	
//	public List<int> GetAvaliableTiles3()
//	{
//		return avaliableTiles3;
//	}
}
