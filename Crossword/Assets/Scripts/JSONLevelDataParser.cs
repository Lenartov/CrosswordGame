using System.IO;
using UnityEngine;

public static class JSONLevelDataParser
{
    public static LevelData ReadLevelDataFromJsonTextAsset(TextAsset textAsset)
    {
        return JsonUtility.FromJson<LevelData>(textAsset.text);
    }

    public static void WriteLevelDataToJsonFile(string path, LevelData levelData)
    {
        StreamWriter sw = File.CreateText(path);
        string json = JsonUtility.ToJson(levelData);
        sw.Write(json);
        sw.Close();
    }
}
