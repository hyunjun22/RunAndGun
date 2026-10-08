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
    [SerializeField] private Button DowngradeAttackDamageButton;


    void Start()
    {
        

        Setting();
    }

    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
        {
            developerPanel.SetActive(!developerPanel.activeSelf);
        }
    }

    void Setting()
    {
        developerPanel.SetActive(false);
        
        // invincibleToggle
        invincibleToggle.SetIsOnWithoutNotify(false);
        invincibleToggle.onValueChanged.AddListener(SetInvincible);

        // timeScaleSlider
        timeScaleSlider.value = Time.timeScale;
        timeScaleValueText.text = Time.timeScale.ToString("F2");
        timeScaleSlider.onValueChanged.AddListener(SetTimeScale);

        // experience
        experiencePlusButton.onClick.AddListener(() =>
        {
            playerStats.AddExp(100f);
        });

        // levelUp
        levelUpButton.onClick.AddListener(() =>
        {
            playerUI.ShowLevelUp();
        });

        // Health
        HealthPlusButton.onClick.AddListener(() => 
        {
            playerStats.Recovery(10f);
            playerUI.UpdateSetText();
            playerUI.UpdateHealthBar();
        });

        HealthMinusButton.onClick.AddListener(() =>
        {
            player.TakeDamage(10f);
            playerUI.UpdateSetText();
            playerUI.UpdateHealthBar();
        });

        // AttackDamage
        UpgradeAttackDamageButton.onClick.AddListener(() =>
        {
            playerStats.UpgradeDamage(10f);
            playerUI.UpdateSetText();
        });

        DowngradeAttackDamageButton.onClick.AddListener(() =>
        {
            playerStats.UpgradeDamage(-10f);
            playerUI.UpdateSetText();
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
