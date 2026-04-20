using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    

    
    void Start()
    {
        gameObject.transform.position = target.position;
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);

        
    }
    
    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
    }


    
    
    
}
