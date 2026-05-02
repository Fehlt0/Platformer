using System;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    [SerializeField] private float baseTimeUntilDecay;
    private float timeUntilDecay;
    
    private bool isHitByLuciole;

    public Animator animatorRef;
    
    public bool isAlive;

    public virtual void Start()
    {
        animatorRef = GetComponent<Animator>();
        timeUntilDecay = 0;
    }

    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Light"))
        {
            timeUntilDecay = baseTimeUntilDecay;
        }
    }
    
    private void Update()
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
        IfIsAlive();
    }

    /*private void Update()
    {
        if (isHitByLuciole)
        {
            timeUntilDecay = baseTimeUntilDecay;
        }
        else
        {
            timeUntilDecay -= 0.01f;
            isAlive = timeUntilDecay > 0;
            if (timeUntilDecay <= 0)
            {
                timeUntilDecay = 0;
            }
            IfIsAlive();
        }  
    }*/

    public virtual void IfIsAlive()
    {
        animatorRef.SetBool("isAlive", isAlive);
    }

    public void SetHit(bool value)
    {
        isHitByLuciole = value;
    }
}
