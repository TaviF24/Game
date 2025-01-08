using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public AudioManager audioManager;
    public BackgroundMusicManager backgroundMusicManager;
    public GameObject overlayUI;

	private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        audioManager = GetComponent<AudioManager>();
        backgroundMusicManager = GetComponent<BackgroundMusicManager>();
        player = GameObject.FindWithTag("Player");
    }


	private void Update()
	{
		if (overlayUI.activeSelf)
		{
			// Disable game input
			return;
		}

		//if (Input.GetKeyDown(KeyCode.Tab))
		//{
		//	if (player.GetComponent<PauseMenu>().IsPaused())
		//	{
		//		player.GetComponent<PauseMenu>().ResumeGame();
		//	}
		//	else
		//	{
		//		player.GetComponent<PauseMenu>().PauseGame();
		//	}
		//}
		Debug.Log(Input.GetAxis("Mouse X"));
		Debug.Log(Input.GetAxis("Mouse Y"));
	}

	private void Start()
    {
        lockCursor();
	}

	public void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
	}

	public void unlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
}
