using UnityEngine;

public class WireNode : MonoBehaviour
{
    public int nodeID;
    public bool isLeftNode;
    public bool isConnected = false;

    private WirePuzzleManager manager;

    public void Initialize(WirePuzzleManager m)
    {
        manager = m;
    }

    public void OnTapped()
    {
        if (isConnected) return;
        //manager.NodeSelected(this);
    }
}