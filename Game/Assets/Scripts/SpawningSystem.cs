using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    [SerializeField] public ObjectPool pool;
    [SerializeField] public float spawnDelay;
    [SerializeField] public int activeObjectsSimultaneously;
    private void Update()
    {
        if (CanSpawn())
        {
            GameObject objectToSpawn = pool.getFreeObject();
            if (objectToSpawn != null)
            {
                objectToSpawn.transform.position = transform.position;
                StartCoroutine(DelayedSpawn(objectToSpawn));
            }
        }
    }

    private IEnumerator DelayedSpawn(GameObject objectToSpawn)
    {
        yield return new WaitForSeconds(spawnDelay); 
        objectToSpawn.SetActive(true);
    }

    private bool CanSpawn()
    {
        return pool.getActiveObjectsNr() < activeObjectsSimultaneously;
    }
}
