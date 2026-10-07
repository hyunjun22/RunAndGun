using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DeveloperManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerUI playerUI;

    [Header("UI")]
    [SerializeField] private GameObject developerPanel;
    [SerializeField] private Toggle invincibleToggle;
    [SerializeField] private Slider timeScaleSlider;
    [SerializeField] private Text timeScaleValueText;
    [SerializeField] private Button experiencePlusButton;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button HealthPlusButton;
    [SerializeField] private Button HealthMinusButton;
    [SerializeField] private Button UpgradeAttackDamageButton;
    [SerializeField] private Button DowngradeAttackSpeedButton;


    void Start()
    {
        developerPanel.SetActive(false);
        timeScaleSlider.value = Time.timeScale;
        timeScaleValueText.text = Time.timeScale.ToString("F2");

        Setting();
    }

    void Setting()
    {
        invincibleToggle.onValueChanged.AddListener(SetInvincible);

        timeScaleSlider.onValueChanged.AddListener(SetTimeScale);

        experiencePlusButton.onClick.AddListener(() =>
        {
            playerStats.AddExp(100f);
        });

        levelUpButton.onClick.AddListener(() =>
        {
            playerUI.ShowLevelUp();
        });

        HealthMinusButton.onClick.AddListener(() =>
        {
            player.TakeDamage(10f);
        });

        UpgradeAttackDamageButton.onClick.AddListener(() =>
        {
            playerStats.UpgradeDamage(10f);
        });

        DowngradeAttackSpeedButton.onClick.AddListener(() =>
        {
            playerStats.UpgradeAttackSpeed(-0.1f);
        });
    }

    void SetInvincible(bool value)
    {
        player.isInvincible = value;
    }

    void SetTimeScale(float value)
    {
        Time.timeScale = value;

        timeScaleValueText.text = value.ToString("F2");
    }
}
