﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{

	public Image fadeImage, fadeInImage;
	public Text deathText;
	public GameObject player;
	private Animator playerAnim, menuAnim;

	private bool fadeIn;
	private float fadeSpeed = 2.5f;
	
	float animationCountdown = 2.5f;

	private Levels levels;


	private void Awake()
	{
		levels = new Levels();
		fadeImage.color = new Color(0, 0, 0, 1);
	}

	// Use this for initialization
	void Start ()
	{
		if (SceneManager.GetActiveScene().name == "Death")
		{
			GameObject playerObject = Resources.Load<GameObject>("Prefab/Monsters/" + PlayerPrefs.GetString("player_character"));
			playerObject = Instantiate(playerObject);
			Vector3 pos = playerObject.transform.localPosition;
			playerObject.transform.SetParent(player.transform);
			playerObject.transform.localPosition = pos;
			playerAnim = playerObject.GetComponent<Animator>();
			fadeIn = true;
			menuAnim = GetComponent<Animator>();
			deathText.text = "You died on level " + (PlayerPrefs.GetInt("current_level") + 1) + ".";
		} 
		else
		{
			GameObject playerObject = Resources.Load<GameObject>("Prefab/Enemies/" + (levels.GetLevel( PlayerPrefs.GetInt("current_level")-1).GetEnemyName() ));
			playerObject = Instantiate(playerObject);
			Vector3 pos = playerObject.transform.localPosition;
			playerObject.transform.SetParent(player.transform);
			playerObject.transform.localPosition = pos;
			playerAnim = playerObject.GetComponent<Animator>();
			fadeIn = true;
			menuAnim = GetComponent<Animator>();
			if (PlayerPrefs.GetInt("current_level") == levels.GetLevelCount())
			{
				deathText.text = "Amazing! You have slayed all of the monsters!\nYou have unlcocked 'Hard Mode'!";
			}
			else
			{
				deathText.text = "Victory! You completed level " + (PlayerPrefs.GetInt("current_level")) + ".";
			}
			PlayerPrefs.SetInt("completed_level_" + (PlayerPrefs.GetInt("current_level")-1), 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn)
		{
			float a = fadeImage.color.a;
			float newAlpha = Mathf.Lerp(a, 0, Time.deltaTime * fadeSpeed);
			fadeImage.color = new Color(0, 0, 0, newAlpha);
		}

		if (animationCountdown > 0)
		{
			animationCountdown -= Time.deltaTime;
			if (animationCountdown <= 0)
			{
				playerAnim.SetTrigger("Death");
			}
		}

		if (animationCountdown <= 0)
		{
			if (playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
			{
				float a = deathText.color.a;
				float newAlpha = Mathf.Lerp(a, 1, Time.deltaTime * fadeSpeed);
				if (SceneManager.GetActiveScene().name == "Death")
				{
					deathText.color = new Color(255, 255, 255, newAlpha);	
				}
				else
				{
					deathText.color = new Color(0, 0, 0, newAlpha);
				}
				if (deathText.color.a >= 0.95f)
				{
					StartCoroutine(FadeIn());
				}
			}
		}
	}

	IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(3f);
		StartCoroutine(LoadMenu());
	}
	
	IEnumerator LoadMenu()
	{
		float a = fadeInImage.color.a;
		float newAlpha = Mathf.Lerp(a, 1, Time.deltaTime * fadeSpeed);
		if (SceneManager.GetActiveScene().name == "Death")
		{
			fadeInImage.color = new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, newAlpha);
		}
		else
		{
			fadeInImage.color = new Color(0, 0, 0, newAlpha);
		}
		yield return new WaitForSeconds(1.5f);
		if (SceneManager.GetActiveScene().name == "Death")
		{
			SceneManager.LoadScene("MenuNew");	
		}
		else
		{
			if (PlayerPrefs.GetInt("current_level") == levels.GetLevelCount())
			{
				PlayerPrefs.SetInt("hard_mode", 1);
				SceneManager.LoadScene("MenuNew");
			}
			else
			{
				SceneManager.LoadScene("VS");
			}
		}
	}
}
