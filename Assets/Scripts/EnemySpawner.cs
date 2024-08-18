using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    StartManager startManager;
    CheatManager cheatManager;
    public GameObject enemyPrefab;   // Prefab of the enemy to spawn
    public float spawnInterval = 5f; // Interval between enemy spawns
    
    internal float spawnTimer;        // Timer to track when to spawn the next enemy
    private GameObject boss;         // Reference to the boss GameObject
    private BossHealth bossHealth;   // Reference to the BossHealth script on the boss

    void Start()
    {
        startManager = GameObject.Find("StartManager").GetComponent<StartManager>();
        cheatManager = GameObject.Find("CheatManager").GetComponent<CheatManager>();
        // Assuming boss reference is properly set elsewhere, like through GlobalReferences
        boss = GlobalReferences.Instance.boss;
        if (boss != null)
        {
            bossHealth = boss.GetComponent<BossHealth>(); // Get the BossHealth component
        }
        else

        spawnTimer = -3f;
    }

    void Update()
    {
        if(startManager.isStarted == false || startManager.isShowingPhases || cheatManager.isCheating)
        {
            return;
        }
        // Count down the timer
        spawnTimer -= Time.deltaTime;

        // Check if it's time to spawn a new enemy
        if (spawnTimer <= 0f && bossHealth != null && bossHealth.isBossAlive)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;  // Reset the timer
        }
    }

    void SpawnEnemy()
    {
        if (bossHealth == null || !bossHealth.isBossAlive)
        {
            return;
        }

        // Get boss's x position
        float bossX = boss.transform.position.x;

        // Instantiate a new enemy at the boss's x position and a specific y and z position
        Vector3 spawnPosition = new Vector3(bossX, 3f, 0f);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Attach the EnemyMovement script to the new enemy
        EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            // Initialize the enemy's movement parameters
            enemyMovement.InitializeMovement();
        }
        else
        {
            Debug.LogError("EnemyMovement component not found on the enemy prefab.");
        }
    }
}
