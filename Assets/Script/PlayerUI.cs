using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerStats playerStats;

    [Header("Stats")]
    [SerializeField] private Slider healthBar; // 체력바 UI
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private TMP_Text movementSpeedText;
    [SerializeField] private TMP_Text healthText;
    
    [Header("EXP")]
    [SerializeField] private Slider expBar;
    [SerializeField] private GameObject levelUpPanel;



    private void Start()
    {
        UpdateSetText("Damage");
        UpdateSetText("AttackSpeed");
        UpdateSetText("MovementSpeed");
        UpdateSetText("Health");
        UpdateExpBar();
    }


    // Player가 데미지를 입을 때 실행
    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = playerStats.CurrentHealth / playerStats.MaxHealth;
            UpdateSetText("Health");
        }
    }

    public void UpdateExpBar(){
        expBar.value = playerStats.CurrentExp / playerStats.RequiredExp;
    }

    private void UpdateSetText(string contents)
    {
        switch (contents)
        {
            case "Damage":
                damageText.text = playerStats.Damage.ToString();
                break;
            case "AttackSpeed":
                attackSpeedText.text = playerStats.AttackSpeed.ToString();
                break;
            case "MovementSpeed":
                movementSpeedText.text = playerStats.MoveSpeed.ToString();
                break;
            case "Health":
                healthText.text = playerStats.CurrentHealth.ToString() + " / " + playerStats.MaxHealth.ToString();
                break;
        }
    }
}
