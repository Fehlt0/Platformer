using System.Collections.Generic;
using UnityEngine;

public class LanguePlant : MonoBehaviour
{
    
    public static List<LanguePlant> listPlanteLangue = new List<LanguePlant>();
    
    [SerializeField] private Sprite spriteDefault;
    [SerializeField] private Sprite spriteInRange;
    [SerializeField] private Sprite spriteHanging;
    
    private SpriteRenderer spriteR;
    
    private bool ishanging;

    public void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        listPlanteLangue.Add(this);
    }

    private void OnDestroy()
    {
        listPlanteLangue.Remove(this);
    }

    public void SetSpriteOnTarget(bool active)
    {
        if (spriteR == null)
        {
            return;
        }

        if (!active)
        {
            ishanging = false;
            spriteR.sprite = spriteDefault;
        }
        else
        {
            spriteR.sprite = ishanging ? spriteHanging : spriteInRange;
        }
            
    }

    public void SetSpriteHanging(bool hanging)
    {
        ishanging = hanging;
        if (spriteR == null)
        {
            return;
        }
        
        spriteR.sprite = hanging ? spriteHanging : spriteDefault;
    }

   
}