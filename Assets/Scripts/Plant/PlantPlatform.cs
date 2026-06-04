using UnityEngine;

public class PlantPlatform : Plant
{
    public override void IfIsAlive()
    {
        base.IfIsAlive();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = !isAlive;
    }
}
