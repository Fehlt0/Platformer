using System.Collections.Generic;
using UnityEngine;

public class LanguePlant : MonoBehaviour
{
    
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

   
}