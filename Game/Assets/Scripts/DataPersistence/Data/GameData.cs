using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public float playerHealth;
    public string lastScene;
    public bool detected, anticipation, assault;
    public float timeInAnticipation;

    public GameData()
    {
        playerPosition = Vector3.zero;
        lastScene = "InsideVan";
        playerHealth = 100f;
        detected = false;
        anticipation = false;
        assault = false;
        timeInAnticipation = 0f;
    }
}
