using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveProgressManager : MonoBehaviour
{

    [Header("File Storage Config")]
    private string fileName = "game.data";

    private GameData gameData;
    public static SaveProgressManager instance;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
            //{
            DontDestroyOnLoad(gameObject);
            //}
            
        }
        else if (instance != this) 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();        
    }

    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        gameData = fileDataHandler.Load();

        if(gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Player load pos: " + gameData.playerPosition);
        Debug.Log("detected?" + gameData.detected);
    }
    public void SaveGame()
    {

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);

        Debug.Log("Player load pos: " + gameData.playerPosition);
        Debug.Log("detected?" + gameData.detected);
    }

    private void OnApplicationQuit()
    {
        try
        {
            SaveGame();
        }
        catch (Exception) { }
        
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
