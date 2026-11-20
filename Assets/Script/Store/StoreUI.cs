using UnityEngine;

public class StoreUI : MonoBehaviour
{
    [SerializeField] PlayerBaseStats baseStats;
    [SerializeField] GameObject descriptionPanel;
    [SerializeField] UnityEngine.UI.Button upgradeButton;
    [SerializeField] UnityEngine.UI.Button cancelButton;
    [SerializeField] UnityEngine.UI.Text nameText;
    [SerializeField] UnityEngine.UI.Text descriptionText;
    [SerializeField] UnityEngine.UI.Text recentStateText;
    [SerializeField] UnityEngine.UI.Text upgradeStateText;
    [SerializeField] UnityEngine.UI.Text spentGoldText;

    StoreUpgradeData selectedUpgrade;

    void Awake()
    {
        if (upgradeButton != null) upgradeButton.onClick.AddListener(PurchaseSelectedUpgrade);
        if (cancelButton != null) cancelButton.onClick.AddListener(CloseDescription);
    }

    void OnEnable()
    {
        GameProgress.GoldChanged += OnGoldChanged;
        CloseDescription();
    }

    void OnDisable()
    {
        GameProgress.GoldChanged -= OnGoldChanged;
        CloseDescription();
    }

    void OnDestroy()
    {
        if (upgradeButton != null) upgradeButton.onClick.RemoveListener(PurchaseSelectedUpgrade);
        if (cancelButton != null) cancelButton.onClick.RemoveListener(CloseDescription);
    }

    public void ShowUpgrade(StoreUpgradeData data)
    {
        if (data == null || !data.IsValid || baseStats == null || !gameObject.activeInHierarchy) return;
        selectedUpgrade = data;
        RefreshDescription();
        descriptionPanel.SetActive(true);
    }

    public void CloseDescription()
    {
        selectedUpgrade = null;
        if (descriptionPanel != null) descriptionPanel.SetActive(false);
        if (upgradeButton != null) upgradeButton.interactable = false;
    }

    public void PurchaseSelectedUpgrade()
    {
        if (selectedUpgrade == null || descriptionPanel == null || !descriptionPanel.activeInHierarchy) return;
        GameProgress.BuyUpgrade(selectedUpgrade);
        RefreshDescription();
    }

    void OnGoldChanged(int amount) { RefreshDescription(); }

    void RefreshDescription()
    {
        if (selectedUpgrade == null || baseStats == null) return;
        float current = baseStats.GetStat(selectedUpgrade.Type) + GameProgress.GetStatBonus(selectedUpgrade.Type);
        nameText.text = selectedUpgrade.UpgradeName;
        descriptionText.text = selectedUpgrade.Description;
        recentStateText.text = current.ToString("0.##");
        upgradeStateText.text = (current + selectedUpgrade.IncreasePerLevel).ToString("0.##");
        spentGoldText.text = $"{selectedUpgrade.Cost:N0} G";
        upgradeButton.interactable = selectedUpgrade.IsValid && GameProgress.Gold >= selectedUpgrade.Cost;
    }
}
