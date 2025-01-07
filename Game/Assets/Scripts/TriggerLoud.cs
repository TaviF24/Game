using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TriggerLoud : MonoBehaviour,IDataPersistence
{
    private bool detected,anticipation,assault;
    private float timeInAnticipation;
    public GameObject assaultHUD,anticipationHUD;
    public void LoadData(GameData gameData)
    {
        detected = gameData.detected;
        anticipation = gameData.anticipation;
        assault = gameData.assault;
        timeInAnticipation = gameData.timeInAnticipation;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.detected = detected;
        gameData.anticipation = anticipation;
        gameData.assault = assault;
        gameData.timeInAnticipation = timeInAnticipation;
    }
    void Update(){
        if(anticipation)
            timeInAnticipation += Time.deltaTime;
        check();
    }
    public void forceTrigger(){
        detected = true;
        check();
    }
    private void check()
    {
        Debug.Log("Detected: " + detected+" Anticipation"+anticipation+" Assault"+assault+" timeinanticipation"+timeInAnticipation);
        if (detected)
        {
            if (!anticipation && !assault)
            {
                StartAnticipation();
            }
            else if (anticipation && !assault)
            {
                if (timeInAnticipation > 30f)
                {
                    StartAssault();
                }
                else
                {
                    ShowHUD(anticipationHUD, assaultHUD);
                }
            }
        }
        if (assault)
        {
            ShowHUD(assaultHUD, anticipationHUD);
        }
    }

    private void StartAnticipation()
    {
        anticipation = true;
        timeInAnticipation = 0f;
        ShowHUD(anticipationHUD, assaultHUD);
    }

    private void StartAssault()
    {
        assault = true;
        anticipation = false;
        ShowHUD(assaultHUD, anticipationHUD);
    }

    private void ShowHUD(GameObject toEnable, GameObject toDisable)
    {
        if (!toEnable.activeSelf)
        {
            toEnable.SetActive(true);
        }
        if (toDisable.activeSelf)
        {
            toDisable.SetActive(false);
        }
    }

}
