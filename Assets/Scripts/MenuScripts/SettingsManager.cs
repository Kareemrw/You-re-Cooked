using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
	// Public Inspector Vars
    public float defaultMouseSensitivity;
    public float mouseSensitivity;
    public bool isFullscreen;

	// Private Vars
    

    // ------------- Singleton Setup -------------
	private static SettingsManager _instance;
	public static SettingsManager Instance { get { return _instance; } } // make a public way to access the private variable

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{ // if there is already a value assigned to the private variable and its not this, destroy this
			Destroy(this.gameObject);
		}
		else
		{ // if there is no value assigned to the private variable, assign this as the reference
			_instance = this;
		}
		DontDestroyOnLoad(this);

        RestorePrefs();
	}


    void RestorePrefs()
    {
        // PlayerPrefs.DeleteAll(); // debugging
		
        // -- Load settings from player prefs --
        // Fullscreen
        // Unity is stinky and this one doesnt actually work for some reason
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
		Screen.fullScreen = isFullscreen;

        // Mouse Sensitivity
        mouseSensitivity = PlayerPrefs.GetFloat("Mouse Sensitivity", defaultMouseSensitivity);
    }

    public void SetFullscreen(bool isOn)
    {
        // Toggle fullscreen
		Screen.fullScreen = isOn;

		// Save the setting (cant save bool values so convert to 0 or 1)
		PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMouseSensitivity(float sens)
    {
        mouseSensitivity = sens;
        // Save the setting
		PlayerPrefs.SetFloat("Mouse Sensitivity", sens);
        PlayerPrefs.Save();
    }
}
