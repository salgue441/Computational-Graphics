using TMPro;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private Player player;

    private void Update()
    {
        playerHealthText.text = $"Player life: {player.PlayerHealth}";
    }
}
