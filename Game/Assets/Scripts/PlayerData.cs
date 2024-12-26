using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour, IDataPersistence
{
    public static PlayerData instance;

    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            //instance.GetComponentInChildren<ObjectPool>().DestroyBullets();
            Destroy(gameObject);
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        instance.GetComponentInParent<Transform>().position = SceneManager.instance.targetPosition;
        instance.GetComponentInChildren<ObjectPool>().Init();
    }
    public void LoadData(GameData gameData)
    {
        instance.GetComponentInParent<Transform>().position = gameData.playerPosition;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerPosition = instance.GetComponentInParent<Transform>().position;
    }
}
