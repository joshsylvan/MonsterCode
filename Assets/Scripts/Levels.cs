﻿using System.Collections;
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
		levelsNew = new List<Level>();
		//Test Level
//		levelsNew.Add( new Level( 
//			"Skeleton", 
//			new List<List<int>>(){
//				new List<int> {1, 1, 1, 0, 0},
//				new List<int> {3, 1, 1},
//				new List<int> {0, 0, 0},
//				new List<int> {3, 3, 1, 1}
//			},
//			new List<int>() {1, 1, 1, 1},
//			new List<int>() {0, 1, 2, 3},
//			new int[2] {0, 5},
//			new int[2] {1, 5}
//		));
		
		levelsNew.Add( new Level( 
			"Skeleton", 
			new List<List<int>>(){
				new List<int> {3},
				new List<int> {3, 1},
				new List<int> {3, 1, 3},
				new List<int> {3, 3, 1, 1}
			},
			new List<int>() {1, 1, 1, 1},
			new List<int>() {0, 1, 2, 3},
			new int[2] {1, 5},
			new int[2] {4, 5}
		));
		
		levelsNew.Add( new Level( 
			"Undead", 
			new List<List<int>>(){
				new List<int> { 3, 3, 1},
				new List<int> { 3, 1, 1, 4},
				new List<int> {1, 1, 3, 1},
				new List<int> {3, 1, 3, 1, 1, }
			},
			new List<int>() { 1, 1, 2, 2 },
			new List<int>() { 0, 1, 2, 3 },
			new int[2] {1, 5},
			new int[2] {4, 5}
		));
		
		levelsNew.Add( new Level( 
			"Knight", 
			new List<List<int>>(){
				new List<int> {3, 3, 1, 0},
				new List<int> {3, 3, 0, 3, 1},
				new List<int> {3, 3, 1, 4, 3, 1},
				new List<int> {3, 3, 1, 4, 0, 3, 1},
				new List<int> {3, 3, 1, 1, 4, 3, 1}
			},
			new List<int>() {2, 2, 2, 2, 3},
			new List<int>() { 0, 1, 2, 3 },
			new int[2] {0, 5},
			new int[2] {5, 5}
		));
		
		levelsNew.Add( new Level( 
			"Shadow Skeleton", 
			new List<List<int>>(){
				new List<int> {1, 1, 4, 3, 0, 1},
				new List<int> {4, 3, 1, 1, 0, 1},
				new List<int> {1, 0, 1, 3, 1, 1},
				new List<int> {0, 1, 1, 1, 3, 1},
				new List<int> {3, 1, 4, 4, 3, 1}
			},
			new List<int>() {2, 2, 2, 3, 3},
			new List<int>() { 0, 1, 2, 3 },
			new int[2] {1, 5},
			new int[2] {3, 5}
		));

		levelsNew.Add(new Level(
			"Elder Undead",
			new List<List<int>>()
			{
				new List<int> {1, 1, 4, 4, 3, 1},
				new List<int> {1, 4, 3, 1, 0, 4},
				new List<int> {4, 4, 3, 1, 1, 1},
				new List<int> {4, 4, 3, 1, 0, 1},
				new List<int> {1, 4, 3, 1, 4, 4}
			},
			new List<int>() {2, 2, 2, 3, 3},
			new List<int>() {0, 1, 2, 3},
			new int[2] {0, 5},
			new int[2] {1, 5}
		));
		
		levelsNew.Add( new Level( 
			"Ancient Knight", 
			new List<List<int>>(){
				new List<int> {1, 1, 0, 0, 1},
				new List<int> {3, 1, 4, 3, 1, 0, 1},
				new List<int> {1, 0, 1, 4, 3, 1},
				new List<int> {4, 4, 3, 1, 1, 0, 1},
				new List<int> {1, 1, 1, 4, 3, 1}
			},
			new List<int>() { 2, 2, 2, 3, 3 },
			new List<int>() { 0, 1, 2, 3 },
			new int[2] {1, 5},
			new int[2] {3, 5}
		));
	}

	public Level GetLevel(int index)
	{
//		return levels[index];
		return levelsNew[index];
	}

	public int GetLevelCount()
	{
		return levelsNew.Count;
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
