using UnityEngine;

public class KeypadEnterButton : MonoBehaviour, IInteractable
{
    private KeypadManager _manager;

    void Start() => _manager = GetComponentInParent<KeypadManager>();

    public void OnInteract()
    {
        // Visual feedback
        transform.localPosition -= transform.forward * 0.05f;
        Invoke("ResetPosition", 0.1f);
        
        // Trigger the check
        _manager.SubmitCode();
    }

    private void ResetPosition() => transform.localPosition += transform.forward * 0.05f;
}