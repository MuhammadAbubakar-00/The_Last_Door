using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WireNodeUI : MonoBehaviour
{
    [Header("Node Config")]
    [SerializeField] private int nodeID;
    [SerializeField] private bool isLeftNode;
    [SerializeField] private Image nodeImage;

    private WirePuzzleManager puzzleManager;
    private Button button;

    public int NodeID => nodeID;
    public bool IsLeftNode => isLeftNode;
    public Color NodeColor => nodeImage.color;

    public void Initialize(WirePuzzleManager manager)
{
    puzzleManager = manager;

    button = GetComponent<Button>();

    // AUTO ASSIGN IMAGE (safe for production)
    if (nodeImage == null)
        nodeImage = GetComponent<Image>();

    button.onClick.RemoveAllListeners();
    button.onClick.AddListener(OnClicked);
}

    private void OnClicked()
    {
        puzzleManager.HandleNodeSelection(this);
    }

    public void SetInteractable(bool value)
    {
        if (button != null)
            button.interactable = value;
    }

    public void SetColor(Color color)
{
    if (nodeImage == null)
        nodeImage = GetComponent<Image>();

    color.a = 1f; // force visible
    nodeImage.color = color;
}
}