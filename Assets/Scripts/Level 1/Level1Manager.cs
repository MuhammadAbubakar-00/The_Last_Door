using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public static Level1Manager instance;

    public int puzzlesCompleted = 0;
    public GameObject finalConsole;

    void Awake()
    {
        instance = this;
        finalConsole.SetActive(false);
    }

    public void PuzzleSolved()
    {
        puzzlesCompleted++;

        if (puzzlesCompleted >= 3)
        {
            finalConsole.SetActive(true);
            FinalScreenManager.Instance.ShowFinalScreen
            (
                true,
                TimerManager.instance.GetTimeElapsed(),
                TimerManager.instance.GetPenalty(),
                CalculateScore()
            );
        }
    }
    private int CalculateScore()
    {
        float time = TimerManager.instance.GetTimeElapsed();
        float penalty = TimerManager.instance.GetPenalty();

        int baseScore = 1000;
        int finalScore = baseScore - Mathf.RoundToInt(time * 5f) - Mathf.RoundToInt(penalty * 2f);

        return Mathf.Max(finalScore, 0);
    }
}