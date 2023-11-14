using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private KeyCode slowDownKey = KeyCode.LeftShift;
    [SerializeField] private float slowDownFactor = 0.5f;
    [SerializeField] private KeyCode shootKey = KeyCode.Space;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        GetInputs();
        MovePlayer();
        HandleTimeScale();
        HandleShooting();
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
    /// Instantiates and shoots a bullet from the player's position.
    /// </summary>
    private void ShootBullet()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
            {
                bulletRb.AddForce(transform.forward * bulletSpeed);
            }
        }
        else
        {
            Debug.LogError("Bullet prefab is not assigned!");
        }
    }
}
