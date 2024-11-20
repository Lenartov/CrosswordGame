using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] public Button button; 
    [SerializeField] public TextMeshProUGUI levelName;

    [HideInInspector] public int LevelNumber { get; set; }

    private void Awake()
    {
        if(levelName == null)
            levelName = GetComponentInChildren<TextMeshProUGUI>();        
        
        if(button == null)
            button = GetComponentInChildren<Button>();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    public void SetCurrentLevelNumber()
    {
        LevelDataManager.Instance.CurrentLevelIndex = LevelNumber;
    }
}
