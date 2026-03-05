using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public bool LoadSceneOnStart = false;
    public float WaitToLoadSceneOnStart = 0;
    public int SceneIndex = 0;

    public float DelayToLoadScene = 0;

    void Start()
    {
        if (LoadSceneOnStart)
        {
            StartCoroutine(LoadScene_OnStart());
        }
    }
    public void Load_Scene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartCurrentSceneAfterDelay()
    {
        StartCoroutine(RestartSceneAfterDelayCoroutine());
    }

    private IEnumerator RestartSceneAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(DelayToLoadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeSceneAfterDelay(int sceneIndex)
    {
        StartCoroutine(ChangeSceneAfterDelayCoroutine(sceneIndex));
    }

    private IEnumerator ChangeSceneAfterDelayCoroutine(int sceneIndex)
    {
        yield return new WaitForSeconds(DelayToLoadScene);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadScene_OnStart()
    {
        yield return new WaitForSeconds(WaitToLoadSceneOnStart);
        SceneManager.LoadScene(SceneIndex);
    }
}
