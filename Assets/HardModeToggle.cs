using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HardModeToggle : MonoBehaviour
{
	private GameObject hardmode;
	
	// Use this for initialization
	void Start ()
	{
		this.hardmode = this.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetInt("hard_mode") == 1)
		{
			this.hardmode.SetActive(true);
		}
		else
		{
			this.hardmode.SetActive(false);
		}

		if (Input.GetKeyUp(KeyCode.P))
		{
			PlayerPrefs.SetInt("hard_mode", 0);
		}

		if (Input.GetKeyUp(KeyCode.O))
		{
			PlayerPrefs.SetInt("hard_mode", 1);
		}
	}
}
