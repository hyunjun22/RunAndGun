using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Text timeText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverText;


    float gameTime = 0f;
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
        ClickKeyboard();
    }

    void UpdateTimeText()
    {
        if(isPaused) return; // 일시정지일 때는 멈추기

        gameTime += Time.deltaTime;

        timeText.text = gameTime.ToString("F2");
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

    public void GameOver()
    {
        gameOverText.SetActive(true);
        Time.timeScale = 0f; // 게임 오버 시 게임 일시정지
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 재시작 시 시간 스케일을 원래대로
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
