using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LettersCircle lettersCircle;
    [SerializeField] private Crossword crossword;
    [SerializeField] private WinPanel winPanel;

    private void Start()
    {
        InitLevel();
    }

    private void InitLevel()
    {
        lettersCircle.InstantiateLetters(GetLetters());
        crossword.Instantiate(GetCorrectWords());

        lettersCircle.OnWordSelected += crossword.GuessCorrectWord;
        crossword.OnAllWordsFinded += Win;
    }

    private char[] GetLetters()
    {
        return LevelDataManager.Instance.GetCurrentLevelData().Letters.ToLower().ToCharArray();
    }

    private string[] GetCorrectWords()
    {
        string[] strs = LevelDataManager.Instance.GetCurrentLevelData().CorrectWords;
        for (int i = 0; i < strs.Length; i++)
        {
            strs[i] = strs[i].ToLower();
        }

        return strs;
    }

    public void Win()
    {
        winPanel.ShowPanel();
    }


}
