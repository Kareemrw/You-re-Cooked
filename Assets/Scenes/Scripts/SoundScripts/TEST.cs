using System;
using System.Collections;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public AudioSource aS1, aS2;
    private static TEST instance; // Ensure only one instance persists
    private float defaultVolume = 1f;
    private float transitionTime = 1.25f;

    private void Awake()
    {
        // Ensure this object persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates when reloading scenes
            return;
        }

        // Also prevent AudioSources from being destroyed
        if (aS1 != null)
            DontDestroyOnLoad(aS1.gameObject);
        if (aS2 != null)
            DontDestroyOnLoad(aS2.gameObject);
    }

    public void ChangeClip()
    {
        AudioSource nowPlaying = aS1;
        AudioSource target = aS2;

        if (!nowPlaying.isPlaying)
        {
            nowPlaying = aS2;
            target = aS1;
        }

        StartCoroutine(MixSources(nowPlaying, target));
    }

    private IEnumerator MixSources(AudioSource nowPlaying, AudioSource target)
    {
        float percentage = 0;
        while (nowPlaying.volume > 0)
        {
            nowPlaying.volume = Mathf.Lerp(defaultVolume, 0, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }

        nowPlaying.Pause();
        if (!target.isPlaying)
            target.Play();
        target.UnPause();
        percentage = 0;

        while (target.volume < defaultVolume)
        {
            target.volume = Mathf.Lerp(0, defaultVolume, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }
    }
}
