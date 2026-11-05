
using System.Collections;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField] ObjectPoolingManager poolManager;
    [SerializeField] Transform[] spawnPoints;

    public void StartSpawn(StageData stageData)
    {
        if (stageData == null || stageData.spawnRules == null)
            return;

        foreach (SpawnRule rule in stageData.spawnRules)
        {
            if (rule == null)
                continue;

            StartCoroutine(SpawnRoutine(rule));
        }
    }

    IEnumerator SpawnRoutine(SpawnRule rule)
    {
        // 몬스터 소환 시작 시간까지 대기
        yield return new WaitForSeconds(rule.startDelay);

        for (int i = 0; i < rule.count; i++)
        {
            SpawnEnemy(rule.enemyType);

            // 마지막 소환 후에는 기다리지 않음
            if (i < rule.count - 1)
                yield return new WaitForSeconds(rule.interval);
        }
    }

    void SpawnEnemy(EnemyType type)
    {
        if (poolManager == null ||
            spawnPoints == null ||
            spawnPoints.Length == 0)
            return;

        GameObject enemy = type switch
        {
            EnemyType.Zombie => poolManager.GetZombie(),
            EnemyType.Skeleton => poolManager.GetSkeleton(),
            _ => null
        };

        if (enemy == null)
        {
            Debug.LogWarning($"{type} 소환 실패: 풀 확인 필요");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);

        enemy.transform.position = spawnPoints[index].position;
        enemy.SetActive(true);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}

