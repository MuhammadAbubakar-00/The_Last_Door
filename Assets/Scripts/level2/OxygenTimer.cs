using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OxygenTimer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxOxygen = 60f; // 60 seconds
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Image fillImage;
    
    [Header("Critical Warning")]
    [SerializeField] private Color normalColor = Color.cyan;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float warningThreshold = 0.3f; // 30% left

    public UnityEvent OnOxygenDepleted;

    private float _currentOxygen;
    private bool _isTimerRunning = false;

    void Start()
    {
        _currentOxygen = maxOxygen;
        oxygenSlider.maxValue = maxOxygen;
        _isTimerRunning = true;
        UpdateUI();
    }

    // Call this from your PlaceOnPlane "OnContentPlaced" event!
    public void StartTimer()
    {
        _isTimerRunning = true;
    }

    void Update()
    {
        if (!_isTimerRunning) return;

        if (_currentOxygen > 0)
        {
            _currentOxygen -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            _currentOxygen = 0;
            _isTimerRunning = false;
            OnOxygenDepleted?.Invoke();
        }
    }

    void UpdateUI()
    {
        oxygenSlider.value = _currentOxygen;
        
        float ratio = _currentOxygen / maxOxygen;
        
        // Change color to red when low
        if (ratio <= warningThreshold)
        {
            fillImage.color = Color.Lerp(warningColor, normalColor, ratio / warningThreshold);
            // Professional touch: Add a slight camera shake or heartbeat sound here
        }
    }

    public void StopTimer() => _isTimerRunning = false;
}