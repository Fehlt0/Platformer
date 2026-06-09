using UnityEngine;

public class ChangerBastien : MonoBehaviour
{
    private BoxCollider2D wallCollider;
    private Transform offsetPoint;

    [SerializeField] private int id;

    private void Start()
    {
        Transform offset = transform.Find("offset");
        if (offset != null)
        {
            offsetPoint = offset;
        }
        else
        {
            Debug.LogError("YA PAS PUTAIN !");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Change camera
            CameraManager.instance.SetNewTarget(id);

            // TP le joueur
            if (offsetPoint != null)
            {
                other.transform.position = offsetPoint.position;
                GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }
}