using System;
using UnityEngine;

public abstract class Plant : MonoBehaviour, IPlant
{
    public float baseTimeUntilDecay;
    public float timeUntilDecay;
    
    private bool isHitByLuciole;

    private Animator animatorRef;
    public AudioClip aliveClip;
    
    public bool isAlive;
    public bool hasBulb;
    private bool hasDecayed;
    private bool isReseting = true;

    private void Awake()
    {
        //GameManager.instance.AddPlant(this);
        isReseting = true;
    }

    private void OnDestroy()
    {
        GameManager.instance.RemovePlant(this);
    }

    public virtual void Start()
    {       
        
        animatorRef = GetComponent<Animator>();
        timeUntilDecay = 0;
        GameManager.instance.AddPlant(this);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light") && !hasBulb)
        {
            isReseting = false;
            if (animatorRef.GetBool("isAlive") == false)
            {
                AudioManager.instance.PlayAudioClip(aliveClip);
            }
            timeUntilDecay = baseTimeUntilDecay;
            hasDecayed = false;
        }
    }
    
    private void Update()
    {
        Decaying();
        IfIsAlive();
    }

    protected virtual void Decaying()
    {
        if (isHitByLuciole)
        {
            timeUntilDecay = baseTimeUntilDecay;
            hasDecayed = false;
        }
        else
        {
            timeUntilDecay -= Time.deltaTime;
            timeUntilDecay = Mathf.Max(0, timeUntilDecay);
        }
        isAlive = timeUntilDecay > 0;
        if (!isAlive && !hasDecayed && !isReseting)
        {
            hasDecayed = true;
        }
    }
    

    public virtual void IfIsAlive()
    {
        animatorRef.SetBool("isAlive", isAlive);
    }

    public void SetHit(bool value)
    {
        isHitByLuciole = value;
    }

    public virtual void ResetPlant()
    {
        isReseting = true;
        timeUntilDecay = 0;
        isAlive = false;
        animatorRef.SetBool("isAlive", false);
    }
}
