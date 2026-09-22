using UnityEngine;
using System.Collections;
using Unity.Jobs;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float hitStopTime = 0.2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    float currentHealth;
    float lastAttackTime;
    
    Transform playerTransform;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    Color originalColor;

    bool isHit = false; // 맞았는지 안맞았는지

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    // 오브젝트가 활성화 될 때
    void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 태그가 "Player"인 게임 오브젝트를 찾아서 Transform 컴포넌트를 가져옴
        
        currentHealth = maxHealth; // 활성화될 때 현재 체력을 설정

        spriteRenderer.color = originalColor;
        isHit = false;

        // Debug.Log("I am spawned but isHit is " + isHit);
    }

    // 오브젝트가 비활성화 될 때
    void OnDisable()
    {
        StopAllCoroutines();
    }

    void FixedUpdate()
    {
        if (playerTransform == null || isHit) return;

        MoveToPlayer();
    }

    // 플레이어를 따라가는 함수
    void MoveToPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized; // 플레이어와 좀비 사이의 방향 벡터 계산

        rb.linearVelocity = direction * moveSpeed; // 좀비의 속도를 설정하여 플레이어를 향해 이동
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;

        StartCoroutine(HitEffect());

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffect()
    {
        isHit = true;

        // 이동 정지
        rb.linearVelocity = Vector2.zero;

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(hitStopTime);

        spriteRenderer.color = originalColor;
        isHit = false;
    }

    void Die()
    {
        // 사망 시 비활성화
        gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 플레이어인 경우만 허용
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // 공격 쿨타임
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        Player playerCS = collision.gameObject.GetComponent<Player>();

        if(playerCS != null)
        {
            playerCS.TakeDamage(attackDamage);

            lastAttackTime = Time.time;
        }
    }
}
