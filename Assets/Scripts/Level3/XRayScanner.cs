using UnityEngine;
using UnityEngine.UI;

public class XRayScanner : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button scanButton;
    [SerializeField] private Slider batterySlider;

    [Header("Settings")]
    [SerializeField] private GameObject[] shieldObjects; // The outer wall parts
    [SerializeField] private float batteryDrainSpeed = 0.2f;
    [SerializeField] private float batteryRegenSpeed = 0.1f;

    private bool _isScanning = false;
    private float _currentBattery = 1f;

    void Start()
    {
        // Setup Button Events
        // Requires a "Trigger" component or standard UI Button events
    }

    public void PointerDown() // Called when user holds the button
    {
        if (_currentBattery > 0.1f) _isScanning = true;
    }

    public void PointerUp() // Called when user releases the button
    {
        _isScanning = false;
    }

    void Update()
    {
        if (_isScanning && _currentBattery > 0)
        {
            _currentBattery -= Time.deltaTime * batteryDrainSpeed;
            ToggleShields(false); // Hide walls
        }
        else
        {
            _isScanning = false;
            _currentBattery += Time.deltaTime * batteryRegenSpeed;
            ToggleShields(true); // Show walls
        }

        _currentBattery = Mathf.Clamp01(_currentBattery);
        if(batterySlider) batterySlider.value = _currentBattery;
    }

    private void ToggleShields(bool state)
    {
        foreach (var shield in shieldObjects)
        {
            shield.SetActive(state);
        }
    }
}