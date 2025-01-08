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
    public bool alreadyDetected = false;

    public TextMeshProUGUI concealmentHUDText;
    public Image concealmentHUDImage;

    public AudioSource beep, detected;
    public int activeCameras = 0;

    private bool anticipation, assault;
    private float timeInAnticipation;
    public GameObject assaultHUD, anticipationHUD;
    public bool checkAfterReset = false;

    public void LoadData(GameData gameData)
    {
        if(instance != this) return;

        anticipation = gameData.anticipation;
        assault = gameData.assault;
        timeInAnticipation = gameData.timeInAnticipation;
    }

    public void SaveData(ref GameData gameData)
    {
        if(instance != this) return;

        Debug.Log("saving" + anticipation + " " + assault + " " + timeInAnticipation);
        gameData.anticipation = anticipation;
        gameData.assault = assault;
        gameData.timeInAnticipation = timeInAnticipation;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DetectionManager instance created.");
        }
        else
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
        Debug.Log("Detected: " + alreadyDetected + " Anticipation" + anticipation + " Assault" + assault + " timeinanticipation" + timeInAnticipation);
        if (alreadyDetected)
        {
            if (!anticipation && !assault)
                StartAnticipation();
            else if (anticipation && !assault)
            {
                Debug.Log("Anticipation");
                if(!checkAfterReset)
                {
                    Debug.Log("Checked after reset");
                    checkAfterReset = true;
                    ShowHUD(anticipationHUD, assaultHUD);
                }    
                if (timeInAnticipation > 30f){
                    StartAssault();
                    checkAfterReset=true;
                }
                
            }
        }
        if (assault && !checkAfterReset)
            ShowHUD(assaultHUD, anticipationHUD);
        
    }

    private void StartAnticipation()
    {
        anticipation = true;
        timeInAnticipation = 0f;
        Debug.Log("Starting Anticipation");
        ShowHUD(anticipationHUD, assaultHUD);
    }

    private void StartAssault()
    {
        assault = true;
        anticipation = false;
        ShowHUD(assaultHUD, anticipationHUD);
    }
    public void forceTrigger()
    {
        alreadyDetected = true;
        check();
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
    public void updateGlobalConcealment(int val)
    {
        concealment = Mathf.Clamp(concealment + val, 0, maxConcealment);

        if (concealmentHUDText != null)
            concealmentHUDText.text = concealment == 0 ? "" : concealment.ToString();
        if (concealmentHUDImage != null)
            concealmentHUDImage.fillAmount = (float)concealment / maxConcealment;

        if (concealment >= maxConcealment && !alreadyDetected)
            Detected();

        float beepInterval = Mathf.Lerp(0.15f, 0.05f, concealment / 100f);
        if (concealment % Mathf.RoundToInt(beepInterval * 100) == 0 && concealment != 0)
        {
            beep.volume = 0.5f;
            beep.Play();
        }
    }

    public void Detected()
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

    private IEnumerator ClearDetectedText()
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