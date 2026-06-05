using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private bool isPaused = false;

    private GameObject currentPlayer;
    
    [SerializeField] private GameObject pauseMenuUI;

    private void Awake()
    {
        Time .timeScale = 1;
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
        currentPlayer = Instantiate(player, listCheckPoint[lastCheckpoint].position, Quaternion.identity);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame)
        {
            OnPauseMenu();
        }
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
        currentPlayer = Instantiate(player, listCheckPoint[lastCheckpoint].position, Quaternion.identity);
        CameraManager.instance.SetNewTarget(lastCamTarget);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
        
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        
    }

    public void OnPauseMenu()
    {
        Debug.Log("OnPauseMenu");
        
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
        
        isPaused = !isPaused;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetLevel()
    {

        foreach (var plant in listPlant)
        {
            plant.ResetPlant();
        }
        currentPlayer.transform.position = listCheckPoint[lastCheckpoint].position;
        
        CameraManager.instance.SetNewTarget(lastCamTarget);
    }
}
