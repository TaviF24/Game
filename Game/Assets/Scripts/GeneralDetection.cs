using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDetection : MonoBehaviour
{
    public static GeneralDetection instance { get; private set; }

    private GameObject player;
    private SpawningSystem spawningSystem;
    private float timeGiveEnemyLastKnownPosWhileAssault;
    private bool isAssault;

    private float timeSinceLastSpotted;

    [SerializeField] public float timerGiveEnemyLastKnownPosWhileAssault;
    [SerializeField] public float timerLastSpottedTimeout;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            timeGiveEnemyLastKnownPosWhileAssault = 0f;
            timeSinceLastSpotted = 0f;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        player = GameManager.instance.player;
        spawningSystem = FindObjectOfType<SpawningSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectionManager.instance.alreadyDetected)
        {
            if (timeGiveEnemyLastKnownPosWhileAssault > timerGiveEnemyLastKnownPosWhileAssault)
            {
                //Debug.Log("set to patrol STATE");
                timeGiveEnemyLastKnownPosWhileAssault = 0f;
                SetEnemiesToPatrolState();
            }

            if (timeSinceLastSpotted > timerLastSpottedTimeout)
            {
                //Debug.Log("CLEAR NOT SPOTTED in some time");
                //DetectionManager.instance.alreadyDetected = false;
            }

            timeGiveEnemyLastKnownPosWhileAssault += Time.deltaTime;
            timeSinceLastSpotted += Time.deltaTime;
        }
    }

    public void TriggerAssault_GiveEnemyLastKnownPos()
    {
        SetEnemiesToPatrolState();
        DetectionManager.instance.alreadyDetected = true;
        timeSinceLastSpotted = 0f;
    }

    public void SetEnemiesToPatrolState()
    {
        EnsureSpawningSystemIsInitialized();
        if (spawningSystem != null && spawningSystem.pool != null)
        {
            foreach (var enemy in spawningSystem.pool.getActiveObjects())
            {
                SetEnemyToSearchState(enemy);
            }
        }
    }

    private void EnsureSpawningSystemIsInitialized()
    {
        if (spawningSystem == null)
        {
            spawningSystem = FindObjectOfType<SpawningSystem>();
        }
    }

    private void SetEnemyToSearchState(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            StateMachine stateMachine = enemyScript.GetComponent<StateMachine>();
            if (stateMachine != null && stateMachine.activeState.GetType() != typeof(AttackState))
            {
                stateMachine.ChangeState(new SearchState());
                enemyScript.LastKnownPos = player.transform.position;
            }
        }
    }
}
