using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoadSystem
{

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void LoadLevelSelectionMenu()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadGameLevel()
    {
        SceneManager.LoadScene(2);
    }
}