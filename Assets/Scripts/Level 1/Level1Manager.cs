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
        }
    }
}