using UnityEngine;
using System.Collections;

public class PanelFlicker : MonoBehaviour
{
    [Header("Panel Object")]
    [SerializeField] private GameObject panelObject;

    [Header("Flicker Settings")]
    [SerializeField] private float minDelay = 0.05f;
    [SerializeField] private float maxDelay = 0.15f;

    private bool flickering = true;

    void Start()
    {
        if (panelObject == null)
            panelObject = gameObject;

        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (flickering)
        {
            panelObject.SetActive(!panelObject.activeSelf);

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    public void StopFlicker()
    {
        flickering = false;
        panelObject.SetActive(true); // final stable state
    }
}