using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager instance;

    [Header("Match Start Audio")]
    public AudioSource matchStartAudioSource;
    public AudioClip matchStartClip;

    [Header("Gun Audio")]
    public AudioSource gunAudioSource;
    public AudioClip gunShotClip;

    [Header("Ping Audio")]
    public AudioSource pingAudioSource;
    public AudioClip pingClip;

    public AudioSource audioSource;

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);
    }

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
}
