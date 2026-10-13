using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        // 움직이지 않으면 Idle
        if (velocity.sqrMagnitude < 0.01f)
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
            return;
        }

        // 좌우 이동이 더 큰 경우
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            animator.SetFloat("MoveX", Mathf.Sign(velocity.x));
            animator.SetFloat("MoveY", 0f);
        }
        // 상하 이동이 더 큰 경우
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", Mathf.Sign(velocity.y));
        }
    }
}