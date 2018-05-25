using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HardModeToggle : MonoBehaviour
{
	private GameObject hardmode;
	bool hasChanged = false;
	
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

//		if (Input.GetKeyUp(KeyCode.Alpha0))
//		{
//			PlayerPrefs.SetInt("current_level", 0);
//			hasChanged = true;
//		} 
//		else if (Input.GetKeyUp(KeyCode.Alpha1))
//		{
//			PlayerPrefs.SetInt("current_level", 1);
//			hasChanged = true;
//		} 
//		else if (Input.GetKeyUp(KeyCode.Alpha2))
//		{
//			PlayerPrefs.SetInt("current_level", 2);
//			hasChanged = true;
//		} 
//		else if (Input.GetKeyUp(KeyCode.Alpha3))
//		{
//			PlayerPrefs.SetInt("current_level", 3);
//			hasChanged = true;
//		} 
//		else if (Input.GetKeyUp(KeyCode.Alpha4))
//		{
//			PlayerPrefs.SetInt("current_level", 4);
//			hasChanged = true;
//		}
//		else if (Input.GetKeyUp(KeyCode.Alpha5))
//		{
//			PlayerPrefs.SetInt("current_level", 5);
//			hasChanged = true;
//		} 
//		else if (Input.GetKeyUp(KeyCode.Alpha6))
//		{
//			PlayerPrefs.SetInt("current_level", 6);
//			hasChanged = true;
//		} 
	}

	public bool HasChanged
	{
		get { return hasChanged; }
	}
}
