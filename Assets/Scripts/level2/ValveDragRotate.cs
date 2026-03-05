using UnityEngine;

public class ValveDragRotate : MonoBehaviour
{
    private Camera arCamera;
    private bool isDragging = false;
    private Vector2 lastTouchPosition;

    public int CurrentValue { get; private set; }

    private void Start()
    {
        arCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        Ray ray = arCamera.ScreenPointToRay(touch.position);

        if (touch.phase == TouchPhase.Began)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    lastTouchPosition = touch.position;
                }
            }
        }

        if (isDragging && touch.phase == TouchPhase.Moved)
        {
            float delta = touch.position.x - lastTouchPosition.x;

            transform.Rotate(Vector3.forward, -delta * 0.2f);

            lastTouchPosition = touch.position;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            isDragging = false;
            SnapToNearestNumber();
        }
    }

    private void SnapToNearestNumber()
    {
        float angle = transform.localEulerAngles.z;
        int number = Mathf.RoundToInt(angle / 36f) % 10;

        CurrentValue = number;

        float snappedAngle = number * 36f;
        transform.localEulerAngles = new Vector3(0, 0, snappedAngle);

        Level2Manager.Instance.CheckCombination();
    }
}