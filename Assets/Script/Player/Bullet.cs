using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float maxDistance = 20f;
    [SerializeField, Min(0.1f)] float explosionRadius = 1.5f;

    Vector3 startPosition;
    int remainingPierces;
    float explosionDamage;
    ObjectPoolingManager poolManager;
    readonly HashSet<Enemy> explosionTargets = new HashSet<Enemy>();
    readonly List<Collider2D> explosionColliders = new List<Collider2D>();
    readonly HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

    void OnEnable()
    {
        startPosition = transform.position;
        remainingPierces = 0;
        hitEnemies.Clear();
        explosionDamage = 0f;
        poolManager = null;
        explosionTargets.Clear();
        explosionColliders.Clear();
    }

    // Gun 스크립트에서 쏠 때 데미지를 설정해줄 것
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetPierceCount(int count)
    {
        remainingPierces = Mathf.Max(0, count);
    }

    public void SetExplosion(float amount, ObjectPoolingManager pool)
    {
        explosionDamage = Mathf.Max(0f, amount);
        poolManager = pool;
    }

    void Explode(Vector3 position)
    {
        if (explosionDamage <= 0f) return;

        if (poolManager != null) poolManager.PlayExplosion(position);
        explosionTargets.Clear();
        explosionColliders.Clear();
        Physics2D.OverlapCircle(position, explosionRadius,
            new ContactFilter2D().NoFilter(), explosionColliders);
        foreach (Collider2D hit in explosionColliders)
        {
            if (hit == null) continue;
            Enemy target = hit.GetComponentInParent<Enemy>();
            if (target != null && target.gameObject.activeInHierarchy && explosionTargets.Add(target))
                target.takeDamage(explosionDamage);
        }
    }

    void Update()
    {
        // 둘 사이의 거리를 계산
        float distance = Vector3.Distance(startPosition, transform.position);

        // MaxDistance를 초과하면 비활성화
        if (distance >= maxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if(enemy != null && enemy.gameObject.activeInHierarchy && hitEnemies.Add(enemy))
        {
            // 좀비가 맞다면
            // 총알 데미지를 좀비에게 가한다.
            Vector3 explosionPosition = enemy.transform.position;
            enemy.takeDamage(damage);
            // 직격으로 적이 죽어도 해당 위치에서 폭발합니다.
            Explode(explosionPosition);

            if (remainingPierces > 0)
                remainingPierces--;
            else
                gameObject.SetActive(false);
        }
    }
}
