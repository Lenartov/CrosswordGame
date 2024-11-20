using UnityEngine;

public class LevelSelectionPanel : MonoBehaviour
{
    [SerializeField] private LevelButton levelButtonPrefab;

    private void Start()
    {
        SpawnButtons();
    }

    private void SpawnButtons()
    {
        for (int i = 0; i < LevelDataManager.Instance.LevelCount; i++)
        {
            LevelButton levelButton = Instantiate(levelButtonPrefab, transform);
            levelButton.LevelNumber = i;
            levelButton.levelName.text = (i + 1).ToString();

            levelButton.button.onClick.AddListener(levelButton.SetCurrentLevelNumber);
            levelButton.button.onClick.AddListener(SceneLoadSystem.LoadGameLevel);
        }
    }
}
