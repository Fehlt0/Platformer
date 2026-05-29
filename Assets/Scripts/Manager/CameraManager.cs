using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public Transform target;
    public GameObject camera;
    public List<Transform> targetPos;
    
    [SerializeField] private float moveSpeed;

    public void Start()
    {
        camera = GameObject.Find("MainCamera");
        SetNewTarget(GameManager.instance.lastCamTarget);
    }
    
    void Update()
    {
        // du coup la caméra suit sans effet du tout la cible? peut etre qu'un petit tween / lerp pourrait la rendre plus smoothie
        camera.transform.position = new Vector3(target.position.x, target.position.y, -1) ;
    }
    
    public void SetNewTarget(int newTarget)
    {
        target = targetPos[newTarget];
    }
}