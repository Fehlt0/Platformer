using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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

    public GameObject pauseMenu;
    public GameObject deathMenu;

    public void SetDeathMenu(bool set)
    {
        if (set)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        deathMenu.SetActive(set);
    }
}
