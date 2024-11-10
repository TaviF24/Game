using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{   
    private List<GameObject> poolObjects = new List<GameObject>();
    public int size = 1;

    [SerializeField] GameObject objectFromPool;

    private void Start()
    {
        for (int i = 0; i < size; i++) {
            GameObject newObject = Instantiate(objectFromPool);
            newObject.SetActive(false);
            poolObjects.Add(newObject);
        }
    }

    public GameObject getFreeObject()
    {
        for(int i = 0; i < size; i++)
        {
            if (!poolObjects[i].activeInHierarchy)
            {
                return poolObjects[i];
            }
        }
        return null;
    }

}
