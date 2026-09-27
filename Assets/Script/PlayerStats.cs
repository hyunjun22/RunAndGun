using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Attack")]
    [SerializeField] float damage = 10f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float bulletSpeed = 10f;


    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MoveSpeed => moveSpeed;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float BulletSpeed => bulletSpeed;

    public void TakeDamage(float amount)
    {
        // 체력 감소 (수치는 여기서)
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    public void RestartHealth()
    {
        currentHealth = maxHealth;
    }

    // 총알 데미지
    public void UpgradeDamage(float amount)
    {
        damage += amount;
    }

    // 공격 속도
    public void UpgradeAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    // 이동 속도
    public void UpgradeMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    // 체력
    public void UpgradeMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }

}
