using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] PlayerUI playerUI;

    [Header("Health")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Attack")]
    [SerializeField] float damage = 10f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField, Min(0)] int pierceCount = 0;
    [SerializeField, Min(1)] int projectileCount = 1;
    [SerializeField, Min(0f)] float explosionDamage = 0f;


    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MoveSpeed => moveSpeed;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float BulletSpeed => bulletSpeed;
    public int PierceCount => pierceCount;
    public int ProjectileCount => Mathf.Max(1, projectileCount);
    public float ExplosionDamage => explosionDamage;

    public void UpgradeExplosionDamage(float amount)
    {
        explosionDamage = Mathf.Max(0f, explosionDamage + amount);
    }

    public void UpgradeProjectileCount(int amount)
    {
        projectileCount = Mathf.Max(1, projectileCount + amount);
    }

    public void UpgradePierceCount(int amount)
    {
        pierceCount = Mathf.Max(0, pierceCount + amount);
    }

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

    // 체력 회복
    public void Recovery(float amount)
    {
        if(maxHealth <= currentHealth + amount){
            currentHealth = maxHealth;
            return;
        }

        currentHealth += amount;
    }
}
