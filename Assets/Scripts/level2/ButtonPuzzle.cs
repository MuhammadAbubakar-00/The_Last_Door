using UnityEngine;
using System.Collections;

public class ButtonPuzzle : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public string leverID;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float speed = 5f;

    [Header("Effects")]
    [SerializeField] private AudioSource clickSound;
    
    private bool _isPulled = false;
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private SequencePuzzleManager _manager;

    void Start()
    {
        _startRotation = transform.localRotation;
        // Calculate the "pulled" position
        _endRotation = _startRotation * Quaternion.Euler(rotationAngle, 0, 0);
        _manager = GetComponentInParent<SequencePuzzleManager>();
    }

    public void OnInteract()
    {
        if (_isPulled) return; // Prevent double-triggering

        _isPulled = true;
        StartCoroutine(AnimateLever());
        
        // Professional Polish: Physical feedback
        Handheld.Vibrate(); 
        
        if(clickSound) clickSound.Play();

        // Tell the puzzle manager which lever was pulled
        _manager.RegisterPress(leverID);
    }

    private IEnumerator AnimateLever()
    {
        float elapsed = 0;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Slerp(_startRotation, _endRotation, elapsed);
            yield return null;
        }
    }
    
    // Call this if the player gets the sequence wrong to reset the levers
    public void ResetLever()
    {
        _isPulled = false;
        transform.localRotation = _startRotation;
    }
}