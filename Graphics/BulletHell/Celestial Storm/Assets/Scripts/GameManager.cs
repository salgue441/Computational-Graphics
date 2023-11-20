using TMPro;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private int maxEnemies = 30;
    [SerializeField] private TextMeshProUGUI bulletCountText;
    [SerializeField] private TextMeshProUGUI enemiesCounter;
    [SerializeField] private BossBulletCounter bossBulletCounter;
    [SerializeField] private EnemyBulletCounter enemyBulletCounter;

    private int enemiesKilled = 0;
    private int currentEnemies = 0;
    private bool bossSpawned = false;
    private float enemySpawnInterval = 5f;
    private float speedMultiplier = 1f;
    private GameObject currentBoss;

    /// <summary>
    /// Starts the enemy spawning routine.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, enemySpawnInterval);
        UpdateEnemyCounter();

        // Hide the boss bullet count text
        if (bossBulletCounter != null)
        {
            bossBulletCounter.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Spawns an enemy at a random position on the screen.
    /// </summary>
    private void SpawnEnemy()
    {
        if (currentEnemies < maxEnemies && !bossSpawned)
        {
            float randomX = Random.Range(-200f, 200f);
            float randomY = Random.Range(10f, 100f);

            GameObject enemy = Instantiate(enemyPrefab, 
                new Vector3(randomX, randomY, 0), enemyPrefab.transform.rotation);
            enemy.GetComponent<Enemy>().SetSpeed(speedMultiplier);

            currentEnemies++;

            // Enemy bullet counter
            if (enemyBulletCounter != null)
            {
                enemyBulletCounter.gameObject.SetActive(true);
                enemyBulletCounter.SetEnemy(enemy.GetComponent<Enemy>());
            }
        }
    }

    /// <summary>
    /// Increments the number of enemies killed and adjusts the game difficulty.
    /// </summary>
    public void EnemyKilled()
    {
        if (enemiesKilled < maxEnemies)
        {
            enemiesKilled++;
        }

        if (currentEnemies > 0)
        {
            currentEnemies--;
        }

        UpdateEnemyCounter();
        AdjustGameDifficulty();

        if (enemiesKilled >= maxEnemies && !bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;

            CancelInvoke(nameof(SpawnEnemy));
        }
    }


    /// <summary>
    /// Updates the enemy counter displayed in the UI.
    /// </summary>
    private void UpdateEnemyCounter()
    {
        if (enemiesCounter != null)
        {
            int enemiesLeft = maxEnemies - enemiesKilled;
            enemiesCounter.text = "Enemies left: " + enemiesLeft;
        }
    }

    /// <summary>
    /// Adjusts the game difficulty by increasing the enemy spawn rate and speed.
    /// </summary>
    private void AdjustGameDifficulty()
    {
        enemySpawnInterval = Mathf.Max(1f, enemySpawnInterval - 0.1f);
        speedMultiplier += 1.5f;

        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), 0f, enemySpawnInterval);
    }

    /// <summary>
    /// Begins the boss fight.
    /// </summary>
    private void SpawnBoss()
    {
        currentBoss = Instantiate(bossPrefab, new Vector3(200f, 100f, 0), bossPrefab.transform.rotation);

        // Make the boss bullet count text visible
        if (bossBulletCounter != null)
        {
            bossBulletCounter.gameObject.SetActive(true);
        }

        // Boss bullet counter
        if (bossBulletCounter != null && currentBoss != null)
        {
            bossBulletCounter.SetBoss(currentBoss.GetComponent<Boss>());
        }

        // Hide the enemy bullet count text
        if (enemyBulletCounter != null)
        {
            enemyBulletCounter.gameObject.SetActive(false);
        }

        StartCoroutine(BossPhaseTimer());


    }

    /// <summary>
    /// Coroutine for handling the boss phase duration.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BossPhaseTimer()
    {
        yield return new WaitForSeconds(30f); // Wait for 30 seconds

        // Actions to perform after 30 seconds
        EndBossPhase();
    }

    /// <summary>
    /// Disables the boss and resumes the enemy spawning routine.
    /// </summary>
    private void EndBossPhase()
    {
        if (currentBoss != null)
        {
            Destroy(currentBoss);
        }

        // Hide the boss bullet count text
        if (bossBulletCounter != null)
        {
            bossBulletCounter.gameObject.SetActive(false);
        }

        bossSpawned = false;
        enemiesKilled = 0;
        UpdateEnemyCounter();

        // Resume spawning of regular enemies
        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), 0f, enemySpawnInterval);
    }
}
