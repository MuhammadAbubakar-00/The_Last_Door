using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WirePuzzleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<WireNodeUI> leftNodes;
    [SerializeField] private List<WireNodeUI> rightNodes;
    [SerializeField] private LineRenderer wirePrefab;
    [SerializeField] private Transform wireParent;
    [SerializeField] private GameObject puzzlePanel;

    [Header("Wire Colors")]
    [SerializeField] private List<Color> wireColors;
    [SerializeField] private ParticleSystem sparkPrefab;

    [Header("Gameplay")]
    [SerializeField] private float wrongPenalty = 20f;

    private Dictionary<int, int> correctPairs = new Dictionary<int, int>();
    private WireNodeUI selectedLeftNode;
    private int connectedCount = 0;

    private void Start()
    {
        InitializeNodes();
        AssignColors();
        GenerateRandomConnections();
    }

    private void InitializeNodes()
    {
        foreach (var node in leftNodes)
            node.Initialize(this);

        foreach (var node in rightNodes)
            node.Initialize(this);
    }

    private void GenerateRandomConnections()
    {
        List<int> shuffledRightIDs = new List<int>();

        foreach (var node in rightNodes)
            shuffledRightIDs.Add(node.NodeID);

        Shuffle(shuffledRightIDs);

        for (int i = 0; i < leftNodes.Count; i++)
        {
            correctPairs[leftNodes[i].NodeID] = shuffledRightIDs[i];
        }
    }

    public void HandleNodeSelection(WireNodeUI node)
    {
        if (node.IsLeftNode)
        {
            if (!node.GetComponent<UnityEngine.UI.Button>().interactable)
                return;

            selectedLeftNode = node;
            return;
        }

        if (selectedLeftNode == null)
            return;

        ValidateConnection(selectedLeftNode, node);
        selectedLeftNode = null;
    }

    private void ValidateConnection(WireNodeUI leftNode, WireNodeUI rightNode)
    {
        if (!correctPairs.ContainsKey(leftNode.NodeID))
            return;

        if (correctPairs[leftNode.NodeID] == rightNode.NodeID)
        {
            CreateWire(
             leftNode.transform.position,
             rightNode.transform.position,
             leftNode.NodeColor
         );
            SpawnSpark(rightNode.transform.position, leftNode.NodeColor);

            leftNode.SetInteractable(false);
            rightNode.SetInteractable(false);

            connectedCount++;

            if (connectedCount >= leftNodes.Count)
                PuzzleCompleted();
        }
        else
        {
            ApplyPenalty();
        }
    }

  private void CreateWire(Vector3 start, Vector3 end, Color wireColor)
{
    LineRenderer wire = Instantiate(wirePrefab, wireParent);

    wire.startColor = wireColor;
    wire.endColor = wireColor;

    // SAFE emission handling
    if (wire.material.HasProperty("_EmissionColor"))
    {
        wire.material.EnableKeyword("_EMISSION");
        wire.material.SetColor("_EmissionColor", wireColor * 2f);
    }

   DrawFullWire(wire, start, end);
}

// // private IEnumerator AnimateWire(LineRenderer wire, Vector3 start, Vector3 end)
// // {
// //     int resolution = 30;
// //     float duration = 0.4f;

// //     wire.positionCount = resolution;

// //     Vector3 mid = (start + end) / 2f + Vector3.down * 0.03f;

// //     float time = 0f;

// //     while (time < duration)
// //     {
// //         time += Time.deltaTime;
// //         float progress = time / duration;

// //         int visiblePoints = Mathf.FloorToInt(progress * resolution);

// //         for (int i = 0; i < visiblePoints; i++)
// //         {
// //             float t = i / (float)(resolution - 1);

// //             Vector3 point =
// //                 Mathf.Pow(1 - t, 2) * start +
// //                 2 * (1 - t) * t * mid +
// //                 Mathf.Pow(t, 2) * end;

// //             wire.SetPosition(i, point);
// //         }

// //         yield return null;
// //     }

//     // Finalize
//     for (int i = 0; i < resolution; i++)
//     {
//         float t = i / (float)(resolution - 1);

//         Vector3 point =
//             Mathf.Pow(1 - t, 2) * start +
//             2 * (1 - t) * t * mid +
//             Mathf.Pow(t, 2) * end;

//         wire.SetPosition(i, point);
//     }
// }

private void DrawFullWire(LineRenderer wire, Vector3 start, Vector3 end)
{
    int resolution = 30;
    wire.positionCount = resolution;

    Vector3 mid = (start + end) / 2f + Vector3.down * 0.03f;

    for (int i = 0; i < resolution; i++)
    {
        float t = i / (float)(resolution - 1);

        Vector3 point =
            Mathf.Pow(1 - t, 2) * start +
            2 * (1 - t) * t * mid +
            Mathf.Pow(t, 2) * end;

        wire.SetPosition(i, point);
    }
}
private void SpawnSpark(Vector3 position, Color color)
{
    if (sparkPrefab == null)
        return;

    ParticleSystem spark = Instantiate(sparkPrefab, position, Quaternion.identity);

    var main = spark.main;
    main.startColor = color;

    Destroy(spark.gameObject, 1f);
}
    private void ApplyPenalty()
    {
        TimerManager.instance.AddPenalty(wrongPenalty);
    }

   private void PuzzleCompleted()
{
    Level1Manager.instance.PuzzleSolved();
    puzzlePanel.SetActive(false);
}

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void AssignColors()
{
    for (int i = 0; i < leftNodes.Count; i++)
    {
        Color color = wireColors[i];

        leftNodes[i].SetColor(color);
        rightNodes[i].SetColor(color);
    }
}
}