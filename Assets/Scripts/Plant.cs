using System;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{

    
    [SerializeField] private float baseTimeUntilDecay;
    private float timeUntilDecay;
    
    [SerializeField] private Sprite spriteVivant;
    [SerializeField] private Sprite spriteMort;
    private Sprite spriteRef;

    public bool isAlive;

    private void Start()
    {
        spriteRef = GetComponent<SpriteRenderer>().sprite;
        timeUntilDecay = 0;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("est detecte");
        timeUntilDecay = baseTimeUntilDecay;
    }

    private void Update()
    {
        timeUntilDecay -= 0.01f;
        isAlive = timeUntilDecay > 0;
        if (timeUntilDecay <= 0)
        {
            timeUntilDecay = 0;
        }
        if (isAlive)
        {
            spriteRef = spriteVivant;
        }
        else
        {
            spriteRef = spriteMort;
        }
    }
}
