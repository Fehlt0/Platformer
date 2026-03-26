using System;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{

    
    [SerializeField] private float baseTimeUntilDecay;
    private float timeUntilDecay;

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
            Debug.Log("est detecte");
            timeUntilDecay = baseTimeUntilDecay;
        }
    }

    private void Update()
    {
        timeUntilDecay -= 0.01f;
        isAlive = timeUntilDecay > 0;
        if (timeUntilDecay <= 0)
        {
            timeUntilDecay = 0;
        }
        IfIsAlive();
        
    }

    public virtual void IfIsAlive()
    {
        if (isAlive)
        {
            animatorRef.SetBool("isAlive", true);
        }
        else
        {
            animatorRef.SetBool("isAlive", false);
        }
    }
}
