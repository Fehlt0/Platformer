using System.Linq;
using UnityEngine;

public class BulbPlant : Plant
{
    private Plant childPlant;

    public override void Start()
    {
        base.Start();
        
        // j'suis pas sur de comprendre ce que tu veux faire, tu veux récupérer la premiere
        // plante de ta liste d'enfants ? tu pouvais pas simplement faire un serializefield?
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
}