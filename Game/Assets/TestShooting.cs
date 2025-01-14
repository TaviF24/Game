using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooting : Interactable, IDamageable
{
    Renderer renderer;
    Rigidbody rb;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        if (renderer != null)
        {
            renderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("No Renderer component found on this object!");
        }

        if (rb != null)
        {
            Vector3 randomForce = new Vector3(Random.Range(-10f, 10f), Random.Range(5f, 15f), Random.Range(-10f, 10f));
            rb.AddForce(randomForce, ForceMode.Impulse);
        }
    }

    protected override void Interact()
    {
        if (renderer != null)
        {
            renderer.material.color = new Color(255, 255, 255);
        }
        else
        {
            Debug.LogWarning("No Renderer component found on this object!");
        }
    }
}
