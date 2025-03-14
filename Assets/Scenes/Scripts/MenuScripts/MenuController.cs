using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// If there is a titleMenuPanel, it tells the script to treat it like a titlescreen and wont allow pausing
// If there is a pauseMenuPanel, it tells the script to treat it like an in game menu that allows pausing
public class MenuController : MonoBehaviour
{
    [Header("Title Menu UI")]
    public GameObject titleMenuPanel;
    [Tooltip("Scene to switch to when hitting start")]
    public string gameSceneName;
    public GameObject creditsPanel;
    public GameObject cutscenePanel;

    [Header("Both Menu UI")]
    public GameObject optionsPanel;

    [Header("In Game Menu UI")]

    [Tooltip("Only put the Pause Menu here if this is an in game scene")]
    public GameObject pauseMenuPanel;
    [Tooltip("Only put the Player Camera here if this is an in game scene")]
    public MouseLook playerCamera; // Reference to the player's movement script to freeze it during pause

    private bool isPaused = false;
    private Controls inputActions; // Reference to your Input Action Asset

    void Awake()
    {
        inputActions = new Controls(); // Create an instance of Controls input actions

        // Enable the input actions
        inputActions.Enable();
    }

    void OnEnable()
    {
        // Bind the pause action to the PauseGame method
        inputActions.Game.Pause.performed += ctx => TogglePause();
    }

    void OnDisable()
    {
        // Unbind the pause action when the script is disabled
        inputActions.Game.Pause.performed -= ctx => TogglePause();
    }

    void Start()
    {
        // Ensure the title menu is active at start, if there is one
        if (titleMenuPanel != null) {
            ShowTitleMenu();
            cutscenePanel.GetComponent<CutsceneManager>().nextScene = gameSceneName;
        }

        // If there is a pause menu, ensure all menus disabled at start
        if (pauseMenuPanel != null) {
            // pauseMenuPanel.SetActive(false);
            CloseAllMenus();
            Time.timeScale = 1f; // Make sure game time is unpaused

            // Find player camera controller in the scene if not assigned already
            if (playerCamera == null) {
                playerCamera = FindObjectOfType<MouseLook>();
            }
            optionsPanel.GetComponent<OptionsMenuController>().playerCamera = playerCamera;
        }

        // Unlock cursor in TitleMenus (otherwise, its already taken care of in game by MouseLook.cs)
        if (titleMenuPanel != null) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = titleMenuPanel != null;
        }
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    // ------------ Button Functions ------------
    // Scene Switching
    public void StartGame()
    {
        // SceneManager.LoadScene(gameSceneName);
        CloseAllMenus();
        cutscenePanel.SetActive(true);
    }

    public void ReturnToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Menu Navigation
    // These functions make sure all other menus are closed and then sets the one you want to true
    public void ShowTitleMenu()
    {
        CloseAllMenus();
        titleMenuPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        CloseAllMenus();
        optionsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        CloseAllMenus();
        creditsPanel.SetActive(true);
    }

    public void ShowPause()
    {
        CloseAllMenus();
        pauseMenuPanel.SetActive(true);
    }

    // Helper function that closes all menus (if they exist)
    public void CloseAllMenus()
    {
        if (titleMenuPanel != null) { titleMenuPanel.SetActive(false); }
        if (cutscenePanel != null) { cutscenePanel.SetActive(false); }
        if (optionsPanel != null) { optionsPanel.SetActive(false); }
        if (creditsPanel != null) { creditsPanel.SetActive(false); }
        if (pauseMenuPanel != null) { pauseMenuPanel.SetActive(false); }
    }


    // Pause Menu Logic
    public void PauseGame()
    {
        if (pauseMenuPanel == null) return;

        isPaused = true;
        Time.timeScale = 0f; // Freezes the game
        pauseMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Technically not needed since freezing time works on this too
        // Stop the camera movement script
        // if (playerCamera != null) {
            // playerCamera.enabled = false;
        // }
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel == null) return;

        isPaused = false;
        Time.timeScale = 1f; // Unfreezes the game
        CloseAllMenus();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Technically not needed since freezing time works on this too
        // Resume the camera movement script
        // if (playerCamera != null) {
        //     playerCamera.enabled = true;
        // }
    }
    
}
