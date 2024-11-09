using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    private float health;
    public float maxHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if(health > 0) 
        { 
            health -= damage;
        }
        
        Component[] components = gameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++) 
        {
            Debug.Log(components[i].name);
        }
      
        gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(1,255), 0, 0);

        Debug.Log("Enemy hit: " +  health + "HP");
    }
}
