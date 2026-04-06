using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        onEnter.Invoke();
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        onExit.Invoke();
    }
}
