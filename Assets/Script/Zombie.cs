using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lifeTime = 10f;
    
    Transform playerTransform;
    Rigidbody2D rb;

    void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    // 오브젝트가 활성화 될 때
    void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 태그가 "Player"인 게임 오브젝트를 찾아서 Transform 컴포넌트를 가져옴
        
        StartCoroutine(LifeTimer()); // N초 뒤 비활성화 (향후 사망 처리 시점에 맞춰 없앨 것)
    }

    // 오브젝트가 비활성화 될 때
    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false); // 좀비를 비활성화
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
