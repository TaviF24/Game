using System;
using System.Collections;
using UnityEngine;

public class CameraPatorlling : MonoBehaviour
{

    private float state = 0;
    public Animator cameraAnimator;
    void Start()
    {
        float timeInterval = UnityEngine.Random.Range(9,15);
        StartCoroutine(ChangeState(timeInterval));
    }

    void Update()
    {
        
    }
    private IEnumerator ChangeState(float timeInterval)
    {
        while (true)
        {
            state = 0;
            yield return new WaitForSeconds(timeInterval);
            while(state == 0){
                state = Math.Sign(UnityEngine.Random.Range(-1f, 1f));
            }
            Debug.Log("state change to:"+state);
            cameraAnimator.SetInteger("state", (int)state);
            yield return new WaitForSeconds(timeInterval);
            state = 0;
            Debug.Log("state shoul;d reset to:"+state);
            cameraAnimator.SetInteger("state", (int)state);
        }
    }
}
