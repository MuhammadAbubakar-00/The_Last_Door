using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
using System.Collections;

public class PermissionManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "Splash";

    void Start()
    {
        StartCoroutine(RequestPermissions());
    }

    IEnumerator RequestPermissions()
    {
#if UNITY_ANDROID
        // Microphone Permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            yield return new WaitUntil(() =>
                Permission.HasUserAuthorizedPermission(Permission.Microphone));
        }

        // Camera Permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitUntil(() =>
                Permission.HasUserAuthorizedPermission(Permission.Camera));
        }
        
#endif

        // Load scene after permissions
        SceneManager.LoadScene(sceneToLoad);
    }
}