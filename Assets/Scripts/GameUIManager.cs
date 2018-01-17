using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{

	public GameObject playerHealth, enemyHealth;
	public GameObject countDown;

	private void Awake()
	{
		this.playerHealth = transform.GetChild(0).gameObject;
		this.enemyHealth = transform.GetChild(1).gameObject;
		this.countDown = transform.GetChild(2).gameObject;
		playerHealth.SetActive(true);
		enemyHealth.SetActive(true);
		countDown.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetEnemyHealth(int health)
	{
		if (health <= enemyHealth.transform.childCount)
		{
			int remainngHealth = health;
			for (int i = enemyHealth.transform.childCount - 1; i >= 0; i--)
			{
				if (remainngHealth > 0)
				{
					enemyHealth.transform.GetChild(i).gameObject.SetActive(true);
					remainngHealth--;
				}
				else
				{
					enemyHealth.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}
	
	public void SetPlayerHealth(int health)
	{
		if (health <= playerHealth.transform.childCount)
		{
			int remainngHealth = health;
			for (int i = 0; i < playerHealth.transform.childCount; i++)
			{
				if (remainngHealth > 0)
				{
					playerHealth.transform.GetChild(i).gameObject.SetActive(true);
					remainngHealth--;
				}
				else
				{
					playerHealth.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}
	
}
