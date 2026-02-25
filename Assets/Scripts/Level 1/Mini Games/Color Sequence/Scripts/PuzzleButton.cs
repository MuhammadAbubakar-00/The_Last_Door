using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    public int id;   // 👈 This is the ID

    public ColorSequence manager;

    public void OnPress()
    {
        manager.PlayerInput(id);
    }
}