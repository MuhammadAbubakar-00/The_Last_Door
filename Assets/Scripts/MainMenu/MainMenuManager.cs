using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        settingPopUp.SetActive(false);
        quitPopUp.SetActive(false);
    }
    [SerializeField] private GameObject settingPopUp;
    [SerializeField] private GameObject quitPopUp;
    
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
    public void OpenQuitPopUp()
    {
        if (quitPopUp != null)
            quitPopUp.SetActive(true);
    }
}
