using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolingManager ObjPoolManager; // 오브젝트 풀링을 위한 Object Pooling Manager 참조
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

        GameObject zombie = ObjPoolManager.GetZombie(); // 오브젝트 풀에서 좀비를 가져옴

        if (zombie != null)
        {
            zombie.transform.position = spawnPoint.position; // 좀비의 위치를 설정
            zombie.SetActive(true); // 좀비를 활성화
        }
    }
}
