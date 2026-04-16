using UnityEngine;
using TMPro;
using System.Collections;

public class KeypadManager : MonoBehaviour
{
    [Header("Settings")]
    public string winningCode = "258";
    [SerializeField] private TextMeshProUGUI displayScreen;
    [SerializeField] private int maxDigits = 3;

    [Header("Feedback")]
    public AudioSource beepSound;
    public AudioSource successSound;
    public AudioSource failSound;

    [Header("Level Completion")]
    [SerializeField] private Animator doorAnimator; // Drag your Door here
    [SerializeField] private string doorOpenTrigger = "Open";
    [SerializeField] private OxygenTimer oxygenTimer; // To stop the countdown
    [SerializeField] private GameObject WinScreenUI; // The UI element that shows the keypad interface

    private string _currentInput = "";

    public void OnKeyPress(string value)
{
    // Limit input to 3 or 4 digits (whatever your max is)
    if (_currentInput.Length >= maxDigits) return;

    _currentInput += value;
    displayScreen.text = _currentInput;
    if(beepSound) beepSound.Play();
}

// THIS IS THE NEW FUNCTION FOR THE ENTER BUTTON
public void SubmitCode()
{
    if (_currentInput.Length == 0) return; // Don't check empty input

    if (_currentInput == winningCode)
    {
        HandleAccessGranted();
    }
    else
    {
        StartCoroutine(ResetKeypad());
    }
}

private void HandleAccessGranted()
{
    displayScreen.color = Color.green;
    displayScreen.text = "GRANTED";
    if(successSound) successSound.Play();
    
    // Play Door Animation
    if (doorAnimator != null) doorAnimator.SetTrigger("Open");
    // Show Win Screen
    if (WinScreenUI != null) WinScreenUI.SetActive(true);
    // Stop Oxygen Timer
    if (oxygenTimer != null) oxygenTimer.StopTimer();
}

    private IEnumerator ResetKeypad()
    {
        displayScreen.color = Color.red;
        failSound.Play();
        yield return new WaitForSeconds(1f);
        
        _currentInput = "";
        displayScreen.text = "---";
        displayScreen.color = Color.white;
    }
}