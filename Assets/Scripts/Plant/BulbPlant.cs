using System.Linq;
using UnityEngine;

public class BulbPlant : Plant
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            Plant[] childPlants = GetComponentsInChildren<Plant>();
            
            foreach (var childPlant in childPlants)
            {
                if (childPlant.gameObject != this.gameObject)
                {
                    childPlant.timeUntilDecay = childPlant.baseTimeUntilDecay;
                    break;
                }
            }
        }
    }
}