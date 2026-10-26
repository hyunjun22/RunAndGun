using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    bool isHitStopped;

    void Awake() { CacheComponents(); }

    void CacheComponents()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isHitStopped || rb == null || animator == null) return;
        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.01f)
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
        }
        else if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            animator.SetFloat("MoveX", Mathf.Sign(velocity.x));
            animator.SetFloat("MoveY", 0f);
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", Mathf.Sign(velocity.y));
        }
    }

    public void SetHitStop(bool value)
    {
        // Enemy may be enabled before this component's Awake during pool activation.
        CacheComponents();
        isHitStopped = value;
        if (animator != null) animator.speed = value ? 0f : 1f;
    }
}
