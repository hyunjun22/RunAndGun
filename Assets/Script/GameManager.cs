using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject pausePanel;


    float gameTime = 0f;
    int score = 0;
    bool isPaused = false;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        UpdateTimeText();
        UpdateScoreText();
        ClickKeyboard();
    }

    void UpdateTimeText()
    {
        if(isPaused) return; // 일시정지일 때는 멈추기

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

    // 버튼에 연결할 함수
    public void ClickPauseButton()
    {
        if(isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    // 키보드 입력 처리
    void ClickKeyboard()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && Keyboard.current != null)
        {
            ClickPauseButton();
        }
    }
}
