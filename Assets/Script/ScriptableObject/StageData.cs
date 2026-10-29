using UnityEngine;
using System;

public enum EnemyType
{
    Zombie, Skeleton
}

[Serializable]
public class SpawnRule
{
    public EnemyType enemyType;

    [Min(1)] public int count = 1; // 몇 마리
    [Min(0f)] public float startDelay = 0f; // 소환 시작 시간
    [Min(0.1f)] public float interval = 1f; // 시간 간격
}

[CreateAssetMenu(fileName = "NewStage", menuName = "Stage/Stage Data")]
public class StageData : ScriptableObject
{
    public int stageId;
    public string stageName;

    [TextArea]
    public string description;

    public int rewardGold;

    public SpawnRule[] spawnRules;
}
