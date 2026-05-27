using System;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public float baseTimeUntilDecay;
    public float timeUntilDecay;
    
    private bool isHitByLuciole;

    private Animator animatorRef;
    
    public bool isAlive;
    public bool hasBulb;

    public virtual void Start()
    {
        animatorRef = GetComponent<Animator>();
        timeUntilDecay = 0;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light") && !hasBulb)
        {
            timeUntilDecay = baseTimeUntilDecay;
        }
    }
    
    private void Update()
    {
        Decaying();
        IfIsAlive();
    }

    private void Decaying()
    {
        if (isHitByLuciole)
        {
            timeUntilDecay = baseTimeUntilDecay;
        }
        else
        {
            timeUntilDecay -= Time.deltaTime;
            timeUntilDecay = Mathf.Max(0, timeUntilDecay);
        }
        isAlive = timeUntilDecay > 0;
    }
    

    public virtual void IfIsAlive()
    {
        animatorRef.SetBool("isAlive", isAlive);
    }

    public void SetHit(bool value)
    {
        isHitByLuciole = value;
    }
}
