using UnityEngine;

/// <summary>
/// Handles the bullet movement.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    private Boss boss;

    /// <summary>
    /// Destroys the bullet after some time 
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    /// <summary>
    /// Sets the boss that fired this bullet.
    /// </summary>
    /// <param name="boss">Main boss of the game</param>
    public void SetBoss(Boss boss)
    {
        this.boss = boss;
    }

    /// <summary>
    /// Updates the position of the bullet.
    /// </summary>
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    /// <summary>
    /// Decreases the bullet count of the boss when the bullet is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (boss != null)
        {
            boss.DecreaseBulletCount();
        }
    }
}
