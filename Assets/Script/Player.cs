using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerUI playerUI;
    [SerializeField] PlayerStats playerStats;

    [SerializeField] float changeColorDelay = 0.1f;


    private Vector2 MovementInput;
    private Rigidbody2D rg;
    private SpriteRenderer spriteRenderer;

    Color originalColor;

    public bool isInvincible = false;

    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnEnable()
    {
        playerStats.RestartHealth();
        playerUI.UpdateHealthBar();
    }

    void FixedUpdate()
    {
        Move();
        
    }

    void Move()
    {
        rg.linearVelocity = MovementInput * playerStats.MoveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
            return;

        playerStats.TakeDamage(damage);
        playerUI.UpdateHealthBar();

        StartCoroutine(HitEffect());

        if(playerStats.CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver();
        
        gameObject.SetActive(false);
    }

    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(changeColorDelay);

        spriteRenderer.color = originalColor;
    }

    public void Recover(float amount)
    {
        
    }

}