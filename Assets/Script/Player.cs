using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float changeColorDelay = 0.1f;

    [SerializeField] private Slider healthBar; // 체력바 UI

    private Vector2 MovementInput;
    private Rigidbody2D rg;
    private SpriteRenderer spriteRenderer;

    float currentHealth;
    Color originalColor;

    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        Move();
        
    }

    void Move()
    {
        rg.linearVelocity = MovementInput * moveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        UpdateHealthBar();

        StartCoroutine(HitEffect());

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(changeColorDelay);

        spriteRenderer.color = originalColor;
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

}