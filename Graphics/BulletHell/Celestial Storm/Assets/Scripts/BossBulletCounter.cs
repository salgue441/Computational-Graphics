using TMPro;
using UnityEngine;

public class BossBulletCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bulletCountText;
    private Boss boss;

    public void SetBoss(Boss newBoss)
    {
        boss = newBoss;
    }

    private void Update()
    {
        if (bulletCountText != null && boss != null)
        {
            bulletCountText.text = "Boss bullet's: " + boss.BulletCount;
        }
    }
}
