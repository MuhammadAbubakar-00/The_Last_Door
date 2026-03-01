using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        settingPopUp.SetActive(false);
    }
    [SerializeField] private GameObject settingPopUp;
    public string sceneName;
    public void StartGame()
    {
       SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OpenSetting()
    {
        if (settingPopUp != null)
            settingPopUp.SetActive(true);
    }
}
