using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;
	public static bool isPaused = false;

	void Start()
	{
		pauseMenu.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
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

	public void PauseGame()
	{
		Cursor.lockState = CursorLockMode.None;
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
		isPaused = true;
		Debug.Log("Game Paused");
	}

	public void ResumeGame()
	{
		Cursor.lockState = CursorLockMode.Locked;
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
		Debug.Log("Game Resumed");
	}

	public void QuitGame()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
