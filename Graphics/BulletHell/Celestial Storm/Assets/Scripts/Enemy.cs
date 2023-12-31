using UnityEngine;
using System.Collections;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private float moveSpeed = 45f;
    [SerializeField] private float movementChangeInterval = 5f;

    private Transform _transform;
    private GameManager gameManager;
    private int health = 3;
    private int bulletCount = 0;
    private Vector3 targetPosition;
    private float lastMovementChangeTime;
    private bool isAttacking = false;

    public int BulletCount => bulletCount;

    // Setters
    /// <summary>
    /// Sets the speed of the enemy
    /// </summary>
    public void SetSpeed(float speedMultiplier)
    {
        moveSpeed *= speedMultiplier;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Methods
    /// <summary>
    /// Sets the properties of the enemy object
    /// </summary>
    private void Start()
    {
        _transform = transform;
        InvokeRepeating(nameof(Attacks), 0f, 3f);
    }

    /// <summary>
    /// Updates the enemy's position
    /// </summary>
    private void Update()
    {
        MoveEnemy();
    }

    /// <summary>
    /// Checks if a "PlayerBullet" object collides with the hitbox
    /// </summary>
    /// <param name="other">The object that collided with the hitbox</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            health--;
            Destroy(other.gameObject);

            if (health <= 0)
            {
                gameManager.EnemyKilled();
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Moves the enemy randomly in the XY axis
    /// </summary>
    private void MoveEnemy()
    {
        if (Time.time - lastMovementChangeTime > movementChangeInterval)
        {
            SetRandomTargetPosition();
            lastMovementChangeTime = Time.time;
        }

        _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Sets a random position for the enemy to move to
    /// </summary>
    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-250f, 250f);
        float randomY = Random.Range(-150f, 150f);
        targetPosition = new Vector3(randomX, randomY, 0);
    }

    /// <summary>
    /// Sets the enemy's attack pattern
    /// </summary>
    private void Attacks()
    {
        if (!isAttacking)
        {
            int randomAttack = Random.Range(1, 3);

            switch (randomAttack)
            {
                case 1:
                    StartCoroutine(StraightShots());
                    break;

                case 2:
                    StartCoroutine(CircleShots());
                    break;
            }
        }
    }

    /// <summary>
    /// Shoots in a circle pattern from the parent's center
    /// </summary>
    private IEnumerator CircleShots()
    {
        isAttacking = true;

        float duration = 10f;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float angle = 0f;
            while (angle < 360f)
            {
                FireBullet(Quaternion.Euler(0f, 0f, angle) * transform.forward * 2f);
                angle += 10f;
            }

            yield return new WaitForSeconds(3f);
        }

        isAttacking = false;
    }


    /// <summary>
    /// Fires the bullets in a straight line.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator StraightShots()
    {
        isAttacking = true;

        yield return StartCoroutine(RepeatShooting(10f, 0.5f, Vector3.down));

        isAttacking = false;
    }

    /// <summary>
    /// Repeats the shooting for a certain duration and interval in a given direction.
    /// </summary>
    /// <param name="duration">Duration of the pattern</param>
    /// <param name="interval">Interval between bullets</param>
    /// <param name="directions">Direction of bullets</param>
    /// <returns></returns>
    private IEnumerator RepeatShooting(float duration, float interval,
        params Vector3[] directions)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            foreach (var direction in directions)
                FireBullet(direction);

            yield return new WaitForSeconds(interval);
        }

    }

    /// <summary>
    /// Fires a bullet in a given direction.
    /// </summary>
    /// <param name="direction">Direction of the bullet</param>
    private void FireBullet(Vector3 direction)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        Vector3 spawnOffset = direction.normalized * 20.0f;
        Vector3 spawnPosition = transform.position + spawnOffset;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition,
            Quaternion.LookRotation(direction));

        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

        bulletCount++;
        Destroy(bullet, 5f);
    }
}
