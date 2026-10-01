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


    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MoveSpeed => moveSpeed;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float BulletSpeed => bulletSpeed;

    [Header("Level")]
    [SerializeField] int level = 1;
    [SerializeField] float currentExp = 0f;
    [SerializeField] float requiredExp = 100f;

    public float CurrentExp => currentExp;
    public float RequiredExp => requiredExp;

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

    public void AddExp(float amount){
        currentExp += amount;
        
        playerUI.UpdateExpBar();

        if(currentExp >= requiredExp){
            LevelUp();
        }
    }

    void LevelUp(){
        currentExp -= requiredExp;

        level++;

        requiredExp *= 1.2f;

        // PlayerUI

        Time.timeScale = 0f;
    }

    void EndLevelUp(){
        playerUI.UpdateExpBar();
        Time.timeScale = 1f;
    }

}
