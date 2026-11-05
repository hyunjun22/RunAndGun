using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] StageSpawner stageSpawner;

    [Header("씬 직접 실행 테스트용")]
    [SerializeField] StageData testStageData;

    private StageData currentStage;

    IEnumerator Start()
    {
        // ObjectPoolingManager의 Start() 초기화가 끝나도록 대기
        yield return null;

        currentStage = StageSession.SelectedStage;

        // Battle 씬을 직접 실행했을 때 사용
        if (currentStage == null)
            currentStage = testStageData;

        if (currentStage == null)
        {
            Debug.LogError("StageData가 없습니다.");
            yield break;
        }

        Debug.Log($"{currentStage.stageName} 시작!");

        stageSpawner.StartSpawn(currentStage);
    }
}