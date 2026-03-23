using UnityEngine;

public class ARCameraRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private RectTransform reticle; // Optional: UI dot in center

    void Update()
    {
        // Shoot ray from center of screen
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            // Hover effect
            if(reticle) reticle.localScale = Vector3.one * 2f;

            if (GetInput())
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.OnInteract();
            }
        }
        else
        {
            if(reticle) reticle.localScale = Vector3.one;
        }
    }

    bool GetInput()
    {
        return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }
}

public interface IInteractable
{
    void OnInteract();
}