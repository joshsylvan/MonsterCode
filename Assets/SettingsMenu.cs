using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

	public void OnDeleteSaveButtonClick()
	{
		PlayerPrefs.SetInt("completed_level_0" , 0);
		PlayerPrefs.SetInt("completed_level_1" , 0);
		PlayerPrefs.SetInt("completed_level_2" , 0);
		PlayerPrefs.SetInt("completed_level_3" , 0);
		PlayerPrefs.SetInt("completed_level_4" , 0);
		PlayerPrefs.SetInt("completed_level_5" , 0);
		PlayerPrefs.SetInt("hard_mode", 0);
		PlayerPrefs.SetInt("current_level", 0);
		PlayerPrefs.SetInt("current_phase", 0);
		SceneManager.LoadScene("MenuNew");
	}
}
