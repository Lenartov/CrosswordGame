using UnityEngine;

public class ControlButton : MonoBehaviour
{
    public void GoToLevelList()
    {
        SceneLoadSystem.LoadLevelSelectionMenu();
    }

    public void GoToMainMenu()
    {
        SceneLoadSystem.LoadMainMenu();
    }

    public void ExitGame() 
    {
        SceneLoadSystem.ExitGame();
    }

    public void GoToNextLevel()
    {
        LevelDataManager.Instance.CurrentLevelIndex++;
        if (LevelDataManager.Instance.CurrentLevelIndex >= LevelDataManager.Instance.LevelCount)
            LevelDataManager.Instance.CurrentLevelIndex = 0;

        SceneLoadSystem.LoadGameLevel();
    }
}
