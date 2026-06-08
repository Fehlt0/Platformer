using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioSource audioSource;


    private void Start()
    {
        audioSource.clip = mainMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
