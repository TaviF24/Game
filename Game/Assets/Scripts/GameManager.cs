using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public AudioManager audioManager;
    public BackgroundMusicManager backgroundMusicManager;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else if (instance != this)
        //{
        //    Destroy(gameObject);
        //    //Debug.Log("destroyed");
        //}
        audioManager = GetComponent<AudioManager>();
        backgroundMusicManager = GetComponent<BackgroundMusicManager>();
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

}
