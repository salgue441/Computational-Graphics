using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private KeyCode slowDownKey = KeyCode.LeftShift;
    [SerializeField] private float slowDownFactor = 0.5f;
    [SerializeField] private KeyCode shootKey = KeyCode.Space;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;

    private int bulletCount = 0;
    private int playerHealth = 5;
    public int BulletCount => bulletCount;
    public int PlayerHealth => playerHealth;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;

    /// <summary>
    /// Sets the rigidbody of the player to kinematic.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    /// <summary>
    /// Updates the player's movement and shooting.
    /// </summary>
    private void Update()
    {
        GetInputs();
        MovePlayer();
        HandleTimeScale();
        HandleShooting();
    }

    /// <summary>
    /// Checks if the player is being hit by an enemy bullet.
    /// </summary>
    /// <param name="other">The object that collided with the hitbox</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            playerHealth--;
            Destroy(other.gameObject);

            if (playerHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Gets the horizontal input from the player.
    /// </summary>
    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("horizontal");
        verticalInput = Input.GetAxis("vertical");
    }

    /// <summary>
    /// Moves the player horizontally.
    /// </summary>
    private void MovePlayer()
    { 
        Vector3 position = transform.position;

        float adjustedSpeed = moveSpeed / Time.timeScale;

        position.x += adjustedSpeed * horizontalInput * Time.deltaTime;
        position.y += adjustedSpeed * verticalInput * Time.deltaTime;

        transform.position = position;
    }

    /// <summary>
    /// Slows down the time scale when the player holds down the slow down key.
    /// </summary>
    private void HandleTimeScale()
    {
        if (slowDownKey != KeyCode.None && Input.GetKey(slowDownKey))
        {
            Time.timeScale = slowDownFactor;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Handles the shooting mechanism of the player.
    /// </summary>
    private void HandleShooting()
    {
        if (Input.GetKeyDown(shootKey))
        {
            ShootBullet();
        }
    }

    /// <summary>
    /// Instantiates and shoots a bullet from the player's position in the Y axis.
    /// </summary>
    private void ShootBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, transform.position,
            Quaternion.LookRotation(Quaternion.Euler(0f, 0f, 0f) * Vector2.up));
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bullet.GetComponent<Bullet>().SetPlayer(this);

        bulletRb.velocity = Vector3.up * bulletSpeed;
        bulletCount++;

        Destroy(bullet, 5f);

    }

    /// <summary>
    /// Resets the bullet count to 0.
    /// </summary>
    public void ResetBulletCount()
    {
        bulletCount = 0;
    }

    /// <summary>
    /// Decreases the bullet count.
    /// </summary>
    public void DecreaseBulletCount()
    {
        bulletCount--;
    }
}
