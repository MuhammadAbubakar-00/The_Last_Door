using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WireClueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WirePuzzleManager puzzleManager;
    [SerializeField] private TextMeshProUGUI clueText;

    [Header("Sticker Placement")]
    [SerializeField] private Transform[] wallSpots;
    [SerializeField] private GameObject stickerPrefab;

    private GameObject currentSticker;

    void Start()
    {
        GenerateClue();
    }

    public void GenerateClue()
    {
        Dictionary<int, int> pairs = puzzleManager.GetCorrectPairs();

        string clue = "WIRE ROUTING\n\n";

        foreach (KeyValuePair<int, int> pair in pairs)
        {
            clue += pair.Key + " → " + pair.Value + "\n";
        }

        clueText.text = clue;

        PlaceSticker();
    }

    void PlaceSticker()
    {
        if (currentSticker != null)
            Destroy(currentSticker);

        int randomWall = Random.Range(0, wallSpots.Length);

        currentSticker = Instantiate(
            stickerPrefab,
            wallSpots[randomWall].position,
            wallSpots[randomWall].rotation
        );
    }
}