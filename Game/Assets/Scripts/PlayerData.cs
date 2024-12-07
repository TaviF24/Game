using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    private Vector3 pos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            //Debug.Log(this.transform.position);
            pos = this.transform.position + new Vector3(0,50,0);
            //Destroy(gameObject);
            //Debug.Log(instance.gameObject.transform.position);
        }
    }

    private void Start()
    {
        Debug.Log(instance.gameObject.transform.position);
        Debug.Log(instance.transform.position);
        instance.transform.position = pos;
    }
}
