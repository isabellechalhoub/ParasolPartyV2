using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    //public AudioSource clip1;
    //public AudioSource clip2;

    // Use this for initialization
    void Start () 
	{
        //clip2 = GameObject.FindGameObjectWithTag("LevelMusic").GetComponent<AudioSource>();
        //clip1 = GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<AudioSource>() ;
        //clip1.Stop();
        //clip2.Play();
        //clip1.time = 2.0f;
    }

	// Event handling when Restart button clicked
	public void RestartLevel()
	{
        //clip1.Stop();
        //clip2.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(Application.loadedLevel);
	}

	// Event handling when Exit button clicked - go to main menu
	public void ExitLevel()
	{
        //clip2.Stop();
        //clip1.Play();
        SceneManager.LoadScene(0);
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
        //clip1.Stop();
        //clip2.Play();
        SceneManager.LoadScene(1);
        //Application.LoadLevel(1);
	}
}
