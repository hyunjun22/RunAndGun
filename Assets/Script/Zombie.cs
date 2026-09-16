using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    
    Transform playerTransform;
    Rigidbody2D rb;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 태그가 "Player"인 게임 오브젝트를 찾아서 Transform 컴포넌트를 가져옴
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    // 플레이어를 따라가는 함수
    void MoveToPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized; // 플레이어와 좀비 사이의 방향 벡터 계산

        rb.linearVelocity = direction * moveSpeed; // 좀비의 속도를 설정하여 플레이어를 향해 이동
    }
}
