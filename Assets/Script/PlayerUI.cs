using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Slider healthBar; // 체력바 UI

    
    // Player가 데미지를 입을 때 실행
    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = playerStats.CurrentHealth / playerStats.MaxHealth;
        }
    }
}
