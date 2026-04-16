using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class ARSceneVideoManager : MonoBehaviour
{
    public static ARSceneVideoManager Instance;

    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CanvasGroup fadeOverlay;

    void Awake()
    {
        // Singleton pattern: This object will stay alive across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchLevel(string sceneName, VideoClip transitionVideo)
    {
        StartCoroutine(LevelTransitionSequence(sceneName, transitionVideo));
    }

    private IEnumerator LevelTransitionSequence(string sceneName, VideoClip clip)
    {
        // 1. Fade to Black
        yield return StartCoroutine(Fade(1));

        // 2. Start Video
        videoPlayer.clip = clip;
        videoPlayer.Play();

        // 3. Fade out Black to show video
        yield return StartCoroutine(Fade(0));

        // 4. Wait for video to finish (or almost finish)
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // 5. Fade to Black again to hide scene loading
        yield return StartCoroutine(Fade(1));

        // 6. Load the actual Unity Scene
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        while (!loading.isDone)
        {
            yield return null;
        }

        // 7. Fade back into the new Chemistry Lab Level
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeOverlay.alpha;
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            fadeOverlay.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / 0.5f);
            yield return null;
        }
        fadeOverlay.alpha = targetAlpha;
    }

    public void SkipVideo(string sceneName)
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(sceneName);
    }
}