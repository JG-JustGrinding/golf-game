using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level " + levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
