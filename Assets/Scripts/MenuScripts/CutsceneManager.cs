using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject blackScreenPanel;
    public string nextScene;
    [HideInInspector] 
    private AsyncOperation sceneLoadOperation;

    void OnEnable()
    {
        // Start loading the game scene asynchronously
        sceneLoadOperation = SceneManager.LoadSceneAsync(nextScene);
        sceneLoadOperation.allowSceneActivation = false; // Prevent scene from switching immediately

        blackScreenPanel.SetActive(true); // Keep loading screen active at first
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare(); // Start preparing the video

        // Listen for when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // Allow skipping with any key
        if (Input.anyKeyDown)
        {
            SkipCutscene();
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        blackScreenPanel.SetActive(false); // Hide panel when video is ready
        videoPlayer.Play();
    }
    
    void OnVideoFinished(VideoPlayer vp)
    {
        // When the video naturally finishes, switch to the game scene
        sceneLoadOperation.allowSceneActivation = true;
    }

    public void SkipCutscene()
    {
        // Allow scene activation immediately when skipping
        sceneLoadOperation.allowSceneActivation = true;
    }
}