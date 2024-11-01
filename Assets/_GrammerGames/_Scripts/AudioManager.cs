using System;
using System.Collections;
using System.Collections.Generic;
using TMKOC.Grammer;
using UnityEngine;

public class AudioManager : SerializedSingleton<AudioManager>
{

    public AudioSource audioSource;
    public SoundsDataSO soundsDataSO;



    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        PlayOnce(soundsDataSO.nounsIntroduction, () =>
        {
            PlayOnce(soundsDataSO.categorySelection);
        });
    }



    #region Audio Functions

    public void PlayOnce(AudioClip clip, Action nextClip = null)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null.");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();

        StartCoroutine(WaitForClipEnd(nextClip));
    }


    public void PlayOnLoop(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null.");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();

    }

    public void Stop()
    {
        audioSource.Stop();
    }

    private IEnumerator WaitForClipEnd(Action onClipEnd)
    {
        while (audioSource.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        onClipEnd?.Invoke();
    }

    #endregion



}
