using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NumberPuzzle : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<TextMeshProUGUI> answerTexts; // assign button texts here

    private int correctAnswer;
[Header("Feedback")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private float vibrationDuration = 0.15f;
    [Header("Visual Feedback")]
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private Image panelBackground;   // background image of panel
    [SerializeField] private float flashDuration = 0.25f;

    void Start()
    {
        GeneratePuzzle();
    }
    void Update()
    {
        // ❌ Optional: Add a timer or other mechanics here
    }

    public void GeneratePuzzle()
    {
        int patternType = Random.Range(0, 3);

        int a = Random.Range(2, 6);
        int b, c, d, e;

        if (patternType == 0) // Multiply pattern
        {
            int multiplier = Random.Range(2, 4);
            b = a * multiplier;
            c = b * multiplier;
            d = c * multiplier;
            e = d * multiplier;

            correctAnswer = e;
        }
        else if (patternType == 1) // Addition pattern
        {
            int add = Random.Range(2, 6);
            b = a + add;
            c = b + add;
            d = c + add;
            e = d + add;

            correctAnswer = e;
        }
        else // Fibonacci
        {
            b = Random.Range(2, 6);
            c = a + b;
            d = b + c;
            e = c + d;

            correctAnswer = e;
        }

        questionText.text = a + "   " + b + "   " + c + "   " + d + "   ?";

        GenerateAnswers();
    }

    void GenerateAnswers()
    {
        List<int> answers = new List<int>();
        answers.Add(correctAnswer);

        while (answers.Count < 4)
        {
            int wrong = correctAnswer + Random.Range(-10, 10);

            if (!answers.Contains(wrong) && wrong > 0)
                answers.Add(wrong);
        }

        Shuffle(answers);

        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = answers[i].ToString();
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CheckAnswer(TextMeshProUGUI buttonText)
    {
        int selected = int.Parse(buttonText.text);

        if (selected == correctAnswer)
        {
            Level1Manager.instance.PuzzleSolved();
            gameObject.SetActive(false);
        }
        else
        {
            PlayWrongFeedback();

            TimerManager.instance.AddPenalty(30f);

            PlayWrongFeedback();
            StartCoroutine(FlashRed());

            // 🔁 Generate completely new puzzle
            StartCoroutine(Regenerate());
        }
    }

    IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(0.8f);
        GeneratePuzzle();
    }

    private void PlayWrongFeedback()
{
    // Play sound
    if (audioSource != null && wrongSound != null)
    {
        audioSource.PlayOneShot(wrongSound);
    }

    // Vibrate (mobile only)
#if UNITY_ANDROID || UNITY_IOS
    Handheld.Vibrate();
#endif
}

private IEnumerator FlashRed()
{
    if (panelBackground == null)
        yield break;

    Color originalColor = panelBackground.color;
    Color flashColor = new Color(1f, 0f, 0f, originalColor.a);

    float timer = 0f;

    // Fade to red
    while (timer < flashDuration)
    {
        timer += Time.deltaTime;
        panelBackground.color = Color.Lerp(originalColor, flashColor, timer / flashDuration);
        yield return null;
    }

    timer = 0f;

    // Fade back to original
    while (timer < flashDuration)
    {
        timer += Time.deltaTime;
        panelBackground.color = Color.Lerp(flashColor, originalColor, timer / flashDuration);
        yield return null;
    }

    panelBackground.color = originalColor;
}
}