using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraDetection : MonoBehaviour
{
    public Transform player;
    public float max_dist = 10f;
    public float fov = 60f;
    private int concealment = 0;
    private Coroutine loseDetection;
    private Coroutine addDetection;
    private bool alreadyDetected = false;

    public TextMeshProUGUI concealmentHUDText;

    void Start()
    {
        Debug.Log("incarcat");
        if (concealmentHUDText != null)
        {
            concealmentHUDText.text = "0";
        }
    }

    void Update()
    {
        if (concealment >= 100)
            return;

        Vector3 directionPlayer = player.position - transform.position;
        float distToPlayer = directionPlayer.magnitude;
        if (distToPlayer <= max_dist)
        {
            if (Vector3.Angle(transform.forward, directionPlayer) <= fov / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionPlayer, out hit, max_dist))
                {
                    if (hit.transform == player)
                    {
                        if (addDetection == null)
                        {
                            addDetection = StartCoroutine(AddDetection());
                        }
                        if (loseDetection != null)
                        {
                            StopCoroutine(loseDetection);
                            loseDetection = null;
                        }
                        return;
                    }
                }
            }
        }

        if (loseDetection == null)
        {
            loseDetection = StartCoroutine(LoseDetection());
        }
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
            yield return new WaitForSeconds(0.03f);
            concealment++;
            if (concealmentHUDText != null)
            {
                concealmentHUDText.text = ""+concealment;
            }
            if (concealment >= 100)
            {
                Debug.Log("detectat");
                alreadyDetected = true;
                if (concealmentHUDText != null)
                {
                    concealmentHUDText.text = "detectat";
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
            if (concealmentHUDText != null)
            {
                concealmentHUDText.text = ""+concealment;
            }
            if (concealment == 0)
                Debug.Log("doesnt eixst");
        }
    }
}