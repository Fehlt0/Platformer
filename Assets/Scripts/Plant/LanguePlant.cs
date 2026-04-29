using System.Collections.Generic;
using UnityEngine;

public class LanguePlant : MonoBehaviour
{

    //public bool langueAvailable = true;
    
    public static List<LanguePlant> listPlanteLangue = new List<LanguePlant>();
    
    private SpriteRenderer sr;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        listPlanteLangue.Add(this);
    }

    private void OnDestroy()
    {
        listPlanteLangue.Remove(this);
    }

    public void SetColorOnTarget(bool active)
    {
        if (sr != null)
            sr.color = active ? Color.blue : Color.red;
    }

   /* public override void Start()
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
    }*/
}