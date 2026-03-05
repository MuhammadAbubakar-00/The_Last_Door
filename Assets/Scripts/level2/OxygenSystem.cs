using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OxygenSystem : MonoBehaviour
{
    public static OxygenSystem Instance;

    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private float normalDrainRate = 2f;
    [SerializeField] private float penaltyDrainRate = 6f;

    private float currentDrainRate;
    private bool isRunning = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentDrainRate = normalDrainRate;
        oxygenSlider.maxValue = 100f;
        oxygenSlider.value = 100f;
    }

    private void Update()
    {
        if (!isRunning) return;

        oxygenSlider.value -= currentDrainRate * Time.deltaTime;

        if (oxygenSlider.value <= 0)
        {
            oxygenSlider.value = 0;
            isRunning = false;
            Level2Manager.Instance.FailLevel();
        }
    }

    public void TriggerPenaltyDrain(float duration)
    {
        StartCoroutine(PenaltyDrainRoutine(duration));
    }

    private IEnumerator PenaltyDrainRoutine(float duration)
    {
        currentDrainRate = penaltyDrainRate;
        yield return new WaitForSeconds(duration);
        currentDrainRate = normalDrainRate;
    }

    public void StopOxygen()
    {
        isRunning = false;
    }
}