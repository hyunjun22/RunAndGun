
using System.Collections;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField] ObjectPoolingManager poolManager;
    [SerializeField] Transform[] spawnPoints;

    public int RemainingEnemies {get; private set;}
    public bool isSpawningFinished { get; private set; }

    private int runningSpawnRoutines;

    public void StartSpawn(StageData stageData)
    {
        if (stageData == null || stageData.spawnRules == null)
        {
            Debug.LogError("StageData가 없습니다.");
            return;
        }
            
        if(poolManager == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("풀 매니저나 스폰포인트를 확인하세요.");
            return;
        }

        // 스테이지 전체 몬스터 수 계산
        foreach (SpawnRule rule in stageData.spawnRules)
        {
            if (rule == null)
                continue;

            RemainingEnemies += rule.count;
            runningSpawnRoutines++;
        }

        if(runningSpawnRoutines == 0)
        {
            isSpawningFinished = true;
            return;
        }

        // 소환 규칙 실행
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

        // 소환 규칙 하나 완료
        runningSpawnRoutines--;

        if (runningSpawnRoutines == 0)
        {
            isSpawningFinished = true;
            Debug.Log("모든 소환 규칙 완료");
        }
    }

    void SpawnEnemy(EnemyType type)
    {
        // Zombie Spawn 함수

        if (poolManager == null || spawnPoints == null || spawnPoints.Length == 0)
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

        Enemy enemyComponent = enemy.GetComponent<Enemy>();

        if(enemyComponent == null)
            Debug.LogError("Enemy 컴포넌트가 없습니다.");

        // Event
        enemyComponent.Died -= OnEnemyDied;
        enemyComponent.Died += OnEnemyDied;

        int index = Random.Range(0, spawnPoints.Length);

        enemy.transform.position = spawnPoints[index].position;
        enemy.SetActive(true);
    }

    void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;
        RemainingEnemies = Mathf.Max(0, RemainingEnemies - 1);
        Debug.Log($"남은 몬스터: {RemainingEnemies}");
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}

