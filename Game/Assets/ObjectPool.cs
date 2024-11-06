using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    
    private List<GameObject> poolObjects = new List<GameObject>();
    public int size = 1;

    [SerializeField] GameObject objectFromPool;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

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
