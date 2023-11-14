using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;

    private Transform _transform;
    private readonly float attackInterval = 2f;
    private int bulletCount = 0;
    private bool isAttacking = false;
    
    public int BulletCount => bulletCount;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        _transform = transform;
        InvokeRepeating(nameof(Routines), 0f, attackInterval);
    }

    /// <summary>
    /// Sets the different attack routines for the boss
    /// </summary>
    private void Routines()
    {
        if (!isAttacking)
        {
            int randomAttack = Random.Range(1, 8);

            switch (randomAttack)
            {
                case 1: StartCoroutine(FlowerShots()); break;
                case 2: StartCoroutine(SpiralShots()); break;
                case 3: StartCoroutine(RandomShots()); break;
                case 4: StartCoroutine(WaveShots()); break;
                case 5: StartCoroutine(CircleShots()); break;
                case 6: StartCoroutine(GridShots()); break;
                case 7: StartCoroutine(HelixShots()); break;
            }
        }
    }

    /// <summary>
    /// Shoots in a spiral pattern from the parent's center.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator SpiralShots()
    {
        isAttacking = true;

        float duration = 10f;
        float startTime = Time.time;
        float angle = 0f;
        float angleIncrement = 5f;

        while (Time.time - startTime < duration)
        {
            FireBullet(Quaternion.Euler(0, 0, angle) * Vector2.up);
            angle += angleIncrement;

            yield return new WaitForSeconds(0.1f);
        }

        isAttacking = false;
    }

    /// <summary>
    /// Shoots in a wave pattern from the parent's center
    /// </summary>
    private IEnumerator WaveShots()
    {
        isAttacking = true;

        float duration = 10f;
        float startTime = Time.time;
        float waveFrequency = 0.5f;

        while (Time.time - startTime < duration)
        {
            for (float angle = 0; angle < 360; angle += 5)
            {
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
                direction += 2f * Mathf.Sin(Time.time * waveFrequency) * transform.up;

                FireBullet(direction);
            }

            yield return new WaitForSeconds(0.1f);
        }

        isAttacking = false;
    }

    /// <summary>
    /// Shoots in a wave pattern from the parent's center
    /// </summary>
    private IEnumerator RandomShots()
    {
        isAttacking = true;

        float duration = 10f;
        float startTime = Time.time;
        float waveFrequency = 0.5f;

        while (Time.time - startTime < duration)
        {
            for (float angle = 0; angle < 360;  angle += 5)
            {
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

                direction += Mathf.Sin(Time.time * waveFrequency) * Vector3.right * 5f;

                FireBullet(direction);
            }

            yield return new WaitForSeconds(0.1f);
        }

        isAttacking = false;
    }

    /// <summary>
    /// Produces flower shots with more petals.
    /// </summary>
    private IEnumerator FlowerShots()
    {
        isAttacking = true;
        int totalIterations = 100;
        float angleIncrement = 1f; 

        for (int i = 0; i < totalIterations; i += (int)angleIncrement)
        {
            // Original petals
            FireBullet(Quaternion.Euler(0, 0, i % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (i + 90) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (i + 180) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (i + 270) % 360) * Vector2.up);

            FireBullet(Quaternion.Euler(0, 0, -i % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (-i + 90) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (-i + 180) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (-i + 270) % 360) * Vector2.up);

            // Additional petals
            FireBullet(Quaternion.Euler(0, 0, (i + 45) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (i + 135) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (i + 225) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (i + 315) % 360) * Vector2.up);

            FireBullet(Quaternion.Euler(0, 0, (-i + 45) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (-i + 135) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (-i + 225) % 360) * Vector2.up);
            FireBullet(Quaternion.Euler(0, 0, (-i + 315) % 360) * Vector2.up);

            yield return new WaitForSeconds(0.1f);
        }

        isAttacking = false;
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

            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false;
    }

    /// <summary>
    /// Shoots in a grid pattern from the parent's center
    /// </summary>
    private IEnumerator GridShots()
    {
        isAttacking = true;

        float duration = 10f;
        float startTime = Time.time;
        float gridSpacing = 10f; 

        while (Time.time - startTime < duration)
        {
            for (float x = -30f; x <= 30f; x += gridSpacing)
            {
                for (float y = -30f; y <= 30f; y += gridSpacing)
                {
                    Vector3 direction = new Vector3(x, y, 0).normalized;
                    FireBullet(direction);
                }
            }

            yield return new WaitForSeconds(1f); 
        }

        isAttacking = false;
    }

    /// <summary>
    /// Shoots in a helix pattern from the parent's center
    /// </summary>
    private IEnumerator HelixShots()
    {
        isAttacking = true;

        float duration = 10f;
        float startTime = Time.time;
        float angle = 0f;
        float helixSpacing = 5f; 

        while (Time.time - startTime < duration)
        {
            FireBullet(Quaternion.Euler(0f, 0f, angle) * Vector2.up);
            FireBullet(Quaternion.Euler(0f, 0f, angle + 180f) * Vector2.up);
            FireBullet(Quaternion.Euler(0f, 0f, angle + 90f) * Vector2.up);
            FireBullet(Quaternion.Euler(0f, 0f, angle + 270f) * Vector2.up);

            angle += helixSpacing;

            yield return new WaitForSeconds(0.1f);
        }

        isAttacking = false;
    }



    /// <summary>
    /// Handles firing the bullets from the boss
    /// </summary>
    /// <param name="direction">Direction to shoot the bullet</param>
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
        bullet.GetComponent<Bullet>().SetBoss(this);

        bulletCount++;
    }

    /// <summary>
    /// Resets the bullet count.
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