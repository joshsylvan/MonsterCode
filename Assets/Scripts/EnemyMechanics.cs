using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics : MonoBehaviour {

	private MonsterStats monsterStats;
	private GameObject headObject, bodyObject, legsObject;
	
	// Use this for initialization
	void Start () {
		this.monsterStats = new MonsterStats();
		this.headObject = transform.GetChild(0).gameObject;
		this.bodyObject = transform.GetChild(1).gameObject;
		this.legsObject = transform.GetChild(2).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public MonsterStats GetMonsterStats()
	{
		return this.monsterStats;
	}
	
	public void SetMonsterBody(int headIndex, int bodyIndex, int legsIndex)
	{
		
	}
	
	public void SetRandomMonsterBody()
	{
		
	}
}
