using UnityEngine;

public class AutoAssignID : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i)
                     .GetComponent<PuzzleButton>()
                     .id = i;
        }
    }
}