using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchGame : MonoBehaviour
{
    public void OnToggleMenu()
    {
        SceneManager.LoadScene(1);
    }
    
    
}
