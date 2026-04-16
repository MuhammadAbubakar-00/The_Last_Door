using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequencePuzzleManager : MonoBehaviour
{
    public List<string> correctIDSequence;
    private List<string> currentInputSequence = new List<string>();

    [Header("Level Objects")]
    public Animator doorAnimator;
    public GameObject WinUi;
   // public AudioSource successSFX;
   // public AudioSource errorSFX;
void start()
    {
        WinUi.SetActive(false);
    }

   public void RegisterPress(string id)
{
    currentInputSequence.Add(id);
    
    for (int i = 0; i < currentInputSequence.Count; i++)
    {
        if (currentInputSequence[i] != correctIDSequence[i])
        {
            HandleWrongSequence();
            return;
        }
    }

    if (currentInputSequence.Count == correctIDSequence.Count)
    {
        UnlockEscape();
    }
}

    void UnlockEscape()
    {
       // successSFX.Play();
        doorAnimator.SetTrigger("Open");

        // Logic to move to next level or show "Victory" UI
        WinUi.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    private void HandleWrongSequence()
{
    // errorSFX.Play();
    currentInputSequence.Clear();

    // Find all levers in the room and reset them
    ButtonPuzzle[] allLevers = GetComponentsInChildren<ButtonPuzzle>();
    foreach (var lever in allLevers)
    {
        lever.ResetLever();
    }
    
    // Optional: UI Flash Red Logic here
}
}