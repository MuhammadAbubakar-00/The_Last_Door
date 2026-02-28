using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColorSequence : MonoBehaviour
{
    public List<GameObject> buttons;

    private List<int> pattern = new List<int>();
    private int inputIndex = 0;
    private bool isShowingPattern = false;
    public GameObject patternDisplay;

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

        yield return new WaitForSeconds(2);

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
    PlayWrongFeedback();

    TimerManager.instance.AddPenalty(30f);

    inputIndex = 0;

    GeneratePattern();
    StartCoroutine(ShowPattern());
    PlayWrongFeedback();
    StartCoroutine(FlashRed());

}
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