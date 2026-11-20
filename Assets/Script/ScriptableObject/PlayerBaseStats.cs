using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "Player/Base Stats")]
public class PlayerBaseStats : ScriptableObject
{
    [SerializeField, Min(0f)] float damage = 10f;
    [SerializeField, Min(0.01f)] float attackSpeed = 1f;
    [SerializeField, Min(0f)] float moveSpeed = 5f;

    public float GetStat(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.Damage => damage,
            UpgradeType.AttackSpeed => attackSpeed,
            UpgradeType.MoveSpeed => moveSpeed,
            _ => 0f
        };
    }
}
