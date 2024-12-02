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
        if(health >= damage) 
        { 
            health -= damage;
        }
        else
        {
            health = 0;
        }
        
        //Component[] components = gameObject.GetComponents<Component>();
        //for (int i = 0; i < components.Length; i++) 
        //{
        //    Debug.Log(components[i].name);
        //}

        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        gameObject.GetComponent<Renderer>().material.color = randomColor;

        Debug.Log("Enemy hit: " +  health + "HP");
    }
}
