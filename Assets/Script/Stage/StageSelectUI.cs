using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageSelectUI : MonoBehaviour
{
    [Header("Stage Data")]
    [SerializeField] StageData[] stages;

    [Header("Stage Buttons")]
    [SerializeField] Transform stageButtonContainer;
    [SerializeField] Button stageButtonPrefab;

    [Header("Stage Info Panel")]
    [SerializeField] GameObject stageInfoPanel;
    [SerializeField] Text stageNameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text rewardGoldText;

    [Header("Enemy List")]
    [SerializeField] Transform enemyListContent;
    [SerializeField] Text enemyInfoTextPrefab;

    private StageData selectedStage;


    void Start()
    {
        stageInfoPanel.SetActive(false);

        CreateStageButtons();
    }

    void CreateStageButtons()
    {
        for(int i = 0; i < stages.Length; i++)
        {
            StageData stage = stages[i];

            if (stage == null)
                continue;

            Button button = Instantiate(stageButtonPrefab, stageButtonContainer);

            Text buttonText = button.GetComponentInChildren<Text>();

            if (buttonText != null)
                buttonText.text = "stage" + (i+1).ToString();

            // 버튼 클릭 이벤트 연결
            button.onClick.AddListener(() => SelectStage(stage));
        }
    }

    // 스테이지 선택
    void SelectStage(StageData stage)
    {
        selectedStage = stage;

        stageNameText.text = stage.stageName;
        descriptionText.text = stage.description;
        rewardGoldText.text = $"{stage.rewardGold} G";

        UpdateEnemyList();

        stageInfoPanel.SetActive(true);
    }

    // 몬스터 정보 목록 생성
    void UpdateEnemyList()
    {
        // 기존 Text 제거
        foreach (Transform child in enemyListContent)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        if (selectedStage.spawnRules == null)
            return;

        Dictionary<EnemyType, int> enemyCounts = new();
        
        foreach (SpawnRule rule in selectedStage.spawnRules)
        {
            if (enemyCounts.ContainsKey(rule.enemyType))
            {
                enemyCounts[rule.enemyType] += rule.count;
            }
            else
            {
                enemyCounts[rule.enemyType] = rule.count;
            }
        }

        foreach (var enemy in enemyCounts)
        {
            Text newText = Instantiate(enemyInfoTextPrefab, enemyListContent);

            string enemyName = enemy.Key switch
            {
                EnemyType.Zombie => "좀비",
                EnemyType.Skeleton => "스켈레톤",
                _ => enemy.Key.ToString()
            };

            newText.text = $"{enemyName} {enemy.Value}마리";
        }
    }

    public void CloseStageInfo()
    {
        stageInfoPanel.SetActive(false);
    }

    // 스테이지 입장
    public void EnterStage()
    {
        if (selectedStage == null)
            return;

        Debug.Log($"{selectedStage.stageName} 입장!");

        // 추후 StageManager와 연결
    }
}