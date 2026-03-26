using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TutorialSlide
{
    [TextArea(3, 10)] public string instructionText;
    public AudioClip voiceOverClip;
}

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject nextPromptIcon; // An arrow or "Tap to Continue" icon

    [Header("Content")]
    [SerializeField] private List<TutorialSlide> slides;
    [SerializeField] private float typeSpeed = 0.04f;
    
    [Header("External References")]
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private OxygenTimer oxygenTimer;

    private int _currentSlideIndex = 0;
    private bool _isTyping = false;
    private bool _canMoveToNext = false;

    void Start()
    {
        StartTutorial();
        if(nextPromptIcon) nextPromptIcon.SetActive(false);
        
        nextButton.onClick.AddListener(HandleNextButtonClick);
        nextButton.interactable = false; // Disable until voice/text finishes
    }

    void Update()
    {
        // STEP 4: AUTO-NEXT / READINESS POLISH
        // Only allow "Next" if the voice has finished and text is fully typed
        if (tutorialPanel.activeSelf && !voiceSource.isPlaying && !_isTyping && !_canMoveToNext)
        {
            _canMoveToNext = true;
            nextButton.interactable = true;
            if(nextPromptIcon) nextPromptIcon.SetActive(true);
            
            // Optional: You could call ShowNextSlide() here automatically 
            // if you don't want the user to have to click "Next".
        }
    }

    public void StartTutorial()
    {
        tutorialPanel.SetActive(true);
        _currentSlideIndex = 0;
        DisplaySlide();
    }

    private void DisplaySlide()
    {
        if (_currentSlideIndex < slides.Count)
        {
            _canMoveToNext = false;
            nextButton.interactable = false;
            if(nextPromptIcon) nextPromptIcon.SetActive(false);

            // Play Voice
            voiceSource.Stop();
            voiceSource.clip = slides[_currentSlideIndex].voiceOverClip;
            voiceSource.Play();

            // Start Typewriter
            StopAllCoroutines();
            StartCoroutine(TypeText(slides[_currentSlideIndex].instructionText));
        }
        else
        {
            EndTutorial();
        }
    }

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        displayText.text = "";
        
        foreach (char letter in text.ToCharArray())
        {
            displayText.text += letter;
            // Short delay per letter
            yield return new WaitForSeconds(typeSpeed);
        }
        
        _isTyping = false;
    }

    public void HandleNextButtonClick()
    {
        if (_canMoveToNext)
        {
            _currentSlideIndex++;
            DisplaySlide();
        }
    }

    private void EndTutorial()
    {
        tutorialPanel.SetActive(false);
        if(oxygenTimer != null) oxygenTimer.StartTimer();
        Debug.Log("Tutorial Finished. Oxygen Depleting!");
    }
}