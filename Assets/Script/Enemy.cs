using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class Enemy : MonoBehaviour
{
    // Keep field names so existing Zombie prefab values survive the move to the base class.
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float hitStopTime = 0.2f;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float expReward = 20f;
    [SerializeField] protected Slider healthBar;
    protected float currentHealth;
    protected float lastAttackTime;
    protected bool isHit;
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected EnemyAnimation enemyAnimation;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    PlayerStats playerStats;
    Coroutine hitRoutine;

    public float CurrentHealth => currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        originalColor = spriteRenderer.color;
    }

    protected virtual void OnEnable()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player != null ? player.transform : null;
        playerStats = player != null ? player.GetComponent<PlayerStats>() : null;
        currentHealth = maxHealth;
        lastAttackTime = Time.time;
        rb.linearVelocity = Vector2.zero;
        spriteRenderer.color = originalColor;
        isHit = false;
        if (enemyAnimation != null) enemyAnimation.SetHitStop(false);
        UpdateHealthBar();
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        hitRoutine = null;
        isHit = false;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (enemyAnimation != null) enemyAnimation.SetHitStop(false);
    }

    protected virtual void FixedUpdate()
    {
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy || isHit)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Act();
    }

    protected abstract void Act();

    public void takeDamage(float damage)
    {
        if (!gameObject.activeInHierarchy || currentHealth <= 0f) return;
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        UpdateHealthBar();
        if (currentHealth <= 0f) { Die(); return; }
        if (hitRoutine != null) StopCoroutine(hitRoutine);
        hitRoutine = StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect()
    {
        isHit = true;
        rb.linearVelocity = Vector2.zero;
        if (enemyAnimation != null) enemyAnimation.SetHitStop(true);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(hitStopTime);
        spriteRenderer.color = originalColor;
        isHit = false;
        if (enemyAnimation != null) enemyAnimation.SetHitStop(false);
        hitRoutine = null;
    }

    protected virtual void Die()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddScore(1);
        if (playerStats != null) playerStats.AddExp(expReward);
        gameObject.SetActive(false);
    }

    void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = maxHealth > 0f ? currentHealth / maxHealth : 0f;
        healthBar.gameObject.SetActive(currentHealth < maxHealth);
    }
}
