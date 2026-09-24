using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] TMP_Text scoreText;


    float gameTime = 0f;
    int score = 0;


    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        gameTime += Time.deltaTime;

        timeText.text = "Time : " + gameTime.ToString("F2");
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score : " + score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
