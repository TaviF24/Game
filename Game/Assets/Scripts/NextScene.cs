using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    public Vector3 pos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.instance.targetPosition = pos;
            SceneManager.instance.NextScene(sceneName);
		}
    }
}
