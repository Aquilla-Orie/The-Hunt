using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Match Start Audio")]
    public AudioClip matchStartClip;

    [Header("Gun Audio")]
    public AudioClip gunShotClip;

    [Header("Ping Audio")]
    public AudioClip pingClip;

    [Header("Place Item Audio")]
    public AudioClip placeItemClip;

    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && matchStartClip != null)
        {
            audioSource.clip = matchStartClip;
            audioSource.Play();
        }
    }

    public void PlayGunShot()
    {
        if (audioSource != null && gunShotClip != null)
        {
            audioSource.clip = gunShotClip;
            audioSource.Play();
        }
    }

    public void PlayPingAudio()
    {
        if (audioSource != null && pingClip != null)
        {
            audioSource.clip = pingClip;
            audioSource.Play();
        }
    }

    public void PlayPlaceItemAudio()
    {
        if (audioSource != null && placeItemClip != null)
        {
            audioSource.clip = placeItemClip;
            audioSource.Play();
        }
    }
}
