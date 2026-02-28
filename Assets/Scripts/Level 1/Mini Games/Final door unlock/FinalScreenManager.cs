using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalScreenManager : MonoBehaviour
{
    public static FinalScreenManager Instance;

    [Header("Panel")]
    [SerializeField] private GameObject finalPanel;

    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text penaltyText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowFinalScreen(bool success, float timeTaken, float penalty, int score)
    {
        finalPanel.SetActive(true);

        titleText.text = success ? "SYSTEM RESTORED" : "SYSTEM FAILURE";

        scoreText.text = "Score: " + score.ToString();
        timeText.text = "Time: " + timeTaken.ToString("F1") + "s";
        penaltyText.text = "Penalty: " + penalty.ToString("F0") + "s";

        Time.timeScale = 0f; // freeze game
    }

    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}