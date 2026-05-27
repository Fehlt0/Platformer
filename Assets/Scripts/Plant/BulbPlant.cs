using System.Linq;
using UnityEngine;

public class BulbPlant : Plant
{
    private Plant childPlant;

    public override void Start()
    {
        base.Start();
        foreach (Plant plante in GetComponentsInChildren<Plant>())
        {
            if (plante.gameObject != this.gameObject)
            {
                childPlant = plante;
                break;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            if (childPlant != null)
            {
                childPlant.timeUntilDecay = childPlant.baseTimeUntilDecay;
            }

        }
    }

    public new void SetHit(bool value)
    {
        if (childPlant != null)
            childPlant.SetHit(value);
    }
    
    
/*
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            //Plant[] childPlants = GetComponentsInChildren<Plant>();

            foreach (var childPlant in childPlants)
            {
                if (childPlant.gameObject != this.gameObject)
                {
                    childPlant.timeUntilDecay = childPlant.baseTimeUntilDecay;
                    break;
                }
            }
        }
    }*/
}