using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private GameplayMechanics gameplayMechanics;
	public GameObject tileObject;

	private void Awake()
	{
		this.gameplayMechanics = GetComponent<GameplayMechanics>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnGoButtonClick(List<int> playerInstructions)
	{
		this.gameplayMechanics.SetPlayerInstructions(ParseInstructions(playerInstructions));
		this.tileObject.SetActive(false);
		this.gameplayMechanics.PerformInstructions();
	}

	Queue<int> ParseInstructions(List<int> instructions)
	{
		Queue<int> instructionQueue = new Queue<int>();
		for (int i = 0; i < instructions.Count; i++)
		{
			switch (instructions[i])
			{
				case 0: //Defence
					break;
				case 1: //Fight
					break;
				case 2: // jump
					instructionQueue.Enqueue(2);
					if (i+1 < instructions.Count && instructions[i + 1] == 3)
					{
						instructionQueue.Enqueue(3);
					} 
					else if (i+1 < instructions.Count && instructions[i + 1] == 4)
					{
						instructionQueue.Enqueue(4);
					}
					instructionQueue.Enqueue(1);
					break;
				case 3: //left
					instructionQueue.Enqueue(3);
					break;
				case 4: //right
					instructionQueue.Enqueue(4);
					break;
				case 5: // special
					break;
				default:
					break;
			}
		}
		return instructionQueue;
	}
}
