using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CameraDetection : MonoBehaviour, IDataPersistence
{

    public Transform player;
    public float max_dist = 10f;
    public float fov = 60f;
    public AudioSource beep, detected;
    private int concealment = 0;
    private Coroutine loseDetection;
    private Coroutine addDetection;
    private bool alreadyDetected = false;

    public TextMeshProUGUI concealmentHUDText;
    public Image concealmentHUDImage;

    public TriggerLoud triggerLoud;

    void Start()
    {
        Debug.Log("incarcat");
        if (concealmentHUDText != null)
        {
            concealmentHUDText.text = "";
        }
    }
    public void LoadData(GameData data)
    {
        alreadyDetected = data.detected;
    }

    public void SaveData(ref GameData data)
    {
        data.detected = alreadyDetected;
    }
  void Update()
{
    if (concealment >= 100 || alreadyDetected)
        return;

    if (IsPlayerDetected())
    {
        HandleDetection();
    }
    else
    {
        HandleLostDetection();
    }
}

bool IsPlayerDetected()
{
    Vector3 directionPlayer = player.position - transform.position;
    float distToPlayer = directionPlayer.magnitude;
    if (distToPlayer > max_dist)
        return false;
    if (Vector3.Angle(transform.forward, directionPlayer) > fov / 2f)
        return false;
    RaycastHit hit;
    if (Physics.Raycast(transform.position, directionPlayer, out hit, max_dist))
    {
        return hit.transform == player;
    }
    return false;
}

void HandleDetection()
{
    addDetection ??= StartCoroutine(AddDetection());
    if (loseDetection != null)
    {
        StopCoroutine(loseDetection);
        loseDetection = null;
    }
}

void HandleLostDetection()
{
    loseDetection ??= StartCoroutine(LoseDetection());
    if (addDetection != null)
    {
        StopCoroutine(addDetection);
        addDetection = null;
    }
}

    private IEnumerator AddDetection()
    {
        while (!alreadyDetected)
        {
            yield return new WaitForSeconds(0.025f);
            concealment++;
            if (concealmentHUDText != null)
            {
                concealmentHUDText.text = ""+concealment;
            }
            concealmentHUDImage.fillAmount = concealment / 100f;
            float beepInterval = Mathf.Lerp(0.15f, 0.05f, concealment / 100f);
            if (concealment % Mathf.RoundToInt(beepInterval * 100) == 0)
            {
                beep.volume = 0.5f;
                beep.Play();
            }
            if (concealment >= 100)
            {
                Debug.Log("detectat");
                triggerLoud.forceTrigger();
                detected.volume = 0.1f;
                detected.Play();
                alreadyDetected = true;
                if (concealmentHUDText != null)
                {
                    concealmentHUDText.text = "Detected!";
                    yield return new WaitForSeconds(3f);
                    concealmentHUDText.text = "";
                    concealmentHUDImage.fillAmount = 0;
                }
            }
        }
    }
    private IEnumerator LoseDetection()
    {
        while (!alreadyDetected)
        {
            yield return new WaitForSeconds(0.03f);
            if (concealment > 0)
                concealment--;
            concealmentHUDImage.fillAmount = concealment / 100f;
            if (concealmentHUDText != null)
            {
                concealmentHUDText.text = concealment==0?"":concealment.ToString();
            }
            // if (concealment == 0)
            //     Debug.Log("doesnt eixst");
        }
    }
}