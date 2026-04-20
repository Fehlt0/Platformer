using System.Collections.Generic;
using UnityEngine;

public class LanguePlant : Plant
{

    public bool langueAvailable;
    
    public static List<LanguePlant> listPlanteLangue = new List<LanguePlant>();

    public void Awake()
    {
        base.Start();
        listPlanteLangue.Add(this);
    }

    public override void Start()
    {
        foreach (var plante in listPlanteLangue)
        {
            DontDestroyOnLoad(plante);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isAlive)
        {

            langueAvailable = true;
        }
        else
        {
            langueAvailable = false;
        }
    }
}