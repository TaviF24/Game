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
        Init();
    }

    public void Init()
    {
        if(poolObjects.Count != 0)
        {
            DestroyBullets();
            poolObjects.Clear();
        }
        for (int i = 0; i < size; i++)
        {
            GameObject newObject = Instantiate(objectFromPool);
            newObject.SetActive(false);
            poolObjects.Add(newObject);
        }
    }

    public void DestroyBullets()
    {
        for (int i = 0; i < size; i++)
        {
            Destroy(poolObjects[i]);
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

    public int getActiveObjectsNr()
    {
        int cnt = 0;
        for (int i = 0; i < size; i++)
        {
            if (poolObjects[i].activeInHierarchy)
            {
                cnt++;
            }
        }
        return cnt;
    }

}
