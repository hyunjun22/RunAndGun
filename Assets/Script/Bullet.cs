using UnityEngine;

public class Bullet : MonoBehaviour
{
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
}
