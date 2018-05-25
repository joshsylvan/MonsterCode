using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
	private bool _hasChangedLevel;
	private Button[] _levelButtons;
	
	// Use this for initialization
	void Start ()
	{		
		_levelButtons = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Button>();
		_hasChangedLevel = false;
		for (int i = 0; i < _levelButtons.Length; i++)
		{
			_levelButtons[i].interactable = PlayerPrefs.GetInt("completed_level_" + i) != 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnLevelButtonClick(int level)
	{
		Debug.Log("SETTING LEVEL: " + level);
		SetLevel(level);
		_hasChangedLevel = true;
		Camera.main.GetComponent<CameraMenuMovement>().MoveToCreator();
	}

	public void SetLevel(int level)
	{
		PlayerPrefs.SetInt("current_level", level);
		PlayerPrefs.SetInt("current_phase", 0);
	}
	
	public bool HasChangedLevel()
	{
		return _hasChangedLevel;
	}
}
