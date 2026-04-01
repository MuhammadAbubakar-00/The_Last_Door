using UnityEngine;
using System.Collections;

public class KeypadButton : MonoBehaviour, IInteractable
{
    public string numberValue;
    private KeypadManager _manager;

    void Start() => _manager = GetComponentInParent<KeypadManager>();

    public void OnInteract()
    {
        // Visual "Push" feedback
        StartCoroutine(AnimatePress());
        _manager.OnKeyPress(numberValue);
    }

    private IEnumerator AnimatePress()
    {
        Vector3 startPos = transform.localPosition;
        transform.localPosition -= transform.forward * 0.005f; // Push in
        yield return new WaitForSeconds(0.1f);
        transform.localPosition = startPos; // Pop back
    }

    
}