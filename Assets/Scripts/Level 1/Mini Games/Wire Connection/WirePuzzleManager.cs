using UnityEngine;
using System.Collections.Generic;

public class WirePuzzleManager : MonoBehaviour
{
    [Header("Setup")]
    public Camera arCamera;
    public LayerMask nodeLayer;
    public LineRenderer wirePrefab;
    public Transform wireParent;

    public List<WireNode> leftNodes;
    public List<WireNode> rightNodes;

    private Dictionary<int, int> correctPairs = new Dictionary<int, int>();

    private WireNode selectedLeftNode;
    private int connectedCount = 0;

    void Start()
    {
        foreach (var node in leftNodes)
            node.Initialize(this);

        foreach (var node in rightNodes)
            node.Initialize(this);

        GenerateConnections();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        Ray ray = arCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, nodeLayer))
        {
            WireNode node = hit.collider.GetComponent<WireNode>();
            if (node != null)
                node.OnTapped();
        }
    }

    void GenerateConnections()
    {
        List<int> shuffledRight = new List<int>();

        foreach (var r in rightNodes)
            shuffledRight.Add(r.nodeID);

        Shuffle(shuffledRight);

        for (int i = 0; i < leftNodes.Count; i++)
        {
            correctPairs[leftNodes[i].nodeID] = shuffledRight[i];
        }
    }

    public void NodeSelected(WireNode node)
    {
        if (node.isLeftNode)
        {
            selectedLeftNode = node;
            return;
        }

        if (selectedLeftNode == null) return;

        if (correctPairs[selectedLeftNode.nodeID] == node.nodeID)
        {
            CreateWire(selectedLeftNode.transform.position, node.transform.position);

            selectedLeftNode.isConnected = true;
            node.isConnected = true;

            connectedCount++;

            if (connectedCount >= leftNodes.Count)
                PuzzleComplete();
        }
        else
        {
            // Wrong answer
            FindObjectOfType<TimerManager>().AddPenalty(20f);
        }

        selectedLeftNode = null;
    }

    void CreateWire(Vector3 start, Vector3 end)
    {
        LineRenderer wire = Instantiate(wirePrefab, wireParent);
        wire.positionCount = 20;

        Vector3 mid = (start + end) / 2f + Vector3.down * 0.05f;

        for (int i = 0; i < 20; i++)
        {
            float t = i / 19f;

            Vector3 point =
                Mathf.Pow(1 - t, 2) * start +
                2 * (1 - t) * t * mid +
                Mathf.Pow(t, 2) * end;

            wire.SetPosition(i, point);
        }
    }

    void PuzzleComplete()
    {
        Level1Manager.instance.PuzzleSolved();
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}