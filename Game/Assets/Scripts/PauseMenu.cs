using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;
	public static bool isPaused = false;

	// Start is called before the first frame update
	void Start()
	{
		pauseMenu.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (isPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
	}

	public bool IsPaused() { return isPaused; }

	public void PauseGame()
	{
		GameManager.instance.unlockCursor();
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
		isPaused = true;
		Debug.Log("Game Paused");
	}

	public void ResumeGame()
	{
		GameManager.instance.lockCursor();
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
		Debug.Log("Game Resumed");
	}

	public void GoToMainMenu()
	{

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
		Time.timeScale = 1f;
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
