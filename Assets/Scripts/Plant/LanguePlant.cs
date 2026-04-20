using UnityEngine;

public class LanguePlant : Plant
{

    public static bool langueAvailable;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isAlive)
        {

            langueAvailable = true;
        }
        else
        {
            langueAvailable = false;
        }
    }
}