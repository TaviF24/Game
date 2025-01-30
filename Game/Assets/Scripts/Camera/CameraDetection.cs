using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CameraDetection : MonoBehaviour
{

    public Transform player;
    public float max_dist = 10f;
    public float fov = 75f;
    public AudioSource beep, detected;
    private Coroutine loseDetection;
    private Coroutine addDetection;

    private bool isDetectingPlayer = false;

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

public bool IsPlayerDetected()
{
    Vector3 directionPlayer = GameManager.instance.player.transform.position - transform.position;
    float distToPlayer = directionPlayer.magnitude;
    Debug.Log($"Distance to player: {distToPlayer}");
    if (distToPlayer > max_dist)
    {
        Debug.Log("Player is too far.");
        return false;
    }
    if (Vector3.Angle(transform.forward, directionPlayer) > fov / 2f)
    {
        Debug.Log("Player is out of FOV.");
        return false;
    }
    RaycastHit hit;
    if (Physics.Raycast(transform.position, directionPlayer, out hit, max_dist))
    {
        return hit.transform.root == GameManager.instance.player.transform.root;
    }
    Debug.Log("Raycast did not hit the player.");
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
            DetectionManager.instance.updateGlobalConcealment(1, beep, detected);
        }
    }

    private IEnumerator LoseDetection()
    {
        while (!DetectionManager.instance.alreadyDetected && !DetectionManager.instance.isAnyCameraDetecting())
        {
            yield return new WaitForSeconds(0.03f);
            DetectionManager.instance.updateGlobalConcealment(-1, beep, detected);
        }
    }

}