using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager Instance;

    [SerializeField] private ValveController valve1;
    [SerializeField] private ValveController valve2;
    [SerializeField] private ValveController valve3;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckCombination()
    {
        int v1 = valve1.CurrentValue;
        int v2 = valve2.CurrentValue;
        int v3 = valve3.CurrentValue;

        if (v1 + v2 + v3 == 12 &&
            v2 > v1 &&
            v2 > v3 &&
            v1 % 2 == 1)
        {
            CompleteLevel();
        }
        else
        {
            OxygenSystem.Instance.TriggerPenaltyDrain(5f);
        }
    }

    private void CompleteLevel()
    {
        OxygenSystem.Instance.StopOxygen();
        Debug.Log("OXYGEN STABILIZED");
        // Show final panel
    }

    public void FailLevel()
    {
        Debug.Log("OXYGEN DEPLETED");
        // Show fail screen
    }
}