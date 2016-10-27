using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	}

	// Event handling when Restart button clicked
	public void RestartLevel()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(Application.loadedLevel);
	}

	// Event handling when Exit button clicked - go to main menu
	public void ExitLevel()
	{
        SceneManager.LoadScene("TitleScreen");
        //Application.LoadLevel(0);
	}

	// Even handling for Exit game button on main menu
	public void ExitGame()
	{
		Application.Quit();
	}

	// Event handling for Play button on main menu - load level 1
	public void Play()
	{
        SceneManager.LoadScene("Level 1");
        //Application.LoadLevel(1);
	}
}
