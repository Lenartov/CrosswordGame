using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    [SerializeField] private TextAsset[] levelJsonDatas;

    public static LevelDataManager Instance { get; private set; }


    private LevelData[] levelsData;

    public int LevelCount => levelJsonDatas.Length;
    public int CurrentLevelIndex { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadLevelsData();
    }

    public LevelData GetCurrentLevelData()
    {
        if (levelsData == null || CurrentLevelIndex >= levelsData.Length)
            LoadLevelsData();

        return levelsData[CurrentLevelIndex];
    }

    public LevelData GetLevelData(int levelIndex)
    {
        if (levelIndex >= levelsData.Length)
            LoadLevelsData();

        if (levelIndex >= levelsData.Length)
            return null;

        return levelsData[levelIndex];
    }

    private void LoadLevelsData()
    {
        levelsData = new LevelData[levelJsonDatas.Length];
        for (int i = 0; i < levelJsonDatas.Length; i++)
        {
            levelsData[i] = JSONLevelDataParser.ReadLevelDataFromJsonTextAsset(levelJsonDatas[i]);
        }
    }
}
