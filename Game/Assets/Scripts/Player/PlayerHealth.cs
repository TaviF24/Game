using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable, IDataPersistence
{
    private float health = 100f;
    private float lerpTimer; 
	[Header("Health Bar")]
    public float maxHealth = 100f; 
	// controls how fast the delayed bar takes to reach the current health
	public float chipSpeed = 2f;
    public Image frontHealthBar;
	public Image backHealthBar;

	[Header("Damage Effects")]
	public Image overlay;
	public float duration; // how long the overlay stays fully opaque
	public float fadeSpeed = 1.5f; // how fast the overlay fades out
	private float durationTimer;

    [Header("Death Screen")]
    public GameObject deathScreen;
    public float deathScreenDuration = 5f; // duration the death screen stays visible

    [Header("On Death Scene Transition")]
    public Vector3 targetPosition;
    public string nextScene;

    private PlayerShoot playerShoot;

	private float timeSinceLastDamage = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (health < 0) // Initialize only if health has not been set
        {
            health = maxHealth;
        }
        if (health < 30)
		{
			overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1f);
		}
		else
		{
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0f);
        }

        deathScreen.SetActive(false); // death screen is initially hidden
    }

	// Update is called once per frame
	void Update()
	{
		health = Mathf.Clamp(health, 0, maxHealth);
		UpdateHealthUI();

        timeSinceLastDamage += Time.deltaTime;
        //Debug.Log(timeSinceLastDamage);
        if (timeSinceLastDamage >= 30 && health < maxHealth)
        {
            //Debug.Log("autoheal");
            RestoreHealth(10);
            timeSinceLastDamage = 20;
        }

        if (overlay.color.a > 0)
		{
			// don't fade out the overlay if the player is low on health
			if (health < 30)
				return;

			durationTimer += Time.deltaTime;

			if (durationTimer > duration)
			{
				// fade out the overlay
				float tempAlpha = overlay.color.a;
				tempAlpha -= Time.deltaTime * fadeSpeed;
				fadeSpeed += Time.deltaTime * 1.1f;
				overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
			}
		}
    }

	public void UpdateHealthUI()
	{       
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
		timeSinceLastDamage = 0;
		lerpTimer = 0;
		UpdateHealthUI();
		durationTimer = 0;
		fadeSpeed = 1.5f;
		overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        if (health <= 0 && !deathScreen.activeInHierarchy)
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
		playerShoot = GetComponent<PlayerShoot>();
        //Time.timeScale = 0;
        playerShoot.BlockShooting(true);
        deathScreen.SetActive(true); // show death screen
        yield return new WaitForSeconds(deathScreenDuration);
		//Time.timeScale = 1;
        SceneManager.instance.targetPosition = targetPosition;
        SceneManager.instance.NextScene(nextScene);
        

        DetectionManager.instance.alreadyDetected = false;
        DetectionManager.instance.anticipation = false;
        DetectionManager.instance.assault = false;

        GameManager.instance.player.GetComponent<PlayerUI>().ActivateAnticipationHUD(false);
        GameManager.instance.player.GetComponent<PlayerUI>().ActivateAssaultHUD(false);

		gameObject.GetComponent<MoneyCollection>().money = 0;
		gameObject.GetComponent<MoneyCollection>().viewCount.text = 0 + " $";
		RestoreHealth(maxHealth);
        playerShoot.BlockShooting(false);
        deathScreen.SetActive(false); // hide death screen

    }

	public void RestoreHealth(float healthToRestore)
	{
		if (healthToRestore + health > maxHealth)
		{
			health = maxHealth;
		}
		else
		{
			health += healthToRestore;
		}
		lerpTimer = 0;
		UpdateHealthUI();
	}

    public void LoadData(GameData gameData)
    {
		health = gameData.playerHealth;
    }

    public void SaveData(ref GameData gameData)
    {
		gameData.playerHealth = health;
    }
}
