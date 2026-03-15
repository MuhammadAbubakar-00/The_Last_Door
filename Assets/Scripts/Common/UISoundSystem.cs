using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISoundSystem : MonoBehaviour
{
    public static UISoundSystem Instance;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private float volume = 1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayClickSound);
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (buttonClickSound == null) return;

        if (SfxManager.Instance != null)
            SfxManager.Instance.PlaySFX(buttonClickSound, volume);
    }
}