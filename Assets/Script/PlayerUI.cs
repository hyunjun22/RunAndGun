using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework;
using System.Data;

public enum StatTextType { Damege, AttackSpeed, MovementSpeed, Health, All }

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
    
    [Header("LevelUp")]
    [SerializeField] private Slider expBar;
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private LevelUpPanel[] levelUpPanels;
    [SerializeField] List<UpgradeData> upgradeList;
    

    private void Start()
    {
        UpdateSetText();
        UpdateExpBar();
    }


    // Player가 데미지를 입을 때 실행
    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = playerStats.CurrentHealth / playerStats.MaxHealth;
            UpdateSetText(StatTextType.Health);
        }
    }

    public void UpdateExpBar(){
        expBar.value = playerStats.CurrentExp / playerStats.RequiredExp;
    }

    private void UpdateSetText(StatTextType type = StatTextType.All)
    {
        switch (type)
        {
            case StatTextType.Damege:
                damageText.text = playerStats.Damage.ToString();
                break;
            case StatTextType.AttackSpeed:
                attackSpeedText.text = playerStats.AttackSpeed.ToString();
                break;
            case StatTextType.MovementSpeed:
                movementSpeedText.text = playerStats.MoveSpeed.ToString();
                break;
            case StatTextType.Health:
                healthText.text = playerStats.CurrentHealth.ToString() + " / " + playerStats.MaxHealth.ToString();
                break;
            case StatTextType.All:
                damageText.text = playerStats.Damage.ToString();
                attackSpeedText.text = playerStats.AttackSpeed.ToString();
                movementSpeedText.text = playerStats.MoveSpeed.ToString();
                healthText.text = playerStats.CurrentHealth.ToString() + " / " + playerStats.MaxHealth.ToString();
                break;
        }
    }

    // 레벨업 창 띄우기
    public void ShowLevelUp()
    {
        levelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        CreateRandomOptions();
    }

    void CreateRandomOptions()
    {
        List<UpgradeData> tempList = new List<UpgradeData>(upgradeList);

        for (int i = 0; i < levelUpPanels.Length; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);

            UpgradeData selectedUpgrade = tempList[randomIndex];

            // 같은 강화가 나오지 않도록 제거
            tempList.RemoveAt(randomIndex);

            levelUpPanels[i].Setup(selectedUpgrade, SelectUpgrade);
        }

    }

    void SelectUpgrade(UpgradeData data)
    {
        ApplyUpgrade(data);

        UpdateSetText(); // UI 변환

        levelUpPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // 레벨업 적용하기
    void ApplyUpgrade(UpgradeData data)
    {
        switch (data.type)
        {
            case UpgradeType.Damage:
                playerStats.UpgradeDamage(data.value);
                break;

            case UpgradeType.AttackSpeed:
                playerStats.UpgradeAttackSpeed(data.value);
                break;

            case UpgradeType.MoveSpeed:
                playerStats.UpgradeMoveSpeed(data.value);
                break;

            case UpgradeType.MaxHealth:
                playerStats.UpgradeMaxHealth(data.value);
                break;
        }
    }
}
