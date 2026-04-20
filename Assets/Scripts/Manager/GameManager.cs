using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        
    }

    public int lastCheckpoint;
    public int lastCamTarget;
    [SerializeField] private List<Transform> listCheckPoint;
    [SerializeField] private GameObject player;
    

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Instantiate(player, listCheckPoint[lastCheckpoint].position, Quaternion.identity);
        Debug.Log(CameraManager.instance.targetPos[lastCheckpoint].transform.position);
        CameraManager.instance.SetNewTarget(lastCamTarget);
    }
}
