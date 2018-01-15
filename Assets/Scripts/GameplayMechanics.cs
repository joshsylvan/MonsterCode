using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMechanics : MonoBehaviour
{
	private GameObject environment;
	private GameObject playerObject, enemyObject;
	private PlayerMechanics playerStats;
	private EnemyMechanics enemyStats;
	
	// Use this for initialization
	void Start ()
	{
		this.environment = GameObject.Find("Environment");
		this.playerObject = this.environment.transform.GetChild(0).GetChild(0).gameObject;
		this.enemyObject = this.environment.transform.GetChild(0).GetChild(1).gameObject;
		this.playerStats = playerObject.GetComponent<PlayerMechanics>();
		this.enemyStats = enemyObject.GetComponent<EnemyMechanics>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
