using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    /// <summary>
    /// Enables the OnMinuteChanged and OnHourChanged events.
    /// </summary>
    private void OnEnable()
    {
        TimeManager.OnMinuteChanged += UpdateTime;
        TimeManager.OnHourChanged += UpdateTime;
    }

    /// <summary>
    /// Disables the OnMinuteChanged and OnHourChanged events.
    /// </summary>
    private void OnDisable()
    {
        TimeManager.OnMinuteChanged -= UpdateTime;
        TimeManager.OnHourChanged -= UpdateTime;
    }

    /// <summary>
    /// Updates the Time label
    /// </summary>
    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";
    }
}
