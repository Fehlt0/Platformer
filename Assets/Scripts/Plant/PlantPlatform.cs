using UnityEngine;

public class PlantPlatform : Plant
{
    private BoxCollider2D _collider2d;

    public override void Start()
    {
        base.Start();
        _collider2d = gameObject.GetComponent<BoxCollider2D>();
    }

    public override void IfIsAlive()
    {
        base.IfIsAlive();
        _collider2d.isTrigger = !isAlive;
    }
}
