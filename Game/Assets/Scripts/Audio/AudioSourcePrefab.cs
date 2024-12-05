using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePrefab : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
