using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip flip;
    [SerializeField] private AudioClip correct;
    [SerializeField] private AudioClip incorrect;
    [SerializeField] private AudioClip complete;
    private AudioSource audioSource;

    public static AudioController Instance { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayFlip()
    {
        audioSource.PlayOneShot(flip);
    }
    public void PlayCorrect()
    {
        audioSource.PlayOneShot(correct);
    }
    public void PlayIncorrect()
    {
        audioSource.PlayOneShot(incorrect);
    }
    public void PlayComplete()
    {
        audioSource.PlayOneShot(complete);
    }
}
