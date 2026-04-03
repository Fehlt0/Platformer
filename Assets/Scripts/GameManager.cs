using System;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public int lastCheckpoint;
    [SerializeField] private List<Transform> listCheckPoint;
    [SerializeField] private GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instantiate(player, listCheckPoint[lastCheckpoint].position, Quaternion.identity);
    }
}
