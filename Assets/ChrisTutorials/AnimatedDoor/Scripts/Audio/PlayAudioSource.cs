using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exposes AudioSource.Play to Animations while this script is attached to a GameObject
/// </summary>
public class PlayAudioSource : MonoBehaviour
{
    public AudioSource audioSource;

    // Use this for initialization
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public bool Playing
    {
        set
        {
            if (value == true)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SetClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }
}