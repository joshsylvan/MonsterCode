using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VSManager : MonoBehaviour
{
	private Animator anim;
	public Text levelText;
	public GameObject playerPreview, enemyPreview;
	private float counter = 1f;
	private float sceneCounter = 7f;
	
	// Use this for initialization
	void Start ()
	{
		Levels levels = new Levels();
		this.levelText.text = "Level " + (PlayerPrefs.GetInt("current_level") + 1);
		this.anim = GetComponent<Animator>();
		
		GameObject player = Resources.Load("Prefab/Monsters/" + PlayerPrefs.GetString("player_character")) as GameObject;
		player = Instantiate(player);
		Vector3 pos = player.transform.localPosition;
		player.transform.SetParent(playerPreview.transform);
		player.transform.localPosition = pos;
		
		GameObject enemy = Resources.Load("Prefab/Enemies/" + levels.GetLevel(PlayerPrefs.GetInt("current_level")).GetEnemyName()) as GameObject;
		enemy = Instantiate(enemy);
		Vector3 posE = player.transform.localPosition;
		enemy.transform.SetParent(enemyPreview.transform);
		enemy.transform.localPosition = posE;
		enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.y);


	}
	
	// Update is called once per frame
	void Update () {
		if (counter > 0)
		{
			counter -= Time.deltaTime;
		}
		else
		{
			anim.SetTrigger("Start");
			sceneCounter -= Time.deltaTime;
			if (sceneCounter < 0)
			{
//				PlayerPrefs.SetString("player_character", "Knight");
				SceneManager.LoadScene("GameNew");
			}
		}
	}
}
