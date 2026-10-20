using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    bool isHitStopped = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isHitStopped = false;
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        if (isHitStopped) return;

        // �������� ������ Idle
        if (velocity.sqrMagnitude < 0.01f)
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
            return;
        }

        // �¿� �̵��� �� ū ���
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            animator.SetFloat("MoveX", Mathf.Sign(velocity.x));
            animator.SetFloat("MoveY", 0f);
        }
        // ���� �̵��� �� ū ���
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", Mathf.Sign(velocity.y));
        }
    }


    public void SetHitStop(bool value)
    {
        isHitStopped = value;
        animator.speed = value ? 0f : 1f;
    }

}