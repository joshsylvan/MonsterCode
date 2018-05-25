using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterSelector : MonoBehaviour
{
	public LevelSelectMenu LevelSelectMenu;

	public GameObject preview;
	public Image fadeImage;
	public int selectionIndex = 0;
	public HardModeToggle hm;

	private bool fadeOut = false;
	private float fadeSpeed = 5f;
	
	// Use this for initialization
	void Start () {

		SetPreviewSprite(selectionIndex);
		fadeImage.color = new Color(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeOut)
		{
			float newAlpha = Mathf.Lerp(fadeImage.color.a, 1, Time.deltaTime*fadeSpeed);
			fadeImage.color = new Color(0, 0, 0, newAlpha);
			if (fadeImage.color.a >= 0.95f)
			{
				fadeOut = false;
				SceneManager.LoadScene("VS");
			}
		}
		
	}

	public void SetPreviewSprite(int index)
	{
		for (int i = 0; i < preview.transform.childCount; i++)
		{
			preview.transform.GetChild(i).gameObject.SetActive(false);
		}
		preview.transform.GetChild(index).gameObject.SetActive(true);	
	}

	public void OnRightArrowClick()
	{
		if (++selectionIndex >= preview.transform.childCount)
		{
			selectionIndex = 0;
		}
		SetPreviewSprite(selectionIndex);
	}
	
	public void OnLeftArrowClick()
	{
		if (--selectionIndex < 0)
		{
			selectionIndex = preview.transform.childCount-1;
		}
		SetPreviewSprite(selectionIndex);
	}

	public void StartGame()
	{
		switch (selectionIndex)
		{
			case 0:
				PlayerPrefs.SetString("player_character", "Skeleton");
				break;
			case 1:
				PlayerPrefs.SetString("player_character", "Undead");
				break;
			case 2:
				PlayerPrefs.SetString("player_character", "Knight");
				break;
			default:
				PlayerPrefs.SetString("player_character", "Knight");
				break;
		}

		if (!LevelSelectMenu.HasChangedLevel())
		{
			PlayerPrefs.SetInt("current_level", 0);
			PlayerPrefs.SetInt("current_phase", 0);
		}
		PlayerPrefs.SetInt("player_health", 3);
		PlayerPrefs.SetInt("enemy_health", 3);
//		SceneManager.LoadScene("GameNew");
		fadeOut = true;
	}

	public void OnClickExit()
	{
		Application.Quit();
	}
	
}
