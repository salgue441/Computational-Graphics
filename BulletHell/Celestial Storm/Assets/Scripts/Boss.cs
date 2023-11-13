using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 40f;

    private Coroutine currentRoutine;
    private readonly float attackInterval = 0.1f;
    private int bulletCount = 0;
    public int BulletCount
    {
        get { return bulletCount; }
    }

    private void Start()
    {
        StartCoroutine(Routines());
    }

    /// <summary>
    /// Selects the attack pattern and fires the bullets.
    /// </summary>
    /// <returns>The routine</returns>
    private IEnumerator Routines()
    {
        while (true)
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            yield return new WaitForSeconds(attackInterval);

            int randomAttack = Random.Range(0, 7);
            switch (randomAttack)
            {
                case 0:
                    currentRoutine = StartCoroutine(StraightShots());
                    break;

                case 1:
                    currentRoutine = StartCoroutine(CrossShots());
                    break;

                case 2:
                    currentRoutine = StartCoroutine(CircleShots());
                    break;

                case 3:
                    currentRoutine = StartCoroutine(SpiralShots());
                    break;

                case 4:
                    currentRoutine = StartCoroutine(WaveShots());
                    break;

                case 5:
                    currentRoutine = StartCoroutine(RandomBurstShots());
                    break;
            }

            yield return new WaitUntil(() => currentRoutine == null);
            yield return new WaitForSeconds(attackInterval);
        }
    }

    /// <summary>
    /// Fires the bullets in a straight line.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator StraightShots()
    {
        yield return RepeatShooting(10f, 0.5f, transform.forward);
        currentRoutine = null;
    }

    /// <summary>
    /// Fires the bullets in a double cross pattern.
    /// </summary>
    /// <returns>The coroutine</returns>
    private IEnumerator CrossShots()
{
    float duration = 10f;
    float interval = 0.05f; 
    float startTime = Time.time;

    while (Time.time - startTime < duration)
    {
        FireBullet(transform.forward);   
        FireBullet(-transform.forward);  
        FireBullet(transform.right);     
        FireBullet(-transform.right);    

        yield return new WaitForSeconds(interval);
    }

    currentRoutine = null;
}


    private IEnumerator CircleShots()
    {
        float duration = 10f;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float angle = 0f;
            while (angle < 360f)
            {
                FireBullet(Quaternion.Euler(0f, angle, 0f) * transform.forward * 2f);
                angle += 10f;
            }

            yield return new WaitForSeconds(0.5f);
        }

        currentRoutine = null;
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

        currentRoutine = null;
    }

    /// <summary>
    /// Shoots in a spiral pattern from the parent's center.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator SpiralShots()
    {
        float duration = 10f;
        float startTime = Time.time;
        float angle = 0f;
        float spiralRate = 5f; 

        while (Time.time - startTime < duration)
        {
            FireBullet(Quaternion.Euler(0f, angle, 0f) * transform.forward * 2f);
            angle += spiralRate;
            spiralRate += 0.1f; 

            yield return new WaitForSeconds(0.1f);
        }

        currentRoutine = null;
    }

    /// <summary>
    /// Shoots in a wave pattern from the parent's center.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator WaveShots()
    {
        float duration = 10f;
        float startTime = Time.time;
        float waveFrequency = 5f; 

        while (Time.time - startTime < duration)
        {
            for (float angle = 0; angle < 360; angle += 10)
            {
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward * 2f    ;

                direction += 2f * Mathf.Sin(Time.time * waveFrequency) * transform.up; 
                FireBullet(direction);
            }

            yield return new WaitForSeconds(0.5f);
        }

        currentRoutine = null;
    }

    /// <summary>
    /// Shoots in random directions from the parent's center.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator RandomBurstShots()
    {
        float duration = 10f;
        float startTime = Time.time;
        int burstCount = 20; 

        while (Time.time - startTime < duration)
        {
            for (int i = 0; i < burstCount; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                FireBullet(randomDirection);
            }

            yield return new WaitForSeconds(1f);
        }

        currentRoutine = null;
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

        Vector3 spawnOffset = direction.normalized * 2.0f;
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
