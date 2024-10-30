using System.Collections;
using System.Collections.Generic;
using TMKOC.Grammer;
using UnityEngine;

public class AudioManager : SerializedSingleton<AudioManager>
{

    public AudioSource audioSource;






    #region Audio Functions

    public void PlayOnce(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null.");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
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

    #endregion



}
