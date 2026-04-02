using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    [SerializeField] private float moveSpeed;

    public void Start()
    {
        transform.position = target.position;
    }
    
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position,moveSpeed*Time.deltaTime);
    }
    
    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
