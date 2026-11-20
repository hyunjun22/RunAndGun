using UnityEngine;

[CreateAssetMenu(fileName = "StoreUpgrade", menuName = "Upgrade/Store Upgrade")]
public class StoreUpgradeData : ScriptableObject
{
    [Tooltip("Use a unique ID for each upgrade. Copies sharing an ID share their level.")]
    [SerializeField] string id;
    [SerializeField] string upgradeName;
    [SerializeField, TextArea] string description;
    [SerializeField] UpgradeType type;
    [SerializeField, Min(0.01f)] float increasePerLevel = 1f;
    [SerializeField, Min(0)] int cost = 100;

    public string Id => id;
    public string UpgradeName => upgradeName;
    public string Description => (description ?? "").Replace("{Value}", increasePerLevel.ToString("0.##"));
    public UpgradeType Type => type;
    public float IncreasePerLevel => increasePerLevel;
    public int Cost => cost;
    public bool IsValid => !string.IsNullOrWhiteSpace(id) && cost >= 0 && increasePerLevel > 0f &&
        (type == UpgradeType.Damage || type == UpgradeType.AttackSpeed || type == UpgradeType.MoveSpeed);
}
