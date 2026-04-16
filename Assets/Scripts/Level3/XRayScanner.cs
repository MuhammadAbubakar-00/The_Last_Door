using UnityEngine;
using UnityEngine.UI;

public class XRayScanner : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button scanButton;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private GameObject scannerUI; // The UI element that shows the scanner interface

    [Header("Settings")]
    [SerializeField] private GameObject[] shieldObjects; // The outer wall parts
    [SerializeField] private float batteryDrainSpeed = 0.2f;
    [SerializeField] private float batteryRegenSpeed = 0.1f;

    private bool _isScanning = false;
    private float _currentBattery = 1f;

    private Camera arCamera;
    [SerializeField] private LayerMask hiddenLayer;
    

    void Start()
    {
        arCamera = Camera.main;
        scannerUI.SetActive(false); // Hide scanner UI by default
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

   private void ToggleShields(bool isVisible)
{
    if (isVisible)
        arCamera.cullingMask |= hiddenLayer; // Add layer to view
    else
        arCamera.cullingMask &= ~hiddenLayer; // Remove layer from view
}
    void Update()
{
    // High-performance check: Only run logic if we are scanning OR if battery needs to regen
    if (_isScanning && _currentBattery > 0)
    {
        _currentBattery -= Time.deltaTime * batteryDrainSpeed;
        scannerUI.SetActive(true); // Show the scanner UI
        ToggleShields(true); // SHOW the hidden code
        
        // If battery hits zero while holding, force stop
        if (_currentBattery <= 0) 
        {
            _currentBattery = 0;
            _isScanning = false;
        }
    }
    else
    {
        // Regain battery when not scanning
        if (_currentBattery < 1.0f)
            _currentBattery += Time.deltaTime * batteryRegenSpeed;
            
        ToggleShields(false); // HIDE the hidden code
        scannerUI.SetActive(false); // Hide the scanner UI
    }

    _currentBattery = Mathf.Clamp01(_currentBattery);
    if(batterySlider) batterySlider.value = _currentBattery;
}
}