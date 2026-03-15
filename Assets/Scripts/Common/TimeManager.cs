using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timeRemaining = 600f;
    public TextMeshProUGUI timerText;

    public static TimerManager instance;

    private float timeElapsed;
    private float penaltyTime;

    public Color warningColor = Color.red;
    public float warningTime = 60f;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerUI();
            GameOver();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        if (timeRemaining <= warningTime)
        {
            timerText.color = warningColor;
        }

    }

    public void AddPenalty(float amount)
    {
        timeRemaining -= amount;
        penaltyTime += amount;

        if (timeRemaining < 0)
            timeRemaining = 0;
    }

    void GameOver()
    {
        Debug.Log("Time Up!");
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public float GetPenalty()
    {
        return penaltyTime;
    }
}
