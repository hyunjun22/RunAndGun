using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] StageData stage01;

    StageData selectedStage;

    Button stage01Button;
    Button enterButton;

    VisualElement infoPanel;

    Label stageNameLabel;
    Label descriptionLabel;
    Label enemiesLabel;
    Label rewardLabel;

    void OnEnable()
    {
        VisualElement root =
            GetComponent<UIDocument>().rootVisualElement;

        stage01Button = root.Q<Button>("Stage01Button");
        enterButton = root.Q<Button>("EnterButton");

        infoPanel = root.Q<VisualElement>("StageInfoPanel");

        stageNameLabel = root.Q<Label>("StageNameLabel");
        descriptionLabel = root.Q<Label>("StageDescriptionLabel");
        enemiesLabel = root.Q<Label>("StageEnemiesLabel");
        rewardLabel = root.Q<Label>("StageRewardLabel");

        if (stage01Button == null ||
            enterButton == null ||
            infoPanel == null ||
            stageNameLabel == null ||
            descriptionLabel == null ||
            enemiesLabel == null ||
            rewardLabel == null)
        {
            Debug.LogError("UI Builder의 Name을 확인하세요.");
            return;
        }

        infoPanel.style.display = DisplayStyle.None;

        stage01Button.clicked += SelectStage01;
        enterButton.clicked += EnterStage;
    }

    void OnDisable()
    {
        if (stage01Button != null)
            stage01Button.clicked -= SelectStage01;

        if (enterButton != null)
            enterButton.clicked -= EnterStage;
    }

    void SelectStage01()
    {
        if (stage01 == null)
        {
            Debug.LogError("Stage01.asset이 연결되지 않았습니다.");
            return;
        }

        selectedStage = stage01;

        stageNameLabel.text = selectedStage.stageName;
        descriptionLabel.text = selectedStage.description;

        rewardLabel.text =
            $"보상: {selectedStage.rewardGold}G";

        string enemyInfo = "";

        foreach (SpawnRule rule in selectedStage.spawnRules)
        {
            enemyInfo += $"{rule.enemyType} {rule.count}마리\n";
        }

        enemiesLabel.text = enemyInfo;

        infoPanel.style.display = DisplayStyle.Flex;
    }

    void EnterStage()
    {
        if (selectedStage == null) return;

        StageSession.SelectedStage = selectedStage;

        Time.timeScale = 1f;

        SceneManager.LoadScene("SampleScene");
    }
}