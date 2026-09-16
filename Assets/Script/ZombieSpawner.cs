using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab; // 좀비 프리팹을 연결할 변수
    [SerializeField] private Transform[] spawnPoints; // 좀비가 생성될 위치를 지정할 배열
    [SerializeField] private float spawnInterval = 3f; // 좀비 생성 간격

    void Start()
    {
        InvokeRepeating(nameof(SpawnZombie), 1f, spawnInterval);
    }

    void SpawnZombie()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length); // 랜덤한 인덱스를 생성
        Transform spawnPoint = spawnPoints[randomIndex]; // 랜덤한 위치를 선택

        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity); // 좀비 생성
    }
}
