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
        //DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        Instantiate(player, listCheckPoint[lastCheckpoint].position, Quaternion.identity);
    }

    public List<Plant> listPlant = new List<Plant>();

    public int lastCheckpoint;
    public int lastCamTarget;
    public List<Transform> listCheckPoint;
    [SerializeField] private GameObject player;

    public void AddPlant(Plant plant)
    {
        listPlant.Add(plant);
    }

    public void RemovePlant(Plant plant)
    {
        listPlant.Remove(plant);
    }

    public void ResetScene()
    {
        foreach (var plant in listPlant)
        {
            plant.ResetPlant();
        }
        Instantiate(player, listCheckPoint[lastCheckpoint].position, Quaternion.identity);
        CameraManager.instance.SetNewTarget(lastCamTarget);
    }
}
