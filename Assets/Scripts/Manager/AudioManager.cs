using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        
    }
    
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource audioSource;



    private void Start()
    {
        musicSource.clip = mainMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
