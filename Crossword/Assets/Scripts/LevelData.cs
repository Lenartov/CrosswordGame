using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string Letters;
    public string[] CorrectWords;

    public LevelData(string letters, string[] correctWords)
    {
        Letters = letters;
        CorrectWords = correctWords;
    }
}
