using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    public Vector3 pos;
    public bool isLocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            SceneManager.instance.targetPosition = pos;
            SceneManager.instance.NextScene(sceneName);
        }
    }
}
