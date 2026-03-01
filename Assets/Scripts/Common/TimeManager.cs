using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timeRemaining = 600f;
    public TextMeshProUGUI timerText;
    public static TimerManager instance;
    private float timeElapsed;
private float penaltyTime;

    void Awake()
    {
        instance = this;

    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timeRemaining).ToString();
        }
        else
        {
            GameOver();
        }
    }

    public void AddPenalty(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }
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