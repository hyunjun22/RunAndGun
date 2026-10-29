using UnityEngine;

public class Skeleton : Enemy
{
    [Header("Ranged attack")]
    [SerializeField, Min(0f)] float stopDistance = 5f;
    [SerializeField, Min(0f)] float retreatDistance = 3.5f;
    [SerializeField, Min(0f)] float attackRange = 7f;
    [SerializeField, Min(0.1f)] float arrowSpeed = 7f;
    [SerializeField, Min(0f)] float muzzleOffset = 0.8f;
    ObjectPoolingManager poolManager;
    Animator animator;
    bool isShooting;
    bool arrowReleased;
    Vector2 shotDirection;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isShooting = false;
        arrowReleased = false;
        // Reusing an enemy during its shot must not resume the previous attack.
        animator.Rebind();
        animator.SetBool("IsShooting", false);
        animator.Play("Base Layer.Movement", 0, 0f);
        animator.Update(0f);
    }

    protected override void OnDisable()
    {
        isShooting = false;
        arrowReleased = false;
        base.OnDisable();
    }

    public void SetPoolManager(ObjectPoolingManager pool) { poolManager = pool; }

    protected override void Act()
    {
        if (isShooting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 delta = playerTransform.position - transform.position;
        float distance = delta.magnitude;
        Vector2 direction = distance > 0f ? delta / distance : Vector2.down;
        if (distance > stopDistance) rb.linearVelocity = direction * moveSpeed;
        else if (distance < retreatDistance) rb.linearVelocity = -direction * moveSpeed;
        else rb.linearVelocity = Vector2.zero;

        if (distance > attackRange || Time.time < lastAttackTime + attackCooldown || poolManager == null)
            return;
        if (!HasLineOfSight(direction, distance)) return;
        shotDirection = direction;
        arrowReleased = false;
        isShooting = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("ShootX", Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? Mathf.Sign(direction.x) : 0f);
        animator.SetFloat("ShootY", Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? 0f : Mathf.Sign(direction.y));
        animator.SetBool("IsShooting", true);
        lastAttackTime = Time.time;
    }

    // Called by the release-frame Animation Event on the four Shoot clips.
    public void ReleaseArrow()
    {
        if (!isShooting || arrowReleased || !gameObject.activeInHierarchy) return;
        arrowReleased = true;
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy || poolManager == null) return;
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance > attackRange || !HasLineOfSight(shotDirection, distance)) return;
        GameObject arrow = poolManager.GetArrow();
        if (arrow == null) return;
        arrow.GetComponent<EnemyArrow>().Launch(
            transform.position + (Vector3)(shotDirection * Mathf.Min(muzzleOffset, distance * 0.5f)),
            shotDirection, arrowSpeed, attackDamage);
    }

    // Called by the final-frame Animation Event; normal movement resumes afterward.
    public void EndShoot()
    {
        if (!isShooting) return;
        isShooting = false;
        animator.SetBool("IsShooting", false);
    }

    bool HasLineOfSight(Vector2 direction, float distance)
    {
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, direction, distance))
        {
            if (hit.collider.isTrigger || hit.collider.GetComponentInParent<Enemy>() != null ||
                hit.collider.GetComponentInParent<Player>() != null) continue;
            return false;
        }
        return true;
    }
}
