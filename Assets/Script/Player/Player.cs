using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerUI playerUI;
    [SerializeField] PlayerStats playerStats;

    [SerializeField] float changeColorDelay = 0.1f;

    [Header("HP")]
    [SerializeField] Slider healthBar;
    [SerializeField] Slider damageBar;
    [SerializeField] float damageDelay = 0.5f;
    [SerializeField] float damageSpeed = 0.4f;
    private Coroutine damageRoutine;

    [Header("State UI")]
    [SerializeField] Text damageText;
    [SerializeField] Text attackSpeedText;
    [SerializeField] Text moveSpeedText;

    private Vector2 MovementInput;
    private Rigidbody2D rg;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    Color originalColor;

    public bool isInvincible = false;

    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetStateUI();
    }

    void OnEnable()
    {
        playerStats.RestartHealth();
        UpdateHPSlider();
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

        bool isMoving = MovementInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        if (!isMoving)
            return;

        if(Mathf.Abs(MovementInput.x) > Mathf.Abs(MovementInput.y))
        {
            // 좌우 이동
            if (MovementInput.x > 0)
                animator.SetInteger("Direction", 1); // 오른쪽
            else
                animator.SetInteger("Direction", 3); // 왼쪽
        }
        else
        {
            // 상하 이동
            if (MovementInput.y > 0)
                animator.SetInteger("Direction", 0); // 위쪽
            else
                animator.SetInteger("Direction", 2); // 아래쪽
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
            return;

        playerStats.TakeDamage(damage);
        UpdateHPSlider();

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

    void UpdateHPSlider()
    {
        float ratio = playerStats.CurrentHealth / playerStats.MaxHealth;

        healthBar.value = ratio;

        if (damageRoutine != null)
            StopCoroutine(DamageEffect());

        // 체력 회복 처리
        if (damageBar.value <= ratio)
        {
            damageBar.value = ratio;
            damageRoutine = null;
            return;
        }

        damageRoutine = StartCoroutine(DamageEffect());
    }

    IEnumerator DamageEffect()
    {
        yield return new WaitForSeconds(damageDelay);

        Debug.Log("DamageEffect play");

        while (damageBar.value > healthBar.value)
        {
            damageBar.value = Mathf.MoveTowards(damageBar.value, healthBar.value, damageSpeed * Time.deltaTime);

            yield return null;
        }

        damageRoutine = null;
    }

    void SetStateUI()
    {
        damageText.text = playerStats.Damage.ToString();
        attackSpeedText.text = playerStats.AttackSpeed.ToString("F2");
        moveSpeedText.text = playerStats.MoveSpeed.ToString();
    }
}