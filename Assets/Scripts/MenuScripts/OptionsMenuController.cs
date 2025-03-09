using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public TMP_Text sensitivityNumber;
    public Toggle fullscreenToggle;
    [HideInInspector] public MouseLook playerCamera;

    void Start()
    {
        DisplayCurrentSettings();

        // Functions that are called when these settings change
        mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
    }

    // When this menu is turned on, make sure the correct settings are displayed
    void OnEnable()
    {
        DisplayCurrentSettings();
    }

    // Display the current settings to the options screen
    private void DisplayCurrentSettings()
    {
        DisplayCurrentSensitivity();
        DisplayCurrentFullscreen();
    }

    // Helper for displaying sensitivity
    private void DisplayCurrentSensitivity()
    {
        // Set the slider value to the current mouse sensitivity
        mouseSensitivitySlider.value = SettingsManager.Instance.mouseSensitivity;
        sensitivityNumber.text = SettingsManager.Instance.mouseSensitivity.ToString();
    }

    // Helper for displaying Fullscreen
    private void DisplayCurrentFullscreen()
    {
        // Set the toggle value to the current mouse sensitivity
        fullscreenToggle.isOn = SettingsManager.Instance.isFullscreen;
    }

    // Handles user inputs
    // Called when the slider value changes
    private void OnMouseSensitivityChanged(float value)
    {
        // Update the SettingsManager with the new mouse sensitivity value
        SettingsManager.Instance.SetMouseSensitivity(value);

        // Update MouseLook script with new value
        if (playerCamera != null) {
            playerCamera.mouseSensitivity = value;
        }

        sensitivityNumber.text = SettingsManager.Instance.mouseSensitivity.ToString();
    }

    // Called when the toggle value changes
    void OnFullscreenToggled(bool isFullscreen)
    {
        // Update the SettingsManager with the new fullscreen value
        SettingsManager.Instance.SetFullscreen(isFullscreen);
    }
}
