using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioSourcePrefab audioSourcePrefab;
    [SerializeField] int audioSourceCount;
    [Range(0, 1)] public float volume;
    
    public List<AudioSourcePrefab> audioSources;

    private void Awake()
    {
        //instance = this;
        //DontDestroyOnLoad(gameObject);
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

    void Start()
    {
        Init();
    }

    private void Init()
    {
        audioSources = new List<AudioSourcePrefab>();
        for(int i = 0; i < audioSourceCount; i++)
        {
            AudioSourcePrefab aS = Instantiate(audioSourcePrefab); 
            audioSources.Add(aS);
        }
    }

    public AudioSourcePrefab GetFreeAudioSourceFlag()
    {
        for (int i = 0;i < audioSourceCount; i++)
        {
            if(!audioSources[i].audioSource.isPlaying)
            {
                return audioSources[i];
            }
        }
        return audioSources[0];
    }
    
    public void Play(AudioClip audioClip)
    {
        AudioSource audioSource = GetFreeAudioSourceFlag().audioSource;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        AudioSource audioSource = GetFreeAudioSourceFlag().audioSource;
        PlayOneShot(audioClip, audioSource);
    }

    public void PlayOneShot(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.PlayOneShot(audioClip, volume);
    }

}
