using UnityEngine;

public class PlantPlatform : Plant
{

    private BoxCollider2D boxCollider;

    public override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    public override void IfIsAlive()
    {
        base.IfIsAlive();
        if (isAlive)
        {
            boxCollider.isTrigger = false;
        }
        else
        {
            boxCollider.isTrigger = true;
        }
    }
}
