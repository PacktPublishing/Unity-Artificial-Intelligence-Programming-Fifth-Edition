using UnityEngine;

/// <summary>
/// Behavior DoneDayNightCycle component. Add it to your directional light to control the brightness,colorand time to simulate day and night.
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    /// <value>Event raised when sun rises or sets.</value>
    // Event raised when sun rises or sets.
    public event System.EventHandler OnChanged;

    /// <value>Complete day-night cycle duration (in seconds).</value>
    // Complete day-night cycle duration (in seconds).
    public float dayDuration = 10.0f;

    /// <value>Read-only property that informs if it is currently night time.</value>
    // Read-only property that informs if it is currently night time.
    public bool IsNight { get; private set; }

    // Private field with the hard-coded night color.
    public Color nightColor = Color.white * 0.1f;

    // Private field with the day color. It is set to the initial light color.
    private Color dayColor;

    // Reference to the Light component
    private Light lightComponent;

    /// <summary>DoneDayNightCycle Initialization Method.</summary>
    /// <remarks>Search the light component and set color light.</remarks>
    void Start()
    {
        lightComponent = GetComponent<Light>();
        dayColor = lightComponent.color;
    }

    /// <summary>DoneDayNightCycle Update Method.</summary>
    /// <remarks>Calculate the intensity of the light with the time elapsed and 
    /// depending on it do a lerp between the color of day and night, also register of EventHandler of this class.</remarks>
    void Update() {
        float lightIntensity = 0.5f +
                      Mathf.Sin(Time.time * 2.0f * Mathf.PI / dayDuration) / 2.0f;

        bool shouldBeNight = lightIntensity < 0.3f;
        if (IsNight != shouldBeNight) {
            IsNight = shouldBeNight;
            OnChanged?.Invoke(this, System.EventArgs.Empty); // Invoke event handler (if set).
        }

        lightComponent.color = Color.Lerp(nightColor, dayColor, lightIntensity);
    }

} 