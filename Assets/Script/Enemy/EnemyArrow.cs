using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyArrow : MonoBehaviour
{
    [SerializeField, Min(0.1f)] float lifetime = 5f;
    [SerializeField, Min(0.1f)] float maxDistance = 30f;
    Rigidbody2D rb;
    Vector3 startPosition;
    float damage;
    float elapsed;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }
    void OnEnable()
    {
        elapsed = 0f;
        damage = 0f;
        startPosition = transform.position;
        rb.linearVelocity = Vector2.zero;
    }

    public void Launch(Vector3 position, Vector2 direction, float speed, float attackDamage)
    {
        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        gameObject.SetActive(true);
        damage = attackDamage;
        rb.linearVelocity = direction.normalized * speed;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= lifetime || (transform.position - startPosition).sqrMagnitude >= maxDistance * maxDistance)
            gameObject.SetActive(false);
    }
    void OnDisable() { if (rb != null) rb.linearVelocity = Vector2.zero; }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeInHierarchy) return;
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            gameObject.SetActive(false);
            return;
        }
        if (other.isTrigger || other.GetComponentInParent<Enemy>() != null) return;
        gameObject.SetActive(false);
    }
}
