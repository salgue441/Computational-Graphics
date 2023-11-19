using UnityEngine;
using TMPro;

/// <summary>
/// Handles the bullet counter UI.
/// </summary>
public class PlayerBulletCount : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI playerBulletCountText;

    /// <summary>
    /// Updates the bullet count text.
    /// </summary>
    private void Update()
    {
        if (player == null || playerBulletCountText == null)
            return;

        playerBulletCountText.text = "Player bullet's: " + player.BulletCount;
    }
}