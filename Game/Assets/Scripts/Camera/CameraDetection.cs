using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CameraDetection : MonoBehaviour, IDataPersistence
{

    public Transform player;
    public float max_dist = 10f;
    public float fov = 75f;
    public AudioSource beep, detected;
    private Coroutine loseDetection;
    private Coroutine addDetection;
    public DetectionManager DetectionManager;



    private bool isDetectingPlayer = false;

    public void LoadData(GameData data)
    {
        DetectionManager.instance.alreadyDetected = data.detected;
    }

    public void SaveData(ref GameData data)
    {
        data.detected = DetectionManager.instance.alreadyDetected;
    }
    void Update()
    {
        if (DetectionManager.instance.concealment >= 100 || DetectionManager.instance.alreadyDetected)
            return;

        if (IsPlayerDetected())
        {
            if (!isDetectingPlayer)
            {
                isDetectingPlayer = true;
                DetectionManager.instance.registerCameraDetection();
            }
            HandleDetection();
        }
        else
        {
            if (isDetectingPlayer)
            {
                isDetectingPlayer = false;
                DetectionManager.instance.unregisterCameraDetection();
            }

            if (!DetectionManager.instance.isAnyCameraDetecting())
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
        
            return hit.transform == player;
        
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
        while (!DetectionManager.instance.alreadyDetected)
        {
            yield return new WaitForSeconds(0.025f);
            DetectionManager.instance.updateGlobalConcealment(1);
        }
    }

    private IEnumerator LoseDetection()
    {
        while (!DetectionManager.instance.alreadyDetected && !DetectionManager.instance.isAnyCameraDetecting())
        {
            yield return new WaitForSeconds(0.03f);
            DetectionManager.instance.updateGlobalConcealment(-1);
        }
    }

}