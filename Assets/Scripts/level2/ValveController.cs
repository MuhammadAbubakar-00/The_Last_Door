using UnityEngine;

public class ValveController : MonoBehaviour
{
    [SerializeField] private int currentValue = 0;
    [SerializeField] private float rotationStep = 36f; // 360 / 10
    [SerializeField] private AudioSource rotateSound;

    public int CurrentValue => currentValue;

    public void RotateValve()
    {
        currentValue++;
        if (currentValue > 9)
            currentValue = 0;

        transform.Rotate(Vector3.forward * -rotationStep);

        if (rotateSound != null)
            rotateSound.Play();

        Level2Manager.Instance.CheckCombination();
    }
}