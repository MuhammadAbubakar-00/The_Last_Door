using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSequence : MonoBehaviour
{
    public List<GameObject> buttons;

    private List<int> pattern = new List<int>();
    private int inputIndex = 0;
    private bool isShowingPattern = false;
    public GameObject patternDisplay;

    void Start()
    {
        GeneratePattern();
        StartCoroutine(ShowPattern());
    }

    void GeneratePattern()
    {
        pattern.Clear();

        for (int i = 0; i < 4; i++)
            pattern.Add(Random.Range(0, 4));
    }

    IEnumerator ShowPattern()
    {
        isShowingPattern = true;

        yield return new WaitForSeconds(1);

        foreach (int index in pattern)
        {
            buttons[index].GetComponent<Animator>().SetTrigger("Flash");
            yield return new WaitForSeconds(0.6f);
        }

        inputIndex = 0;
        isShowingPattern = false;
    }

    public void PlayerInput(int id)
    {
        // ❗ Prevent input while pattern is showing
        if (isShowingPattern)
            return;

        if (id == pattern[inputIndex])
        {
            inputIndex++;

            if (inputIndex >= pattern.Count)
            {
                Level1Manager.instance.PuzzleSolved();
                patternDisplay.SetActive(false);
            }
        }
        else
        {
            // ❌ Wrong input
            FindObjectOfType<TimerManager>().AddPenalty(30f);

            inputIndex = 0;

            // 🔁 Show pattern again
           GeneratePattern();
           StartCoroutine(ShowPattern());
        }
    }
}