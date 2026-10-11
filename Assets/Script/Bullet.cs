using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float maxDistance = 20f;

    Vector3 startPosition;
    int remainingPierces;
    readonly HashSet<Zombie> hitZombies = new HashSet<Zombie>();

    void OnEnable()
    {
        startPosition = transform.position;
        remainingPierces = 0;
        hitZombies.Clear();
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

        Zombie zombie = collision.GetComponentInParent<Zombie>();

        if(zombie != null && hitZombies.Add(zombie))
        {
            // 좀비가 맞다면
            // 총알 데미지를 좀비에게 가한다.
            zombie.takeDamage(damage);

            if (remainingPierces > 0)
                remainingPierces--;
            else
                gameObject.SetActive(false);
        }
    }
}
