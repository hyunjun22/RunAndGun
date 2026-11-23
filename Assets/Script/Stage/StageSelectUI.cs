using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    [Header("Stage Data")]
    [SerializeField] StageData[] stages;

    [Header("Stage Buttons")]
    [SerializeField] RectTransform stageButtonContainer;
    [SerializeField] Button stageButtonPrefab;

    [Header("Stage Info Panel")]
    [SerializeField] GameObject stageInfoPanel;
    [SerializeField] Text stageNameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text rewardGoldText;

    [Header("Store")]
    [SerializeField] GameObject storePanel;

    [Header("Gold")]
    [SerializeField] Text goldText;

    [Header("Enemy List")]
    [SerializeField] Transform enemyListContent;
    [SerializeField] Text enemyInfoTextPrefab;

    private StageData selectedStage;
    readonly List<(RectTransform From, RectTransform To, RectTransform Line)> stageConnections = new();

    void OnEnable()
    {
        GameProgress.GoldChanged += UpdateGoldText;
        UpdateGoldText(GameProgress.Gold);
    }

    void OnDisable()
    {
        GameProgress.GoldChanged -= UpdateGoldText;
    }

    void UpdateGoldText(int amount)
    {
        if (goldText != null) goldText.text = $"{amount:N0} G";
    }


    void Start()
    {
        stageInfoPanel.SetActive(false);
        if (storePanel != null) storePanel.SetActive(false);

        CreateStageButtons();
    }

    public void OpenStore()
    {
        if (storePanel != null) storePanel.SetActive(true);
    }

    public void CloseStore()
    {
        if (storePanel != null) storePanel.SetActive(false);
    }

    void CreateStageButtons()
    {
        RectTransform previousButton = null;
        foreach (StageData stage in stages)
        {
            if (stage == null)
                continue;

            Button button = Instantiate(stageButtonPrefab, stageButtonContainer);
            Text buttonText = button.GetComponentInChildren<Text>();

            if (buttonText != null)
                buttonText.text = string.IsNullOrWhiteSpace(stage.buttonLabel) ? stage.stageName : stage.buttonLabel;

            button.onClick.AddListener(() => SelectStage(stage));

            var buttonRect = (RectTransform)button.transform;
            if (previousButton != null)
                AddStageConnection(previousButton, buttonRect);

            previousButton = buttonRect;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(stageButtonContainer);
    }

    void AddStageConnection(RectTransform from, RectTransform to)
    {
        var lineObject = new GameObject("StageConnection", typeof(RectTransform),
            typeof(UnityEngine.UI.Image), typeof(LayoutElement));
        var line = (RectTransform)lineObject.transform;
        line.SetParent(stageButtonContainer, false);
        line.sizeDelta = Vector2.zero;
        line.SetAsFirstSibling();
        lineObject.GetComponent<LayoutElement>().ignoreLayout = true;

        var image = lineObject.GetComponent<UnityEngine.UI.Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f);
        image.raycastTarget = false;
        stageConnections.Add((from, to, line));
    }

    void LateUpdate()
    {
        foreach (var (from, to, line) in stageConnections)
        {
            Vector2 start = stageButtonContainer.InverseTransformPoint(from.TransformPoint(from.rect.center));
            Vector2 end = stageButtonContainer.InverseTransformPoint(to.TransformPoint(to.rect.center));
            Vector2 delta = end - start;
            line.localPosition = (start + end) * 0.5f;
            line.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
            line.sizeDelta = new Vector2(delta.magnitude, 5f);
        }
    }

    // 스테이지 선택
    void SelectStage(StageData stage)
    {
        CloseStore();
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

        StageSession.SelectedStage = selectedStage;

        Time.timeScale = 1f;

        SceneManager.LoadScene("BattleMap");
    }
}
