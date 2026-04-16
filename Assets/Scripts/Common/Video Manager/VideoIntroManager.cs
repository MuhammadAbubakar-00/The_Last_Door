using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class VideoIntroManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private RawImage videoDisplay;
    [SerializeField] private Button skipButton;
    [SerializeField] private CanvasGroup videoCanvasGroup; // To fade the whole video UI
    [SerializeField] private GameObject gameplayUI; // The Oxygen Bar, etc.

    [Header("Settings")]
    [SerializeField] private float skipDelay = 3f;
    [SerializeField] private float fadeSpeed = 1.5f;

    [Header("Level References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private OxygenTimer oxygenTimer;

    private bool _isLevelStarted = false;

    void Awake()
    {
        // Initial State
        skipButton.gameObject.SetActive(false);
        videoCanvasGroup.alpha = 1;
        videoDisplay.gameObject.SetActive(true);
        if(gameplayUI) gameplayUI.SetActive(false);
        
        // Listen for video completion
        videoPlayer.loopPointReached += OnVideoReachedEnd;
    }

    public void PlayLevelIntro(VideoClip introClip)
    {
        _isLevelStarted = false;
        videoPlayer.clip = introClip;
        videoPlayer.Play();
        
        // Start the timer for the skip button
        StartCoroutine(ShowSkipButtonAfterDelay());
    }

    private IEnumerator ShowSkipButtonAfterDelay()
    {
        yield return new WaitForSeconds(skipDelay);
        
        // Only show skip if the video is still playing and level hasn't started
        if (videoPlayer.isPlaying && !_isLevelStarted)
        {
            skipButton.gameObject.SetActive(true);
            // Optional: Add a small fade-in for the button here
        }
    }

    // Triggered by the Skip Button "On Click"
    public void SkipIntro()
    {
        StartGameplay();
    }

    // Triggered automatically when video ends
    private void OnVideoReachedEnd(VideoPlayer vp)
    {
        StartGameplay();
    }

    private void StartGameplay()
    {
        if (_isLevelStarted) return;
        _isLevelStarted = true;

        StopAllCoroutines();
        StartCoroutine(TransitionOut());
    }

    private IEnumerator TransitionOut()
    {
        // 1. Hide Skip Button immediately
        skipButton.gameObject.SetActive(false);

        // 2. Fade out the video display
        float elapsed = 0;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            videoCanvasGroup.alpha = 1 - elapsed;
            yield return null;
        }

        // 3. Cleanup and Start Game
        videoPlayer.Stop();
        videoDisplay.gameObject.SetActive(false);
        
        if(gameplayUI) gameplayUI.SetActive(true);
        
        // Start the oxygen countdown
       if(oxygenTimer != null) oxygenTimer.StartTimer();
        
        Debug.Log("Gameplay Started!");
    }
}