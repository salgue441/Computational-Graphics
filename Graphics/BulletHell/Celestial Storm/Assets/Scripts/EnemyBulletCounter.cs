using UnityEngine;
using TMPro;

/// <summary>
/// Handles the bullet counter UI.
/// </summary>
public class EnemyBulletCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyBulletCounter;
    private Enemy enemy;

    public void SetEnemy(Enemy newEnemy)
    {
        enemy = newEnemy;
    }

    /// <summary>
    /// Updates the bullet count text.
    /// </summary>
    private void Update()
    {
        if (enemy == null || enemyBulletCounter == null)
            return;

        enemyBulletCounter.text = "Enemy bullet's: " + enemy.BulletCount;
    }
}