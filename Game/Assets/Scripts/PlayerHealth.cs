using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 100f;
	// controls how fast the delayed bar takes to reach the current health
	public float chipSpeed = 2f;
    public Image frontHealthBar;
	public Image backHealthBar;

	// Start is called before the first frame update
	void Start()
    {
		health = maxHealth;
	}

    // Update is called once per frame
    void Update()
    {
		health = Mathf.Clamp(health, 0, maxHealth);
		UpdateHealthUI();
		if (Input.GetKeyUp(KeyCode.T))
		{
			TakeDamage(10);
		}
		if (Input.GetKeyUp(KeyCode.G))
		{
			RestoreHealth(10);
		}
	}

	public void UpdateHealthUI()
	{       
		Debug.Log("Health: " + health);

		float fillFront = frontHealthBar.fillAmount;
		float fillBack = backHealthBar.fillAmount;

		float healthPercent = health / maxHealth;
		
		if (fillBack > healthPercent)
		{
			frontHealthBar.fillAmount = healthPercent;
			backHealthBar.color = Color.red;
			lerpTimer += Time.deltaTime;
			float percentCompleted = lerpTimer / chipSpeed;
			percentCompleted = percentCompleted * percentCompleted; // square the percentCompleted to make the lerp more smooth
			backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthPercent, percentCompleted);
		}

		if (fillFront < healthPercent)
		{
			backHealthBar.fillAmount = healthPercent;
			backHealthBar.color = Color.green;
			lerpTimer += Time.deltaTime;
			float percentCompleted = lerpTimer / chipSpeed;
			percentCompleted = percentCompleted * percentCompleted; // square the percentCompleted to make the lerp more smooth
			frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthPercent, percentCompleted);
		}
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		lerpTimer = 0;
		UpdateHealthUI();
	}

	public void RestoreHealth(float healAmount)
	{
		health += healAmount;
		lerpTimer = 0;
		UpdateHealthUI();
	}
}
