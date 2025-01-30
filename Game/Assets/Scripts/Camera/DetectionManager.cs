using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetectionManager : MonoBehaviour, IDataPersistence
{
    public static DetectionManager instance;
    public int concealment = 0;
    public int maxConcealment = 100;
    public bool alreadyDetected;

    public TextMeshProUGUI concealmentHUDText;
    public Image concealmentHUDImage;

    public int activeCameras = 0;

    public bool anticipation, assault;
    private float timeInAnticipation;
    public bool checkAfterReset = false;

    public void LoadData(GameData gameData)
    {
        if(instance != this) return;

        anticipation = gameData.anticipation;
        assault = gameData.assault;
        timeInAnticipation = gameData.timeInAnticipation;
        alreadyDetected = gameData.detected;
    }

    public void SaveData(ref GameData gameData)
    {
        if(instance != this) return;

        Debug.Log("saving" + anticipation + " " + assault + " " + timeInAnticipation);
        gameData.anticipation = anticipation;
        gameData.assault = assault;
        gameData.timeInAnticipation = timeInAnticipation;
        gameData.detected = alreadyDetected;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DetectionManager instance created.");
        }
        else if (instance != this)
        {
            Debug.Log("Duplicate DetectionManager instance destroyed.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (anticipation)
            timeInAnticipation += Time.deltaTime;
        check();
    }
    private void check()
    {
        //Debug.Log("Detected: " + alreadyDetected + " Anticipation" + anticipation + " Assault" + assault + " timeinanticipation" + timeInAnticipation);
        if (alreadyDetected)
        {
            if (!anticipation && !assault)
                StartAnticipation();
            else if (anticipation && !assault)
            {
                //Debug.Log("Anticipation");
                if(!checkAfterReset)
                {
                    //Debug.Log("Checked after reset");
                    checkAfterReset = true;
                    GameManager.instance.player.GetComponent<PlayerUI>().ActivateAnticipationHUD(true);
                    GameManager.instance.player.GetComponent<PlayerUI>().ActivateAssaultHUD(false);
                }    
                if (timeInAnticipation > 30f){
                    StartAssault();
                    checkAfterReset=true;
                }
                
            }
        }
        if (assault && !checkAfterReset)
        {
            GameManager.instance.player.GetComponent<PlayerUI>().ActivateAssaultHUD(true);
            GameManager.instance.player.GetComponent<PlayerUI>().ActivateAnticipationHUD(false);
        }
        
    }

    private void StartAnticipation()
    {
        anticipation = true;
        timeInAnticipation = 0f;
        Debug.Log("Starting Anticipation");
        GameManager.instance.player.GetComponent<PlayerUI>().ActivateAnticipationHUD(true);
        GameManager.instance.player.GetComponent<PlayerUI>().ActivateAssaultHUD(false);


        GeneralDetection.instance.TriggerAssault_GiveEnemyLastKnownPos();
    }

    private void StartAssault()
    {
        assault = true;
        anticipation = false;
        GameManager.instance.player.GetComponent<PlayerUI>().ActivateAssaultHUD(true);
        GameManager.instance.player.GetComponent<PlayerUI>().ActivateAnticipationHUD(false);

        GeneralDetection.instance.TriggerAssault_GiveEnemyLastKnownPos();
    }
    public void forceTrigger()
    {
        alreadyDetected = true;
        check();
    }

    public void updateGlobalConcealment(int val, AudioSource beep, AudioSource detected)
    {
        concealment = Mathf.Clamp(concealment + val, 0, maxConcealment);

        if (concealmentHUDText != null)
            concealmentHUDText.text = concealment == 0 ? "" : concealment.ToString();
        if (concealmentHUDImage != null)
            concealmentHUDImage.fillAmount = (float)concealment / maxConcealment;

        if (concealment >= maxConcealment && !alreadyDetected)
            Detected(detected);

        float beepInterval = Mathf.Lerp(0.15f, 0.05f, concealment / 100f);
        if (concealment % Mathf.RoundToInt(beepInterval * 100) == 0 && concealment != 0)
        {
            beep.volume = 0.5f;
            beep.Play();
        }
    }

    public void Detected(AudioSource detected)
    {
        alreadyDetected = true;
        detected.volume = 0.1f;
        detected.Play();
        forceTrigger();
        if (concealmentHUDText != null)
        {
            concealmentHUDText.text = "Detected";
            StartCoroutine(ClearDetectedText());
        }
    }

    public IEnumerator ClearDetectedText()
    {
        yield return new WaitForSeconds(3);
        if (concealmentHUDText != null)
            concealmentHUDText.text = "";
        if (concealmentHUDImage != null)
            concealmentHUDImage.fillAmount = 0;
    }

    public void registerCameraDetection()
    {
        activeCameras++;
    }

    public void unregisterCameraDetection()
    {
        activeCameras = Mathf.Max(0, activeCameras - 1);
    }

    public bool isAnyCameraDetecting()
    {
        return activeCameras > 0;
    }
}