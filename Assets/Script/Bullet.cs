using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage = 20f;
    [SerializeField] float maxDistance = 20f;

    Vector3 startPosition;

    void OnEnable()
    {
        startPosition = transform.position;
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
        Zombie zombie = collision.gameObject.GetComponent<Zombie>();

        if(zombie != null)
        {
            // 좀비가 맞다면
            // 총알 데미지를 좀비에게 가한다.
            zombie.takeDamage(damage);

            gameObject.SetActive(false);
        }
    }
}
