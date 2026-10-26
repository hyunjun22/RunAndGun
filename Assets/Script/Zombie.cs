using UnityEngine;

public class Zombie : Enemy
{
    protected override void Act()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        Player player = collision.collider.GetComponentInParent<Player>();
        if (player == null) return;
        player.TakeDamage(attackDamage);
        lastAttackTime = Time.time;
    }
}
