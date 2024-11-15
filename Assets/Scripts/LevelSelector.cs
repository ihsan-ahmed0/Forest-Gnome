using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    
    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
    }
}
