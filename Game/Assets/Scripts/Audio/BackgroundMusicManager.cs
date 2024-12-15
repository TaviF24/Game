using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance;
    [SerializeField] AudioSource audioSource;
    [Range(0, 1)] public float volume;

    public List<AudioClip> clipList;

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
    }

    private void Start()
    {
        audioSource.clip = clipList[0];
        audioSource.volume = volume;
        audioSource.Play();
    }
    /*
     TO DO: 
        Play a specific sound on a specific scene
     */
}
