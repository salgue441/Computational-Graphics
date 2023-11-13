using UnityEngine;
using TMPro;

/// <summary>
/// Handles the bullet counter UI.
/// </summary>
public class BulletCount : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private TextMeshProUGUI bulletCountText;

    /// <summary>
    /// Updates the bullet count text.
    /// </summary>
    private void Update()
    {
        if (boss == null || bulletCountText == null)
            return;

        bulletCountText.text = "Bullets: " + boss.BulletCount;
    }
}