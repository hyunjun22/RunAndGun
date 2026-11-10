using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] StageSpawner stageSpawner;
    [SerializeField] StageData testStageData;

    [Header("UI")]
    [SerializeField] GameObject clearPanel;
    [SerializeField] Text remainingEnemiesText;
    [SerializeField] Text clearGoldText;

    private StageData currentStage;
    private bool isCleared;


    IEnumerator Start()
    {
        isCleared = false;

        if(clearPanel != null)
            clearPanel.SetActive(false);

        // ObjectPoolingManager의 Start() 초기화가 끝나도록 대기
        yield return null;

        currentStage = StageSession.SelectedStage;

        // Battle 씬을 직접 실행했을 때 사용
        if (currentStage == null && testStageData != null)
            currentStage = testStageData;

        // 예외 상황
        if (currentStage == null || currentStage.spawnRules == null || currentStage.spawnRules.Length == 0 || stageSpawner == null)
        {
            Debug.LogError("StageData가 없습니다.");
            yield break;
        }

        if(clearPanel != null)
            clearPanel.SetActive(false);

        stageSpawner.StartSpawn(currentStage);
    }

    void Update()
    {
        if (currentStage == null || stageSpawner == null || isCleared)
            return;

        // 일시정지 또는 게임오버 중에는 클리어 판정 안 함
        if(Time.timeScale == 0f)
            return;

        CountRemainingEnemies();
    }

    void CountRemainingEnemies()
    {
        // 남은 적 수를 세고, 스폰이 끝났을 때 남은 적 수가 0이면 스테이지 클리어 처리

        int remaining = stageSpawner.RemainingEnemies;

        if(remainingEnemiesText != null)
            remainingEnemiesText.text = $"{remaining}";

        if(stageSpawner.isSpawningFinished && remaining == 0)
        {
            ClearStage();
        }
    }

    void ClearStage(){

        // 1회만 실행
        if(isCleared)
            return;

        isCleared = true;

        GameProgress.AddGold(currentStage.rewardGold);

        if(clearGoldText != null)
            clearGoldText.text = $"+{currentStage.rewardGold} G";

        if(clearPanel != null)
            clearPanel.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("스테이지 클리어!");
    }

    public void ReturnToMain()
    {
        // Connect to Button
        Time.timeScale = 1f;
        StageSession.SelectedStage = null;
        SceneManager.LoadScene("Main");
    }
}