using UnityEngine;

public enum UpgradeType
{
    Damage, AttackSpeed, MoveSpeed, MaxHealth
}

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Upgrade/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public Texture icon;
    public string upgradeName;

    [TextArea]
    public string description;
    public UpgradeType type;
    public float value;
}
